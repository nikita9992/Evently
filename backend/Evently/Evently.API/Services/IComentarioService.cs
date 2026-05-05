using Evently.API.DTOs.Comentario;

namespace Evently.API.Services
{
    public interface IComentarioService
    {
        // Obtener comentarios de una actividad
        Task<List<ComentarioDto>> ObtenerPorActividadAsync(int idActividad);

        // Crear comentario 
        Task<ComentarioDto?> CrearAsync(int idActividad, int idUsuario,
            CrearComentarioDto dto);

        // Eliminar comentario
        Task<bool> EliminarAsync(int idComentario, int idUsuario, bool esAdmin);
    }
}