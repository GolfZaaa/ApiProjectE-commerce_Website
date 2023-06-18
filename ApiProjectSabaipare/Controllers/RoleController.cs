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
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _roleManager.Roles.ToListAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleDto roleDto)
        {
            var identityRole = new IdentityRole
            {
                Name = roleDto.Name,
                NormalizedName = _roleManager.NormalizeKey(roleDto.Name)
            };

            var result = await _roleManager.CreateAsync(identityRole);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }

            return StatusCode(201);
        }


        [HttpPut]
        public async Task<IActionResult> Update(RoleUpdateDto roleUpdateDto)
        {
            var identityRole = await _roleManager.FindByNameAsync(roleUpdateDto.Name);

            if (identityRole == null) return NotFound();

            identityRole.Name = roleUpdateDto.UpdateName;
            identityRole.NormalizedName = _roleManager.NormalizeKey(roleUpdateDto.UpdateName);

            var result = await _roleManager.UpdateAsync(identityRole);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }

            return StatusCode(201);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(RoleDto roleDto)
        {
            var identityRole = await _roleManager.FindByNameAsync(roleDto.Name);


            if (identityRole == null) return NotFound();


            //ตรวจสอบมีผู้ใช้บทบาทนี้หรือไม่
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleDto.Name);
            if (usersInRole.Count != 0) return BadRequest();

            var result = await _roleManager.DeleteAsync(identityRole);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }

            return StatusCode(201);
        }


    }
}
