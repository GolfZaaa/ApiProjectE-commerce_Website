using ApiProjectSabaipare.DTOs;
using Microsoft.AspNetCore.Identity;

namespace ApiProjectSabaipare.Services.IService
{
    public interface IRoleService
    {
        Task<List<IdentityRole>> GetAsync();
        Task<Object> CreateAsync(RoleDto roleDto);
        Task<Object> UpdateAsync(RoleUpdateDto roleUpdateDto);
        Task<Object> DeleteAsync(RoleDto roleDto);

    }
}
