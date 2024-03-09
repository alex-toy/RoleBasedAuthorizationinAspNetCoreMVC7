using SecureAPI.Dtos;

namespace SecureAPI.Services.Authentication
{
    public interface IAuthService
    {
        Task<LoginResponseResponseDto> SeedRolesAsync();
        Task<LoginResponseResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<LoginResponseResponseDto> LoginAsync(LoginRequestDto loginDto);
        Task<LoginResponseResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto);
        Task<LoginResponseResponseDto> MakeOwnerAsync(UpdatePermissionDto updatePermissionDto);
    }
}
