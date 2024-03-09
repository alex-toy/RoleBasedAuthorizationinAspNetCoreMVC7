using System.Security.Claims;

namespace SecureAPI.Services.Token
{
    public interface ITokenService
    {
        string GenerateNewJsonWebToken(List<Claim> claims);
    }
}