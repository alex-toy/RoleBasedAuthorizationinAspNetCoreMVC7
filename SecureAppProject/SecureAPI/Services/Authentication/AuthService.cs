using Microsoft.AspNetCore.Identity;
using SecureAPI.Dtos;
using SecureAPI.Entities;
using SecureAPI.Enums;
using SecureAPI.Services.Token;
using System.Security.Claims;

namespace SecureAPI.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseResponseDto> LoginAsync(LoginRequestDto loginDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user is null) return new LoginResponseResponseDto()
            {
                IsSuccess = false,
                Message = "Invalid Credentials"
            };

            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordCorrect) return new LoginResponseResponseDto()
            {
                IsSuccess = false,
                Message = "Invalid Credentials"
            };

            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            List<Claim> authClaims = GetClaims(user, userRoles);

            string token = _tokenService.GenerateNewJsonWebToken(authClaims);

            return new LoginResponseResponseDto()
            {
                IsSuccess = true,
                Message = token
            };
        }

        public async Task<LoginResponseResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

            if (user is null) return new LoginResponseResponseDto()
            {
                IsSuccess = false,
                Message = "Invalid User name!!!!!!!!"
            };

            await _userManager.AddToRoleAsync(user, UserRoles.ADMIN);

            return new LoginResponseResponseDto()
            {
                IsSuccess = true,
                Message = "User is now an ADMIN"
            };
        }

        public async Task<LoginResponseResponseDto> MakeOwnerAsync(UpdatePermissionDto updatePermissionDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);
            if (user is null) return new LoginResponseResponseDto()
            {
                IsSuccess = false,
                Message = "Invalid User name!!!!!!!!"
            };

            await _userManager.AddToRoleAsync(user, UserRoles.OWNER);

            return new LoginResponseResponseDto()
            {
                IsSuccess = true,
                Message = "User is now an OWNER"
            };
        }

        public async Task<LoginResponseResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            ApplicationUser? isExistsUser = await _userManager.FindByNameAsync(registerDto.UserName);
            if (isExistsUser != null) return new LoginResponseResponseDto()
            {
                IsSuccess = false,
                Message = "UserName Already Exists"
            };

            ApplicationUser newUser = new ApplicationUser()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            IdentityResult createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);
            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation Failed Beacause: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return new LoginResponseResponseDto()
                {
                    IsSuccess = false,
                    Message = errorString
                };
            }

            await _userManager.AddToRoleAsync(newUser, UserRoles.USER);

            return new LoginResponseResponseDto()
            {
                IsSuccess = true,
                Message = "User Created Successfully"
            };
        }

        public async Task<LoginResponseResponseDto> SeedRolesAsync()
        {
            bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(UserRoles.OWNER);
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(UserRoles.ADMIN);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(UserRoles.USER);

            if (isOwnerRoleExists && isAdminRoleExists && isUserRoleExists)
                return new LoginResponseResponseDto()
                {
                    IsSuccess = true,
                    Message = "Roles Seeding is Already Done"
                };

            await _roleManager.CreateAsync(new IdentityRole(UserRoles.USER));
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.OWNER));

            return new LoginResponseResponseDto()
            {
                IsSuccess = true,
                Message = "Role Seeding Done Successfully"
            };
        }

        private static List<Claim> GetClaims(ApplicationUser? user, IList<string> userRoles)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
            };

            authClaims.AddRange(userRoles.Select(ur => new Claim(ClaimTypes.Role, ur)));
            return authClaims;
        }
    }
}
