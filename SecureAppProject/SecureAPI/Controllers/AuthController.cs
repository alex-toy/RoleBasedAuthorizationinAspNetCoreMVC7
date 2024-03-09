using Microsoft.AspNetCore.Mvc;
using SecureAPI.Dtos;
using SecureAPI.Services.Authentication;

namespace JwtAuthAspNet7WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    { 
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Route For Seeding my roles to DB
        [HttpPost]
        [Route("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        {
            LoginResponseResponseDto seerRoles = await _authService.SeedRolesAsync();

            return Ok(seerRoles);
        }

        // Route -> Register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            LoginResponseResponseDto registerResult = await _authService.RegisterAsync(registerDto);

            if (registerResult.IsSuccess) return Ok(registerResult);

            return BadRequest(registerResult);
        }

        // Route -> Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            LoginResponseResponseDto loginResult = await _authService.LoginAsync(loginDto);

            if(loginResult.IsSuccess) return Ok(loginResult);

            return Unauthorized(loginResult);
        }

        // Route -> make user -> admin
        [HttpPost]
        [Route("make-admin")]
        public async Task<IActionResult> MakeAdmin([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            LoginResponseResponseDto loginResponse = await _authService.MakeAdminAsync(updatePermissionDto);

            if(loginResponse.IsSuccess) return Ok(loginResponse);

            return BadRequest(loginResponse);
        }

        // Route -> make user -> owner
        [HttpPost]
        [Route("make-owner")]
        public async Task<IActionResult> MakeOwner([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            LoginResponseResponseDto operationResult = await _authService.MakeOwnerAsync(updatePermissionDto);

            if (operationResult.IsSuccess) return Ok(operationResult);

            return BadRequest(operationResult);
        }
    }
}

