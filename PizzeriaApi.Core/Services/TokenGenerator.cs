using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.UtilModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Core.Services
{
    public class TokenGenerator: ITokenGenerator
    {

        private readonly JwtSettings _jwtSettings;

        public TokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            
        }

        public string GenerateToken(PizzeriaUser pizzeriaUser)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,pizzeriaUser.Id),
                new Claim("Username",pizzeriaUser.UserName),
                new Claim("UserID",pizzeriaUser.Id),

            };

            var adminUsername = _jwtSettings.AdminUsername;

            if (pizzeriaUser.UserName == adminUsername)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtSettings.TokenExpirationInHours),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    
    
}
