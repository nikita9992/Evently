using Evently.API.DTOs.Categoria;
using Evently.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evently.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        //GET api/categorias > cualquier usuario puede ver las categorías
        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var categorias = await _categoriaService.ObtenerTodasAsync();
            return Ok(categorias);
        }

        // GET api/categorias/1
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var categoria = await _categoriaService.ObtenerPorIdAsync(id);

            if (categoria == null)
                return NotFound(new { mensaje = "Categoría no encontrada" });

            return Ok(categoria);
        }

        //POST api/categorias > solo administradores pueden crear categorías
        [HttpPost]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Crear([FromBody] CrearCategoriaDto crearCategoriaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoria = await _categoriaService.CrearAsync(crearCategoriaDto);
            return CreatedAtAction(nameof(ObtenerPorId),
                new { id = categoria.IdCategoria }, categoria);
        }

        // PUT api/categorias/1
        [HttpPut("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Editar(int id, [FromBody] CrearCategoriaDto crearCategoriaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoria = await _categoriaService.EditarAsync(id, crearCategoriaDto);

            if (categoria == null)
                return NotFound(new { mensaje = "Categoría no encontrada" });

            return Ok(categoria);
        }

        // DELETE api/categorias/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _categoriaService.EliminarAsync(id);

            if (!resultado)
                return NotFound(new { mensaje = "Categoría no encontrada" });

            return Ok(new { mensaje = "Categoría eliminada correctamente" });
        }
    }
}