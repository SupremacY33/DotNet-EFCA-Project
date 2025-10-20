using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IUserServices _userServices;
        private readonly IConfiguration _configuration;

        public AuthServices(IUserServices userServices, IConfiguration configuration)
        {
            _userServices = userServices;
            _configuration = configuration;
        }

        // Implement authentication methods here

        public async Task<string?> AuthenticationAsync(LoginDTO loginDTO)
        {
            var user = await _userServices.GetByUsernameAsync(loginDTO.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
            {
                return null; // Authentication failed
            }
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var jwtKey = _configuration["Jwt:Key"];

                if (string.IsNullOrWhiteSpace(jwtKey))
                {
                    throw new InvalidOperationException("JWT Key is not configured.");
                }
                
                var key = Encoding.UTF8.GetBytes(jwtKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
        }
    }
}
