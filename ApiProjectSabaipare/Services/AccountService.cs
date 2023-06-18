using ApiProjectSabaipare.DTOs;
using ApiProjectSabaipare.Models;
using ApiProjectSabaipare.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiProjectSabaipare.Services
{
    public class AccountService : ControllerBase, IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManager<ApplicationUser> userManager, TokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Object>> GetUsersAsync()
        {
            // var result = await _userManager.Users.Select(x=>x.UserName).ToListAsync();


            var result = await _userManager.Users.ToListAsync();


            List<Object> users = new();


            foreach (var user in result)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                users.Add(new { user.UserName, userRole });
            }


            return users;
        }


        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);


            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return null;


            var userDto = new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
            };


            return userDto;
        }


        public async Task<object> RegisterAsync(RegisterDto registerDto)
        {
            var user = new ApplicationUser { UserName = registerDto.Username, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);


            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }


            await _userManager.AddToRoleAsync(user, registerDto.Role);
            return StatusCode(201);
        }


        public Object GetMe()
        {
            var username = string.Empty;
            var role = string.Empty;


            if (_httpContextAccessor.HttpContext != null)
            {
                username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            }
            return new { username, role };
        }

    }
}
