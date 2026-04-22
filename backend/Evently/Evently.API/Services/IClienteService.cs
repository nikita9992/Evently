using Evently.API.DTOs.Cliente;

namespace Evently.API.Services
{
    // Interfaz que define los métodos para gestionar clientes
    public interface IClienteService
    {
        Task<List<ClienteDto>> ObtenerTodosAsync();
        Task<ClienteDto?> ObtenerPorIdAsync(int id);
        Task<ClienteDto?> ObtenerPorUsuarioAsync(int idUsuario);
        Task<ClienteDto> CrearAsync(CrearClienteDto crearClienteDto);
        Task<ClienteDto?> EditarAsync(int id, CrearClienteDto crearClienteDto);
        Task<bool> EliminarAsync(int id);
    }
}