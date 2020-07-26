using System.Collections.Generic;
using API.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

namespace API.Infrastructure.Security
{
    public class JWTGenerator
    {
        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret key"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}