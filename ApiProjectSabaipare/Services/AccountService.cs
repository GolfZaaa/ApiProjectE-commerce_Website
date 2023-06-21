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
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(UserManager<ApplicationUser> userManager, TokenService tokenService, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
        }

        public async Task<List<Object>> GetUsersAsync()
        {
            var result = await _userManager.Users.ToListAsync();
            List<Object> users = new();

            foreach (var user in result)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                var useremail = await _userManager.GetEmailAsync(user);
                var userid = await _userManager.GetUserIdAsync(user);
                users.Add(new { user.UserName, userRole,useremail, userid });
            }
            return users;
        }



        public async Task<object> LoginAsync(LoginDto loginDto)
        {
            var check = await _userManager.FindByNameAsync(loginDto.Username);

            if (check == null || !await _userManager.CheckPasswordAsync(check, loginDto.Password))
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseReport { Status = "404", Message = "Invalid username or password. Please try again." });
            }

            else
            {
                var userDto = new UserDto
                {
                    Email = check.Email,
                    Token = await _tokenService.GenerateToken(check),
                };
                return (userDto);
            }
        }


        public async Task<object> RegisterAsync(RegisterDto registerDto)
        {
            var check = await _userManager.FindByEmailAsync(registerDto.Email);
            if (check != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "404", Message = "This e-mail has already been used." });
            }
            var roleExists = await _roleManager.RoleExistsAsync(registerDto.Role);
            if (!roleExists)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "404", Message = "The specified role does not exist." });
            }
            var createuser = new ApplicationUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result = await _userManager.CreateAsync(createuser, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            await _userManager.AddToRoleAsync(createuser, registerDto.Role);
            await _userManager.UpdateAsync(createuser);
            return StatusCode(StatusCodes.Status201Created, new ResponseReport { Status = "201", Message = "Create Successfully" });
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

        public async Task<object> GetSingleUserAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseReport { Status = "404", Message = "Username Not Found" });
            }

            var userRole = await _userManager.GetRolesAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var securitystamp = await _userManager.GetSecurityStampAsync(user);
            var userid = await _userManager.GetUserIdAsync(user);
            var name = await _userManager.GetUserNameAsync(user);

            var userDetails = new { name, userRole, email, securitystamp, userid };

            return userDetails;
        }

        public async Task<object> DeleteAsync(string username)
        {
            var check = await _userManager.FindByNameAsync(username);

            if (check != null)
            {
                await _userManager.DeleteAsync(check);
                return Ok(new ResponseReport { Status = "201", Message = "Deleted Successfully" });
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseReport { Status = "404", Message = "Failed to Delete" });
            }
        }

    }
}
