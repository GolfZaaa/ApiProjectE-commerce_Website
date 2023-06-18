using ApiProjectSabaipare.DTOs;
using ApiProjectSabaipare.Models;
using ApiProjectSabaipare.Services;
using ApiProjectSabaipare.Services.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ApiProjectSabaipare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService, UserManager<ApplicationUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _accountService.GetUsersAsync();
            return Ok(users);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _accountService.LoginAsync(loginDto);

            if (user == null) return Unauthorized();

            return Ok(user);
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var result = await _accountService.RegisterAsync(registerDto);
            return Ok(result);
        }


        [HttpGet("TestAdminRole"), Authorize(Roles = "Admin")]
        public IActionResult test()
        {
            return Ok("Authorize Success");
        }


        [HttpGet("GetMeByContext"), Authorize]
        public IActionResult GetMe()
        {
            var result = _accountService.GetMe();
            return Ok(result);
        }


        [HttpGet("GetMeInBaseController"), Authorize]
        public async Task<IActionResult> GetMyName()
        {
            //var userName = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new { user.UserName, roles });
        }


        [HttpGet("GetToken"), Authorize]
        public async Task<IActionResult> GetToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            if (accessToken == null)
            {
                accessToken = "Not Login";
            }

            return Ok(accessToken);
        }


    }
}
