using Evently.API.DTOs.Estado;
using Evently.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evently.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadosController : ControllerBase
    {
        private readonly IEstadoService _estadoService;

        public EstadosController(IEstadoService estadoService)
        {
            _estadoService = estadoService;
        }

        // GET api/estados
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var estados = await _estadoService.ObtenerTodosAsync();
            return Ok(estados);
        }

        // GET api/estados/1
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var estado = await _estadoService.ObtenerPorIdAsync(id);

            if (estado == null)
                return NotFound(new { mensaje = "Estado no encontrado" });

            return Ok(estado);
        }

        // POST api/estados
        [HttpPost]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Crear([FromBody] CrearEstadoDto crearEstadoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var estado = await _estadoService.CrearAsync(crearEstadoDto);
            return CreatedAtAction(nameof(ObtenerPorId),
                new { id = estado.IdEstado }, estado);
        }

        // PUT api/estados/1
        [HttpPut("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Editar(int id, [FromBody] CrearEstadoDto crearEstadoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var estado = await _estadoService.EditarAsync(id, crearEstadoDto);

            if (estado == null)
                return NotFound(new { mensaje = "Estado no encontrado" });

            return Ok(estado);
        }

        // DELETE api/estados/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _estadoService.EliminarAsync(id);

            if (!resultado)
                return NotFound(new { mensaje = "Estado no encontrado" });

            return Ok(new { mensaje = "Estado eliminado correctamente" });
        }
    }
}