using garagedoor.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace garagedoor.Server.Controllers
{

    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration) => _configuration = configuration;

        [HttpPost("api/login")]

        public LoginResult Login(Credentials credentials)
        {
            var expiry = DateTime.Now.AddDays(30);
            return ValidateCredentials(credentials) ? new LoginResult { Token = GenerateJWT(credentials.Email, expiry), Expiry = expiry } : new LoginResult();
        }

        bool ValidateCredentials(Credentials credentials)
        {
          var user = _configuration.GetSection("Credentials").Get<Credentials>();
          var passwordHasher = new PasswordHasher<string>();
          return passwordHasher.VerifyHashedPassword(null, user.Password, credentials.Password) == PasswordVerificationResult.Success;
        }

        private string GenerateJWT(string email, DateTime expiry)
        {
          var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
          var token = new JwtSecurityToken(
              _configuration["Jwt:Issuer"],
              _configuration["Jwt:Audience"],
              new[] { new Claim(ClaimTypes.Name, email) },
              expires: expiry,
              signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
          );
          var tokenHandler = new JwtSecurityTokenHandler();
          return tokenHandler.WriteToken(token);
        }
    }
}