using Evently.API.Data;
using Evently.API.DTOs.Actividad;
using Evently.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Evently.API.Services
{
    public class ActividadService : IActividadService
    {
        private readonly EventlyDbContext _contexto;

        public ActividadService(EventlyDbContext contexto)
        {
            _contexto = contexto;
        }

        //Obtener todas las actividades con filtros opcionales
        public async Task<List<ActividadDto>> ObtenerTodasAsync(FiltroActividadDto filtro)
        {
            var consulta = _contexto.Actividades
                .Include(a => a.Categoria)
                .AsQueryable();

            if (filtro.IdCategoria.HasValue)
                consulta = consulta.Where(a => a.IdCategoria == filtro.IdCategoria.Value);

            if (!string.IsNullOrEmpty(filtro.Titulo))
                consulta = consulta.Where(a => a.Titulo.Contains(filtro.Titulo));

            return await consulta
                .Select(a => new ActividadDto
                {
                    IdActividad = a.IdActividad,
                    IdCategoria = a.IdCategoria,
                    NombreCatego = a.Categoria.NombreCatego,
                    Titulo = a.Titulo,
                    Descripcion = a.Descripcion,
                    Precio = a.Precio,
                    FechaActiv = a.FechaActiv,
                    CupoMaximo = a.CupoMaximo
                })
                .ToListAsync();
        }

        //Obtener una actividad por id
        public async Task<ActividadDto?> ObtenerPorIdAsync(int id)
        {
            var actividad = await _contexto.Actividades
                .Include(a => a.Categoria)
                .FirstOrDefaultAsync(a => a.IdActividad == id);

            if (actividad == null) return null;

            return new ActividadDto
            {
                IdActividad = actividad.IdActividad,
                IdCategoria = actividad.IdCategoria,
                NombreCatego = actividad.Categoria.NombreCatego,
                Titulo = actividad.Titulo,
                Descripcion = actividad.Descripcion,
                Precio = actividad.Precio,
                FechaActiv = actividad.FechaActiv,
                CupoMaximo = actividad.CupoMaximo
            };
        }

        //Crear una nueva actividad
        public async Task<ActividadDto> CrearAsync(CrearActividadDto crearActividadDto)
        {
            var nuevaActividad = new Actividad
            {
                IdCategoria = crearActividadDto.IdCategoria,
                Titulo = crearActividadDto.Titulo,
                Descripcion = crearActividadDto.Descripcion,
                Precio = crearActividadDto.Precio,
                FechaActiv = crearActividadDto.FechaActiv.HasValue
                            ? DateTime.SpecifyKind(crearActividadDto.FechaActiv.Value, DateTimeKind.Utc)
                            : null,
                CupoMaximo = crearActividadDto.CupoMaximo
            };

            _contexto.Actividades.Add(nuevaActividad);
            await _contexto.SaveChangesAsync();

            await _contexto.Entry(nuevaActividad)
                .Reference(a => a.Categoria)
                .LoadAsync();

            return new ActividadDto
            {
                IdActividad = nuevaActividad.IdActividad,
                IdCategoria = nuevaActividad.IdCategoria,
                NombreCatego = nuevaActividad.Categoria.NombreCatego,
                Titulo = nuevaActividad.Titulo,
                Descripcion = nuevaActividad.Descripcion,
                Precio = nuevaActividad.Precio,
                FechaActiv = nuevaActividad.FechaActiv,
                CupoMaximo = nuevaActividad.CupoMaximo
            };
        }

        // Editar una actividad
        public async Task<ActividadDto?> EditarAsync(int id, CrearActividadDto crearActividadDto)
        {
            var actividad = await _contexto.Actividades
                .Include(a => a.Categoria)
                .FirstOrDefaultAsync(a => a.IdActividad == id);

            if (actividad == null) return null;

            actividad.IdCategoria = crearActividadDto.IdCategoria;
            actividad.Titulo = crearActividadDto.Titulo;
            actividad.Descripcion = crearActividadDto.Descripcion;
            actividad.Precio = crearActividadDto.Precio;
            actividad.FechaActiv = crearActividadDto.FechaActiv.HasValue
                                ? DateTime.SpecifyKind(crearActividadDto.FechaActiv.Value, DateTimeKind.Utc)
                                : null;
            actividad.CupoMaximo = crearActividadDto.CupoMaximo;

            await _contexto.SaveChangesAsync();

            return new ActividadDto
            {
                IdActividad = actividad.IdActividad,
                IdCategoria = actividad.IdCategoria,
                NombreCatego = actividad.Categoria.NombreCatego,
                Titulo = actividad.Titulo,
                Descripcion = actividad.Descripcion,
                Precio = actividad.Precio,
                FechaActiv = actividad.FechaActiv,
                CupoMaximo = actividad.CupoMaximo
            };
        }

        // Eliminar una actividad
        public async Task<bool> EliminarAsync(int id)
        {
            var actividad = await _contexto.Actividades
                .FirstOrDefaultAsync(a => a.IdActividad == id);

            if (actividad == null) return false;

            _contexto.Actividades.Remove(actividad);
            await _contexto.SaveChangesAsync();

            return true;
        }
    }
}