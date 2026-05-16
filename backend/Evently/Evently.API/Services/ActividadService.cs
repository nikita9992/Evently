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
                .Include(a => a.Imagenes)
                .AsQueryable();

            if (filtro.IdCategoria.HasValue)
                consulta = consulta.Where(a => a.IdCategoria == filtro.IdCategoria.Value);

            if (!string.IsNullOrEmpty(filtro.Titulo))
                consulta = consulta.Where(a =>
                    a.Titulo.ToLower().Contains(filtro.Titulo.ToLower()));

            var actividades = await consulta.ToListAsync();
            var actividadesDto = new List<ActividadDto>();

            foreach (var a in actividades)
            {
                var valoraciones = await _contexto.Valoraciones
                    .Where(v => v.IdActividad == a.IdActividad)
                    .ToListAsync();

                actividadesDto.Add(new ActividadDto
                {
                    IdActividad = a.IdActividad,
                    IdCategoria = a.IdCategoria,
                    NombreCatego = a.Categoria.NombreCatego,
                    Titulo = a.Titulo,
                    Descripcion = a.Descripcion,
                    Precio = a.Precio,
                    FechaActiv = a.FechaActiv,
                    CupoMaximo = a.CupoMaximo,
                    PlazasDisponibles = a.PlazasDisponibles,
                    EsDestacada = a.EsDestacada,
                    Imagenes = a.Imagenes.OrderBy(i => i.Orden).Select(i => i.Url).ToList(),
                    MediaValoracion = valoraciones.Any() ? Math.Round(valoraciones.Average(v => v.Puntuacion), 1) : 0,
                    TotalValoraciones = valoraciones.Count
                });
            }

            return actividadesDto;
        }

        // Obtener actividades marcadas para mostrarse en la página de inicio
        public async Task<List<ActividadDto>> ObtenerDestacadasAsync()
        {
            var actividades = await _contexto.Actividades
                .Include(a => a.Categoria)
                .Include(a => a.Imagenes)
                .Where(a => a.EsDestacada)
                .OrderBy(a => a.FechaActiv)
                .ToListAsync();

            var actividadesDto = new List<ActividadDto>();

            foreach (var a in actividades)
            {
                var valoraciones = await _contexto.Valoraciones
                    .Where(v => v.IdActividad == a.IdActividad)
                    .ToListAsync();

                actividadesDto.Add(new ActividadDto
                {
                    IdActividad = a.IdActividad,
                    IdCategoria = a.IdCategoria,
                    NombreCatego = a.Categoria.NombreCatego,
                    Titulo = a.Titulo,
                    Descripcion = a.Descripcion,
                    Precio = a.Precio,
                    FechaActiv = a.FechaActiv,
                    CupoMaximo = a.CupoMaximo,
                    PlazasDisponibles = a.PlazasDisponibles,
                    EsDestacada = a.EsDestacada,
                    Imagenes = a.Imagenes.OrderBy(i => i.Orden).Select(i => i.Url).ToList(),
                    MediaValoracion = valoraciones.Any() ? Math.Round(valoraciones.Average(v => v.Puntuacion), 1) : 0,
                    TotalValoraciones = valoraciones.Count
                });
            }

            return actividadesDto;
        }

        //Obtener una actividad por id
        public async Task<ActividadDto?> ObtenerPorIdAsync(int id)
        {
            var actividad = await _contexto.Actividades.Include(a => a.Categoria).Include(a => a.Imagenes).FirstOrDefaultAsync(a => a.IdActividad == id);

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
                CupoMaximo = actividad.CupoMaximo,
                PlazasDisponibles = actividad.PlazasDisponibles,
                EsDestacada = actividad.EsDestacada,
                Imagenes = actividad.Imagenes.OrderBy(i => i.Orden).Select(i => i.Url).ToList()
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
                ? DateTime.SpecifyKind(crearActividadDto.FechaActiv.Value, DateTimeKind.Utc) : null,
                CupoMaximo = crearActividadDto.CupoMaximo,
                EsDestacada = crearActividadDto.EsDestacada
            };

            _contexto.Actividades.Add(nuevaActividad);
            await _contexto.SaveChangesAsync();

            if (crearActividadDto.Imagenes.Any())
            {
                var imagenes = crearActividadDto.Imagenes.Take(8).Select((url, indice) => new ImagenActividad
                {
                    IdActividad = nuevaActividad.IdActividad,
                    Url = url,
                    Orden = indice
                });

                _contexto.ImagenesActividad.AddRange(imagenes);
                await _contexto.SaveChangesAsync();
            }

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
                CupoMaximo = nuevaActividad.CupoMaximo,
                PlazasDisponibles = nuevaActividad.PlazasDisponibles,
                EsDestacada = nuevaActividad.EsDestacada
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
            actividad.EsDestacada = crearActividadDto.EsDestacada;


            await _contexto.SaveChangesAsync();

            if (crearActividadDto.Imagenes != null)
            {

                var imagenesAnteriores = await _contexto.ImagenesActividad.Where(i => i.IdActividad == id).ToListAsync();
                _contexto.ImagenesActividad.RemoveRange(imagenesAnteriores);

                var nuevasImagenes = crearActividadDto.Imagenes.Take(8).Select((url, indice) => new ImagenActividad
                {
                    IdActividad = id,
                    Url = url,
                    Orden = indice
                });
                _contexto.ImagenesActividad.AddRange(nuevasImagenes);
                await _contexto.SaveChangesAsync();
            }

            return new ActividadDto
            {
                IdActividad = actividad.IdActividad,
                IdCategoria = actividad.IdCategoria,
                NombreCatego = actividad.Categoria.NombreCatego,
                Titulo = actividad.Titulo,
                Descripcion = actividad.Descripcion,
                Precio = actividad.Precio,
                FechaActiv = actividad.FechaActiv,
                CupoMaximo = actividad.CupoMaximo,
                PlazasDisponibles = actividad.PlazasDisponibles,
                EsDestacada = actividad.EsDestacada
            };
        }

        // Eliminar una actividad
        public async Task<bool?> EliminarAsync(int id)
        {
            var actividad = await _contexto.Actividades
                .FirstOrDefaultAsync(a => a.IdActividad == id);

            if (actividad == null) return null;

            bool tienePedidos = await _contexto.DetallesPedido.AnyAsync(d => d.IdActividad == id);

            if (tienePedidos) return false;

            _contexto.Actividades.Remove(actividad);
            await _contexto.SaveChangesAsync();

            return true;
        }
    }
}