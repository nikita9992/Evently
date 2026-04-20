using Evently.API.DTOs.Auth;
using Evently.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Evently.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacionController : ControllerBase
    {
        private readonly IAutenticacionService _autenticacionService;

        public AutenticacionController(IAutenticacionService autenticacionService)
        {
            _autenticacionService = autenticacionService;
        }

        // POST api/autenticacion/registro
        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroDto registroDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var respuesta = await _autenticacionService.RegistrarAsync(registroDto);

            if (respuesta == null)
                return BadRequest(new { mensaje = "El email ya está registrado" });

            return Ok(respuesta);
        }

        // POST api/autenticacion/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var respuesta = await _autenticacionService.LoginAsync(loginDto);

            if (respuesta == null)
                return Unauthorized(new { mensaje = "Email o contraseña incorrectos" });

            return Ok(respuesta);
        }
    }
}