using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BankingApi.Data;
using BankingApi.Models.Identity;
using BankingApi.Shared.requests;
using BankingApi.Shared.responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BankingApi.Shared.Repositories
{
    public interface IAuth
    {
        public string GenereateToken(string email, string UserId, IList<string> userRoles);
        public RefreshTokensModel GenerateRefreshToken(string UserId);
        public Task<LoginResponse> ReturnNewLoginResponseByRefreshToken(string refreshToken);
        public Task<GeneralResponse> RegisterUser(RegisterationRequest request);    
        public Task<LoginResponse> UserLogin(LoginRequest request);

    }



    public class AuthRepo : IAuth
    {

        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public AuthRepo(IConfiguration config, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public RefreshTokensModel GenerateRefreshToken(string UserId)
        {
            return new RefreshTokensModel
            {
                RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = UserId,
                ExpiresAt = DateTime.Now.AddDays(7),
                CreatedAt = DateTime.Now,
            };
        }

        public string GenereateToken(string email, string UserId, IList<string> userRoles)
        {

            var claims = new[]
            {
                new Claim(System.Security.Claims.ClaimTypes.Email, email),
                new Claim("UserId",UserId),
            };

            foreach (var item in userRoles)
            {
                claims.Append(new Claim(ClaimTypes.Role, item));
            }



            var SecretKey = _config.GetSection("Secrets:jwtSecret").Value;
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: "BankingApi",
                audience: "BankingApiUsers",
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            var SignedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return SignedToken;

        }

        public async Task<GeneralResponse> RegisterUser(RegisterationRequest request)
        {
            var CheckIfUserEmailExists = await _userManager.FindByEmailAsync(request.Email);
            if (CheckIfUserEmailExists is null)
            {
                var RegisterUser = new AppUser
                {
                    UserName = request.Email,
                    NormalizedUserName = request.Email.ToUpper(),
                    Email = request.Email,
                    NormalizedEmail = request.Email.ToUpper(),
                    PhoneNumber = request.PhoneNumber,
                    FullName = request.FullName,
                    Address = request.Address,
                    RegisteredAt = DateTime.Now,
                };

                var Result = await _userManager.CreateAsync(RegisterUser, request.Password);
                if (Result.Succeeded)
                {
                    var RoleAddResult = await _userManager.AddToRoleAsync(RegisterUser, Seedings.RolesSeeds.Customer);
                    if (RoleAddResult.Succeeded)
                    {
                        return new GeneralResponse
                        {
                            IsSuccessful = true,
                            Message = $"The User {request.Email} has been registered successfully"
                        };
                    }
                    else
                    {
                        await _userManager.DeleteAsync(RegisterUser);
                        var errors = string.Join(", ", RoleAddResult.Errors.Select(e => e.Description));
                        return new GeneralResponse
                        {
                            IsSuccessful = false,
                            Message = $"The User {request.Email} failed to register. Errors: {errors}"
                        };
                    }
                }
                else
                {
                    var errors = string.Join(", ", Result.Errors.Select(e => e.Description));
                    return new GeneralResponse
                    {
                        IsSuccessful = false,
                        Message = $"The User {request.Email} failed to register. Errors: {errors}"
                    };
                }

            }
            else
            {
                return new GeneralResponse
                {
                    IsSuccessful = false,
                    Message = $"The User {request.Email} failed to register, please check the validity of the data!"
                };
            }
        }

        public async Task<LoginResponse> ReturnNewLoginResponseByRefreshToken(string refreshToken)
        {
            var CheckIfRefreshTokenExists = await _db.RefreshTokenTable.Include(a => a.User)
                                            .FirstOrDefaultAsync(a => a.RefreshToken == refreshToken);

            if (CheckIfRefreshTokenExists is null)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = $"Invalid refresh token"
                };
            }
            else if (CheckIfRefreshTokenExists.IsActive)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = $"Refresh token has been revoked or expired, please login again"
                };
            }
            else
            {
                var UserRoles = await _userManager.GetRolesAsync(CheckIfRefreshTokenExists.User);
                var Jwt = GenereateToken(CheckIfRefreshTokenExists.User.Email, CheckIfRefreshTokenExists.UserId, UserRoles);
                var RefreshToken = GenerateRefreshToken(CheckIfRefreshTokenExists.UserId);

                CheckIfRefreshTokenExists.RevokedAt = DateTime.Now;
                _db.RefreshTokenTable.Update(CheckIfRefreshTokenExists);
                await _db.RefreshTokenTable.AddAsync(RefreshToken); 
                var Result = await _db.SaveChangesAsync();

                if (Result == 1)
                {
                    return new LoginResponse
                    {
                        IsSuccess = true,
                        Message = $"Token refreshed successfully",
                        Token = Jwt,
                        RefreshToken = RefreshToken.RefreshToken
                    };
                }
                else
                {
                    return new LoginResponse
                    {
                        IsSuccess = false,
                        Message = $"Unknown error has occurred, please try again",
                        Token = null,
                        RefreshToken = null
                    };
                }
            }
        }

        public async Task<LoginResponse> UserLogin(LoginRequest request)
        {
            var CheckIfUserEmailExists = await _userManager.FindByEmailAsync(request.UserEmail);
            if (CheckIfUserEmailExists is null)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = $"UserName or password is incorrect"
                };
            }
            else
            {
                var CheckUserPassword = await _userManager.CheckPasswordAsync(CheckIfUserEmailExists, request.Password);
                if (CheckUserPassword == true)
                {
                    var UserRoles  = await _userManager.GetRolesAsync(CheckIfUserEmailExists);
                    var RefreshTokenObject = GenerateRefreshToken(CheckIfUserEmailExists.Id);

                     await _db.RefreshTokenTable.AddAsync(RefreshTokenObject);
                    var ResultOfSavingToDb = await _db.SaveChangesAsync();
                    if (ResultOfSavingToDb == 1)
                    {
                        return new LoginResponse
                        {
                            IsSuccess = true,
                            Message = "Login success",
                            Token = GenereateToken(CheckIfUserEmailExists.Email, CheckIfUserEmailExists.Id, UserRoles),
                            RefreshToken = RefreshTokenObject.RefreshToken
                        };
                    }
                    else
                    {
                        return new LoginResponse
                        {
                            IsSuccess = false,
                            Message = $"Unknown Error has occurred, please try again",
                            Token = null,
                        };
                    }
                   
                }
                else
                {
                    return new LoginResponse
                    {
                        IsSuccess = false,
                        Message = $"UserName or password is incorrect",
                        Token = null
                    };
                }
            }
        }
    }



}