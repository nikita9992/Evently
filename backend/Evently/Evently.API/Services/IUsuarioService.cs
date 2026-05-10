using Evently.API.DTOs.Usuario;

namespace Evently.API.Services
{
    // Interfaz que define los métodos para gestionar usuarios
    public interface IUsuarioService
    {
        Task<List<UsuarioAdminDto>> ObtenerTodosAsync();
        Task<UsuarioAdminDto?> ObtenerPorIdAsync(int id);
        Task<UsuarioAdminDto?> CrearAsync(CrearUsuarioDto crearUsuarioDto);
        Task<UsuarioAdminDto?> CambiarRolAsync(int id, CambiarRolUsuarioDto cambiarRolUsuarioDto);
        Task<bool> TieneDatosAsociadosAsync(int id);
        Task<bool> EliminarAsync(int id);
    }
}