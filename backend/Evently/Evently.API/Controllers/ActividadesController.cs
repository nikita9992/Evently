using Evently.API.DTOs.Actividad;
using Evently.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evently.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActividadesController : ControllerBase
    {
        private readonly IActividadService _actividadService;

        public ActividadesController(IActividadService actividadService)
        {
            _actividadService = actividadService;
        }

        //GET api/actividades?idCategoria=1&titulo=surf
        [HttpGet]
        public async Task<IActionResult> ObtenerTodas(
            [FromQuery] int? idCategoria,
            [FromQuery] string? titulo)
        {
            var filtro = new FiltroActividadDto
            {
                IdCategoria = idCategoria,
                Titulo = titulo
            };

            var actividades = await _actividadService.ObtenerTodasAsync(filtro);
            return Ok(actividades);
        }

        // GET api/actividades/1
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var actividad = await _actividadService.ObtenerPorIdAsync(id);

            if (actividad == null)
                return NotFound(new { mensaje = "Actividad no encontrada" });

            return Ok(actividad);
        }

        // POST api/actividades > Solo administradores pueden crear actividades
        [HttpPost]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Crear([FromBody] CrearActividadDto crearActividadDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var actividad = await _actividadService.CrearAsync(crearActividadDto);
            return CreatedAtAction(nameof(ObtenerPorId),
                new { id = actividad.IdActividad }, actividad);
        }

        // PUT api/actividades/1 > solo administradores pueden editar
        [HttpPut("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Editar(int id, [FromBody] CrearActividadDto crearActividadDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var actividad = await _actividadService.EditarAsync(id, crearActividadDto);

            if (actividad == null)
                return NotFound(new { mensaje = "Actividad no encontrada" });

            return Ok(actividad);
        }

        // DELETE api/actividades/1 > solo administradores pueden eliminar
        [HttpDelete("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _actividadService.EliminarAsync(id);

            if (!resultado)
                return NotFound(new { mensaje = "Actividad no encontrada" });

            return Ok(new { mensaje = "Actividad eliminada correctamente" });
        }
    }
}