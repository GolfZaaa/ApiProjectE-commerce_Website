using ApiProjectSabaipare.DTOs;
using ApiProjectSabaipare.Models;
using ApiProjectSabaipare.Services;
using ApiProjectSabaipare.Services.IService;
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
        private readonly IRoleService _roleService;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IRoleService roleService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _roleService = roleService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _roleService.GetAsync());
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Create(RoleDto roleDto)
        {
            var result = await _roleService.CreateAsync(roleDto);
            return Ok(result);
        }


        [HttpPut("[action]")]
        public async Task<IActionResult> Update(RoleUpdateDto roleUpdateDto)
        {
            var result = await _roleService.UpdateAsync(roleUpdateDto);
            return Ok(result);
        }


        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(RoleDto roleDto)
        {
            var result = await _roleService.DeleteAsync(roleDto);
            return Ok(result);
        }


    }
}
