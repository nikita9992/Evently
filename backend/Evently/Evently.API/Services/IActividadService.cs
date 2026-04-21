using Evently.API.DTOs.Actividad;

namespace Evently.API.Services
{
    // Interfaz que define los métodos para gestionar actividades
    public interface IActividadService
    {
        Task<List<ActividadDto>> ObtenerTodasAsync(FiltroActividadDto filtro);
        Task<ActividadDto?> ObtenerPorIdAsync(int id);
        Task<ActividadDto> CrearAsync(CrearActividadDto crearActividadDto);
        Task<ActividadDto?> EditarAsync(int id, CrearActividadDto crearActividadDto);
        Task<bool> EliminarAsync(int id);
    }
}