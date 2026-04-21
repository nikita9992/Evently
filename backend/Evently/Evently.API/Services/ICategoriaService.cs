using Evently.API.DTOs.Categoria;

namespace Evently.API.Services
{
    // Interfaz que define los métodos para gestionar categorías
    public interface ICategoriaService
    {
        Task<List<CategoriaDto>> ObtenerTodasAsync();

        Task<CategoriaDto?> ObtenerPorIdAsync(int id);

        Task<CategoriaDto> CrearAsync(CrearCategoriaDto crearCategoriaDto);

        Task<CategoriaDto?> EditarAsync(int id, CrearCategoriaDto crearCategoriaDto);

        Task<bool> EliminarAsync(int id);
    }
}