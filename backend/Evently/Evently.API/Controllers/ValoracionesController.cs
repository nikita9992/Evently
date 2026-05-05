using Evently.API.DTOs.Valoracion;
using Evently.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Evently.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValoracionesController : ControllerBase
    {
        private readonly IValoracionService _valoracionService;

        public ValoracionesController(IValoracionService valoracionService)
        {
            _valoracionService = valoracionService;
        }

        // GET api/valoraciones/actividad/1
        [HttpGet("actividad/{idActividad}")]
        public async Task<IActionResult> ObtenerResumen(int idActividad)
        {
            int? idUsuario = null;
            var idUsuarioStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(idUsuarioStr, out int id))
                idUsuario = id;

            var resumen = await _valoracionService
                .ObtenerResumenAsync(idActividad, idUsuario);

            return Ok(resumen);
        }

        // GET api/valoraciones/actividad/1/puedovalorar
        // Verificar si el usuario puede valorar
        [HttpGet("actividad/{idActividad}/puedovalorar")]
        [Authorize]
        public async Task<IActionResult> PuedeValorar(int idActividad)
        {
            var idUsuarioStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(idUsuarioStr, out int idUsuario))
                return Unauthorized();

            var puedeValorar = await _valoracionService
                .UsuarioComproActividadAsync(idActividad, idUsuario);

            return Ok(new { puedeValorar });
        }

        // POST api/valoraciones/actividad/1
        // Solo usuarios que compraron la actividad
        [HttpPost("actividad/{idActividad}")]
        [Authorize]
        public async Task<IActionResult> CrearOActualizar(
            int idActividad,
            [FromBody] CrearValoracionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var idUsuarioStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(idUsuarioStr, out int idUsuario))
                return Unauthorized();

            var valoracion = await _valoracionService
                .CrearOActualizarAsync(idActividad, idUsuario, dto);

            if (valoracion == null)
                return BadRequest(new
                {
                    mensaje = "Solo puedes valorar actividades que hayas comprado"
                });

            return Ok(valoracion);
        }
    }
}