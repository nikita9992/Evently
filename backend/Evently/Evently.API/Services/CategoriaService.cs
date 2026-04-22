using Evently.API.Data;
using Evently.API.DTOs.Categoria;
using Evently.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Evently.API.Services
{
    //Servicio que gestiona las categorías del sistema
    public class CategoriaService : ICategoriaService
    {
        private readonly EventlyDbContext _contexto;

        public CategoriaService(EventlyDbContext contexto)
        {
            _contexto = contexto;
        }

        //Obtener todas las categorías
        public async Task<List<CategoriaDto>> ObtenerTodasAsync()
        {
            return await _contexto.Categorias
                .Select(c => new CategoriaDto
                {
                    IdCategoria = c.IdCategoria,
                    NombreCatego = c.NombreCatego,
                    Descripcion = c.Descripcion
                })
                .ToListAsync();
        }

        // Obtener una categoría por ID
        public async Task<CategoriaDto?> ObtenerPorIdAsync(int id)
        {
            var categoria = await _contexto.Categorias
                .FirstOrDefaultAsync(c => c.IdCategoria == id);

            if (categoria == null) return null;

            return new CategoriaDto
            {
                IdCategoria = categoria.IdCategoria,
                NombreCatego = categoria.NombreCatego,
                Descripcion = categoria.Descripcion
            };
        }

        //Crear una nueva categoría
        public async Task<CategoriaDto> CrearAsync(CrearCategoriaDto crearCategoriaDto)
        {
            var nuevaCategoria = new Categoria
            {
                NombreCatego = crearCategoriaDto.NombreCatego,
                Descripcion = crearCategoriaDto.Descripcion
            };

            _contexto.Categorias.Add(nuevaCategoria);
            await _contexto.SaveChangesAsync();

            return new CategoriaDto
            {
                IdCategoria = nuevaCategoria.IdCategoria,
                NombreCatego = nuevaCategoria.NombreCatego,
                Descripcion = nuevaCategoria.Descripcion
            };
        }

        // Editar una categoría existente
        public async Task<CategoriaDto?> EditarAsync(int id, CrearCategoriaDto crearCategoriaDto)
        {
            var categoria = await _contexto.Categorias
                .FirstOrDefaultAsync(c => c.IdCategoria == id);

            if (categoria == null) return null;

            categoria.NombreCatego = crearCategoriaDto.NombreCatego;
            categoria.Descripcion = crearCategoriaDto.Descripcion;

            await _contexto.SaveChangesAsync();

            return new CategoriaDto
            {
                IdCategoria = categoria.IdCategoria,
                NombreCatego = categoria.NombreCatego,
                Descripcion = categoria.Descripcion
            };
        }

        // Eliminar una categoría
        public async Task<bool> EliminarAsync(int id)
        {
            var categoria = await _contexto.Categorias
                .FirstOrDefaultAsync(c => c.IdCategoria == id);

            if (categoria == null) return false;

            _contexto.Categorias.Remove(categoria);
            await _contexto.SaveChangesAsync();

            return true;
        }
    }
}