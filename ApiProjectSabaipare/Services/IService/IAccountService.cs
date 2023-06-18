using ApiProjectSabaipare.DTOs;

namespace ApiProjectSabaipare.Services.IService
{
    public interface IAccountService
    {
        Task<List<Object>> GetUsersAsync();
        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<Object> RegisterAsync(RegisterDto registerDto);
        Object GetMe();

    }
}
