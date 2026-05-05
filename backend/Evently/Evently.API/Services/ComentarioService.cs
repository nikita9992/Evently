using Evently.API.Data;
using Evently.API.DTOs.Comentario;
using Evently.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Evently.API.Services
{
    public class ComentarioService : IComentarioService
    {
        private readonly EventlyDbContext _contexto;

        public ComentarioService(EventlyDbContext contexto)
        {
            _contexto = contexto;
        }

        // Obtener todos los comentarios de una actividad
        public async Task<List<ComentarioDto>> ObtenerPorActividadAsync(int idActividad)
        {
            return await _contexto.Comentarios
                .Include(c => c.Usuario)
                .Include(c => c.Usuario.Cliente)
                .Where(c => c.IdActividad == idActividad)
                .OrderByDescending(c => c.FechaCreacion)
                .Select(c => new ComentarioDto
                {
                    IdComentario = c.IdComentario,
                    IdActividad = c.IdActividad,
                    IdUsuario = c.IdUsuario,
                    NombreUsuario = c.Usuario.Cliente != null
                        ? c.Usuario.Cliente.Nombre + " " + c.Usuario.Cliente.Apellidos
                        : c.Usuario.Email,
                    Texto = c.Texto,
                    FechaCreacion = c.FechaCreacion
                })
                .ToListAsync();
        }

        // сrear comentario
        public async Task<ComentarioDto?> CrearAsync(int idActividad, int idUsuario,
            CrearComentarioDto dto)
        {
            var cliente = await _contexto.Clientes
                .FirstOrDefaultAsync(c => c.IdUsuario == idUsuario);

            if (cliente == null
                || string.IsNullOrEmpty(cliente.Nombre)
                || string.IsNullOrEmpty(cliente.Apellidos))
                return null;

            var comentario = new Comentario
            {
                IdActividad = idActividad,
                IdUsuario = idUsuario,
                Texto = dto.Texto,
                FechaCreacion = DateTime.UtcNow
            };

            _contexto.Comentarios.Add(comentario);
            await _contexto.SaveChangesAsync();

            return new ComentarioDto
            {
                IdComentario = comentario.IdComentario,
                IdActividad = comentario.IdActividad,
                IdUsuario = comentario.IdUsuario,
                NombreUsuario = cliente.Nombre + " " + cliente.Apellidos,
                Texto = comentario.Texto,
                FechaCreacion = comentario.FechaCreacion
            };
        }

        // Eliminar comentario
        public async Task<bool> EliminarAsync(int idComentario, int idUsuario, bool esAdmin)
        {
            var comentario = await _contexto.Comentarios
                .FirstOrDefaultAsync(c => c.IdComentario == idComentario);

            if (comentario == null) return false;

            if (!esAdmin && comentario.IdUsuario != idUsuario) return false;

            _contexto.Comentarios.Remove(comentario);
            await _contexto.SaveChangesAsync();
            return true;
        }
    }
}