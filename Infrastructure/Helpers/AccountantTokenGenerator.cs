using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public class AccountantTokenGenerator : ITokenGenerator<Accountant>
    {
        private readonly JwtSecret _jwtSecret;
        public AccountantTokenGenerator(IOptions<JwtSecret> jwtSecret)
        {
            _jwtSecret = jwtSecret.Value;
        }
        public string GenerateToken(Accountant accountant)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
            new Claim(ClaimTypes.NameIdentifier, accountant.Id.ToString()),
            new Claim(ClaimTypes.GivenName, accountant.FirstName),
            new Claim(ClaimTypes.Surname, accountant.LastName),
            new Claim(ClaimTypes.Email, accountant.Email),
            new Claim(ClaimTypes.Role, accountant.Role)
        }),
                Expires = DateTime.UtcNow.AddDays(7), // Token validity of 7 days
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
