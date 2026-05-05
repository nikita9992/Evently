using Evently.API.DTOs.Comentario;
using Evently.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Evently.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComentariosController : ControllerBase
    {
        private readonly IComentarioService _comentarioService;

        public ComentariosController(IComentarioService comentarioService)
        {
            _comentarioService = comentarioService;
        }

        // GET api/comentarios/actividad/1
        [HttpGet("actividad/{idActividad}")]
        public async Task<IActionResult> ObtenerPorActividad(int idActividad)
        {
            var comentarios = await _comentarioService
                .ObtenerPorActividadAsync(idActividad);
            return Ok(comentarios);
        }

        // POST api/comentarios/actividad/1
        // Solo usuarios autenticados con perfil completo
        [HttpPost("actividad/{idActividad}")]
        [Authorize]
        public async Task<IActionResult> Crear(
            int idActividad,
            [FromBody] CrearComentarioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Obtener IdUsuario del token JWT
            var idUsuarioStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(idUsuarioStr, out int idUsuario))
                return Unauthorized();

            var comentario = await _comentarioService
                .CrearAsync(idActividad, idUsuario, dto);

            if (comentario == null)
                return BadRequest(new
                {
                    mensaje = "Debes completar tu perfil antes de comentar"
                });

            return Ok(comentario);
        }

        // DELETE api/comentarios/1
        // Solo el autor o un admin puede eliminar
        [HttpDelete("{idComentario}")]
        [Authorize]
        public async Task<IActionResult> Eliminar(int idComentario)
        {
            var idUsuarioStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(idUsuarioStr, out int idUsuario))
                return Unauthorized();

            var esAdmin = User.IsInRole("administrador");

            var resultado = await _comentarioService
                .EliminarAsync(idComentario, idUsuario, esAdmin);

            if (!resultado)
                return NotFound(new { mensaje = "Comentario no encontrado" });

            return Ok(new { mensaje = "Comentario eliminado correctamente" });
        }
    }
}