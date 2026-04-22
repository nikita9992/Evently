using Evently.API.Data;
using Evently.API.DTOs.Estado;
using Evently.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Evently.API.Services
{
    // Servicio que gestiona los estados del sistema
    public class EstadoService : IEstadoService
    {
        private readonly EventlyDbContext _contexto;

        public EstadoService(EventlyDbContext contexto)
        {
            _contexto = contexto;
        }

        // Obtener todos los estados
        public async Task<List<EstadoDto>> ObtenerTodosAsync()
        {
            return await _contexto.Estados
                .Select(e => new EstadoDto
                {
                    IdEstado = e.IdEstado,
                    NombreEstado = e.NombreEstado,
                    DescripEstado = e.DescripEstado
                })
                .ToListAsync();
        }

        // Obtener un estado por ID
        public async Task<EstadoDto?> ObtenerPorIdAsync(int id)
        {
            var estado = await _contexto.Estados
                .FirstOrDefaultAsync(e => e.IdEstado == id);

            if (estado == null) return null;

            return new EstadoDto
            {
                IdEstado = estado.IdEstado,
                NombreEstado = estado.NombreEstado,
                DescripEstado = estado.DescripEstado
            };
        }

        // Crear un nuevo estado
        public async Task<EstadoDto> CrearAsync(CrearEstadoDto crearEstadoDto)
        {
            var nuevoEstado = new Estado
            {
                NombreEstado = crearEstadoDto.NombreEstado,
                DescripEstado = crearEstadoDto.DescripEstado
            };

            _contexto.Estados.Add(nuevoEstado);
            await _contexto.SaveChangesAsync();

            return new EstadoDto
            {
                IdEstado = nuevoEstado.IdEstado,
                NombreEstado = nuevoEstado.NombreEstado,
                DescripEstado = nuevoEstado.DescripEstado
            };
        }

        // Editar un estado existente
        public async Task<EstadoDto?> EditarAsync(int id, CrearEstadoDto crearEstadoDto)
        {
            var estado = await _contexto.Estados
                .FirstOrDefaultAsync(e => e.IdEstado == id);

            if (estado == null) return null;

            estado.NombreEstado = crearEstadoDto.NombreEstado;
            estado.DescripEstado = crearEstadoDto.DescripEstado;

            await _contexto.SaveChangesAsync();

            return new EstadoDto
            {
                IdEstado = estado.IdEstado,
                NombreEstado = estado.NombreEstado,
                DescripEstado = estado.DescripEstado
            };
        }

        // Eliminar un estado
        public async Task<bool> EliminarAsync(int id)
        {
            var estado = await _contexto.Estados
                .FirstOrDefaultAsync(e => e.IdEstado == id);

            if (estado == null) return false;

            _contexto.Estados.Remove(estado);
            await _contexto.SaveChangesAsync();

            return true;
        }
    }
}