using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace BankingApi.Shared.Repositories
{
    public interface IAuth
    {

        public string GenereateToken(string email, double nationalId);


    }



    public class AuthRepo : IAuth
    {

        private readonly IConfiguration _config;

        public AuthRepo(IConfiguration config)
        {
            _config = config;
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




    }



}