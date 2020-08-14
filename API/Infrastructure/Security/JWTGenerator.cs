using System.Collections.Generic;
using API.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace API.Infrastructure.Security
{
    public class JWTGenerator
    {
        SymmetricSecurityKey key;
        private readonly UserManager<User> userManager;

        public JWTGenerator(IConfiguration config, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public async Task<string> CreateToken(User user)
        {
            var roles = await userManager.GetRolesAsync(user) as List<string>;

            List<Claim> claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId, user.Email)
            };

            foreach (var role in roles)
            {
                var claim = new Claim(ClaimsIdentity.DefaultRoleClaimType, role);
                claims.Add(claim);
            }

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