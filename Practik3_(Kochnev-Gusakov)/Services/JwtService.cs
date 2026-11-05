using Microsoft.IdentityModel.Tokens;
using Practik3__Kochnev_Gusakov_.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Practik3__Kochnev_Gusakov_.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            
            var claims = new[]
            {
                
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                
                new Claim(ClaimTypes.Name, user.Login),
                
                new Claim(ClaimTypes.Role, user.Role)
            };

            
            var keySection = _configuration.GetSection("Jwt:Key").Value;
            if (string.IsNullOrEmpty(keySection))
            {
                
                keySection = "SuperSecretLongKeyThatHasAtLeast32Characters!";
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keySection));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            
            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("Jwt:Issuer").Value ?? "MyApi",
                audience: _configuration.GetSection("Jwt:Audience").Value ?? "MyApps",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7), 
                signingCredentials: creds
            );

            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
