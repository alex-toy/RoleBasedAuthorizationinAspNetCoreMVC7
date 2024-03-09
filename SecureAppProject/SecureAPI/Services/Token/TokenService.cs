using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecureAPI.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateNewJsonWebToken(List<Claim> claims)
        {
            SymmetricSecurityKey authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            JwtSecurityToken tokenObject = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
            );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }
    }
}
