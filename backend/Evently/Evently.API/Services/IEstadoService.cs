using Evently.API.DTOs.Estado;

namespace Evently.API.Services
{
    // Interfaz que define los métodos para gestionar estados
    public interface IEstadoService
    {
        Task<List<EstadoDto>> ObtenerTodosAsync();
        Task<EstadoDto?> ObtenerPorIdAsync(int id);
        Task<EstadoDto> CrearAsync(CrearEstadoDto crearEstadoDto);
        Task<EstadoDto?> EditarAsync(int id, CrearEstadoDto crearEstadoDto);
        Task<bool> EliminarAsync(int id);
    }
}