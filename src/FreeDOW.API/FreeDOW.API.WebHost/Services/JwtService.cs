using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FreeDOW.API.WebHost.Services
{
    public class JwtService
    {
        private readonly string _jwtKey;
        private readonly IConfiguration _config;
        public JwtService(IConfiguration config)
        {
            _config = config;
            _jwtKey = config.GetSection("JwtKey").Value;
        }

        public string GenerateRefreshToken(IConfiguration config)
        {
            return String.Empty;
        }

        public string CreateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var issuer = "BustlerGreen";
            var audience = "FreeDOW";
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
