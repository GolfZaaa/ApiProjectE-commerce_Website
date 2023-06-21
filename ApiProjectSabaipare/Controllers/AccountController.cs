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

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSingleUser(string username)
        {
            var result = await _accountService.GetSingleUserAsync(username);
            return Ok(result);
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

        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete([FromForm] string username)
        {
            var result = await _accountService.DeleteAsync(username);
            return Ok(result);
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string password, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                // ไม่พบผู้ใช้ที่เข้าสู่ระบบ
                return Unauthorized();
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, password, newPassword);
            if (!changePasswordResult.Succeeded)
            {
                // ไม่สามารถเปลี่ยนรหัสผ่านได้
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "400", Message = "The password you entered is incorrect. Please try again." });
            }
            //เช็ค Error ถ้ารหัสใหม่กับรหัสเก่าซ้ำกัน
            if (password == newPassword)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseReport { Status = "400", Message = "The new password is the same as the current password you are using. Please enter a password that is different from the current one." });
            }
            // เปลี่ยนรหัสผ่านสำเร็จ
            return Ok(StatusCode(StatusCodes.Status200OK, new ResponseReport { Status = "200", Message = "ChangePassword Successfuly" }));
        }

        [HttpPost("ChangeEmail")]
        [Authorize]
        public async Task<IActionResult> ChangeUserEmail(string NewEmail)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                // หา User ไม่เจอ
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "400", Message = "User Not Found" });
            }

            if (user.Email == NewEmail)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseReport { Status = "400", Message = "The new Email is the same as the current Email you are using. Please enter a Email that is different from the current one." });
            }
            var result = await _userManager.SetEmailAsync(user, NewEmail);
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                // เกิดข้อผิดพลาดในการตั้งค่าอีเมลใหม่
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "400", Message = "Fail to SetEmail" });
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseReport { Status = "200", Message = "Change Email Successfuly" });
        }


        [HttpPost("[action]"), Authorize]
        public async Task<IActionResult> ChangeUserName(string NewUserName)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "400", Message = "User Not Found" });
            }

            if (string.IsNullOrEmpty(User.Identity.Name))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "400", Message = "User Not Found" });
            }

            if (user.UserName == NewUserName)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseReport { Status = "400", Message = "The new UserName is the same as the current UserName you are using. Please enter a UserName that is different from the current one." });
            }

            var result = await _userManager.SetUserNameAsync(user, NewUserName);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "400", Message = "Fail to SetUserName" });
            }
            return StatusCode(StatusCodes.Status200OK, new ResponseReport { Status = "200", Message = "Change UserName Successfully, please logout and login to get token again" });
        }


    }
}
