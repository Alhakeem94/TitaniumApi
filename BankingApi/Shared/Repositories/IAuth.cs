using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BankingApi.Models.Identity;
using BankingApi.Shared.requests;
using BankingApi.Shared.responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BankingApi.Shared.Repositories
{
    public interface IAuth
    {
        public string GenereateToken(string email, string UserId, IList<string> userRoles);
        public Task<GeneralResponse> RegisterUser(RegisterationRequest request);    
        public Task<LoginResponse> UserLogin(LoginRequest request);

    }



    public class AuthRepo : IAuth
    {

        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthRepo(IConfiguration config, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
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
                expires: DateTime.Now.AddHours(1),
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
                    return new LoginResponse
                    {
                        IsSuccess = true,
                        Message = "Login success",
                        Token = GenereateToken(CheckIfUserEmailExists.Email, CheckIfUserEmailExists.Id,UserRoles)
                    };
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