using Evently.API.Data;
using Evently.API.DTOs.Valoracion;
using Evently.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Evently.API.Services
{
    public class ValoracionService : IValoracionService
    {
        private readonly EventlyDbContext _contexto;

        public ValoracionService(EventlyDbContext contexto)
        {
            _contexto = contexto;
        }

        // obtener resumen de valoraciones
        public async Task<ResumenValoracionDto> ObtenerResumenAsync(
            int idActividad, int? idUsuario)
        {
            var valoraciones = await _contexto.Valoraciones
                .Where(v => v.IdActividad == idActividad)
                .ToListAsync();

            var media = valoraciones.Any()
                ? valoraciones.Average(v => v.Puntuacion)
                : 0;

            int? valoracionUsuario = null;
            if (idUsuario.HasValue)
            {
                var valoracionExistente = valoraciones
                    .FirstOrDefault(v => v.IdUsuario == idUsuario.Value);
                valoracionUsuario = valoracionExistente?.Puntuacion;
            }

            return new ResumenValoracionDto
            {
                Media = Math.Round(media, 1),
                TotalValoraciones = valoraciones.Count,
                ValoracionUsuario = valoracionUsuario
            };
        }

        // Crear/actualizar valoración
        public async Task<ValoracionDto?> CrearOActualizarAsync(
            int idActividad, int idUsuario, CrearValoracionDto dto)
        {
            var compro = await UsuarioComproActividadAsync(idActividad, idUsuario);
            if (!compro) return null;

            var valoracionExistente = await _contexto.Valoraciones
                .FirstOrDefaultAsync(v =>
                    v.IdActividad == idActividad &&
                    v.IdUsuario == idUsuario);

            if (valoracionExistente != null)
            {
                valoracionExistente.Puntuacion = dto.Puntuacion;
                await _contexto.SaveChangesAsync();

                return new ValoracionDto
                {
                    IdValoracion = valoracionExistente.IdValoracion,
                    IdActividad = valoracionExistente.IdActividad,
                    IdUsuario = valoracionExistente.IdUsuario,
                    Puntuacion = valoracionExistente.Puntuacion,
                    FechaCreacion = valoracionExistente.FechaCreacion
                };
            }

            var nuevaValoracion = new Valoracion
            {
                IdActividad = idActividad,
                IdUsuario = idUsuario,
                Puntuacion = dto.Puntuacion,
                FechaCreacion = DateTime.UtcNow
            };

            _contexto.Valoraciones.Add(nuevaValoracion);
            await _contexto.SaveChangesAsync();

            return new ValoracionDto
            {
                IdValoracion = nuevaValoracion.IdValoracion,
                IdActividad = nuevaValoracion.IdActividad,
                IdUsuario = nuevaValoracion.IdUsuario,
                Puntuacion = nuevaValoracion.Puntuacion,
                FechaCreacion = nuevaValoracion.FechaCreacion
            };
        }

        // Verificar si el usuario compró la actividad
        public async Task<bool> UsuarioComproActividadAsync(
            int idActividad, int idUsuario)
        {
            return await _contexto.DetallesPedido
                .Include(dp => dp.Pedido)
                    .ThenInclude(p => p.Cliente)
                .Include(dp => dp.Pedido)
                    .ThenInclude(p => p.Estado)
                .AnyAsync(dp =>
                    dp.IdActividad == idActividad &&
                    dp.Pedido.Cliente.IdUsuario == idUsuario &&
                    dp.Pedido.Estado.NombreEstado == "Confirmado");
        }
    }
}