using ApiProjectSabaipare.DTOs;
using ApiProjectSabaipare.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProjectSabaipare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _userManager.Users.ToListAsync();
            List<Object> users = new();
            foreach (var user in result)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                users.Add(new { user.UserName, userRole });
            }
            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Unauthorized();
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
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


    }
}
