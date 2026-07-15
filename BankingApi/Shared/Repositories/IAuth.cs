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
        public string GenereateToken(string email, double nationalId);
        public Task<GeneralResponse> RegisterUser(RegisterationRequest request);    

    }



    public class AuthRepo : IAuth
    {

        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;


        public AuthRepo(IConfiguration config, UserManager<AppUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public string GenereateToken(string email, double nationalId)
        {

            var claims = new[]
            {
                new Claim(System.Security.Claims.ClaimTypes.Email, email),
                new Claim("NationalId", nationalId.ToString())
            };

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
                    return new GeneralResponse
                    {
                        IsSuccessful = true,
                        Message = $"The User {request.Email} has been registered successfully"
                    };
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
    }



}