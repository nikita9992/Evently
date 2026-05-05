using Evently.API.DTOs.Valoracion;

namespace Evently.API.Services
{
    public interface IValoracionService
    {
        Task<ResumenValoracionDto> ObtenerResumenAsync(int idActividad, int? idUsuario);

        // Crear /actualizar valoracion
        Task<ValoracionDto?> CrearOActualizarAsync(int idActividad, int idUsuario,
            CrearValoracionDto dto);

        // Verificar si el usuario compró la actividad
        Task<bool> UsuarioComproActividadAsync(int idActividad, int idUsuario);
    }
}