using Evently.API.DTOs.Auth;

namespace Evently.API.Services
{
    public interface IAutenticacionService
    {
        Task<RespuestaAuthDto?> RegistrarAsync(RegistroDto registroDto);
        Task<RespuestaAuthDto?> LoginAsync(LoginDto loginDto);
    }
}