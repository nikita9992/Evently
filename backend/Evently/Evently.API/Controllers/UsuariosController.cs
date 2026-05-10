using Evently.API.DTOs.Usuario;
using Evently.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Evently.API.Controllers
{
    // Controlador para gestionar usuarios. Solo accesible por administradores.
    [Authorize(Roles = "administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/Usuarios
        // Obtenemos los usuarios para mostrarlos en el panel de administración.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioAdminDto>>> GetUsuarios()
        {
            List<UsuarioAdminDto> usuarios = await _usuarioService.ObtenerTodosAsync();

            return Ok(usuarios);
        }

        // GET: api/Usuarios/5
        // Buscamos un usuario por su id.
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioAdminDto>> GetUsuario(int id)
        {
            UsuarioAdminDto? usuario = await _usuarioService.ObtenerPorIdAsync(id);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            return Ok(usuario);
        }

        // POST: api/Usuarios
        // Creamos un nuevo usuario desde el panel de administración.
        [HttpPost]
        public async Task<ActionResult<UsuarioAdminDto>> PostUsuario(CrearUsuarioDto crearUsuarioDto)
        {
            if (string.IsNullOrWhiteSpace(crearUsuarioDto.Email) ||
                string.IsNullOrWhiteSpace(crearUsuarioDto.Password))
            {
                return BadRequest(new { mensaje = "El email y la contraseña son obligatorios" });
            }

            UsuarioAdminDto? usuarioCreado = await _usuarioService.CrearAsync(crearUsuarioDto);

            if (usuarioCreado == null)
            {
                return BadRequest(new { mensaje = "Ya existe un usuario con ese email" });
            }

            return CreatedAtAction(
                nameof(GetUsuario),
                new { id = usuarioCreado.IdUsuario },
                usuarioCreado
            );
        }

        // PUT: api/Usuarios/5/rol
        // Cambiamos únicamente el rol de un usuario.
        // No modificamos la contraseña ni el resto de datos.
        // No permitimos que un administrador cambie su propio rol.
        [HttpPut("{id}/rol")]
        public async Task<IActionResult> CambiarRolUsuario(int id, CambiarRolUsuarioDto cambiarRolUsuarioDto)
        {
            string? idUsuarioActual = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (idUsuarioActual == id.ToString())
            {
                return BadRequest(new { mensaje = "No puedes cambiar tu propio rol mientras tienes la sesión iniciada" });
            }

            if (string.IsNullOrWhiteSpace(cambiarRolUsuarioDto.Rol))
            {
                return BadRequest(new { mensaje = "El rol es obligatorio" });
            }

            if (cambiarRolUsuarioDto.Rol != "usuario" &&
                cambiarRolUsuarioDto.Rol != "administrador")
            {
                return BadRequest(new { mensaje = "El rol no es válido" });
            }

            UsuarioAdminDto? usuarioActualizado = await _usuarioService.CambiarRolAsync(id, cambiarRolUsuarioDto);

            if (usuarioActualizado == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            return Ok(usuarioActualizado);
        }

        // DELETE: api/Usuarios/5
        // Eliminamos un usuario por su id.
        // No permitimos que un administrador elimine su propia cuenta.
        // Tampoco eliminamos usuarios con cliente, perfil o pedidos asociados.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            string? idUsuarioActual = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (idUsuarioActual == id.ToString())
            {
                return BadRequest(new { mensaje = "No puedes eliminar tu propio usuario mientras tienes la sesión iniciada" });
            }

            bool tieneDatosAsociados = await _usuarioService.TieneDatosAsociadosAsync(id);

            if (tieneDatosAsociados)
            {
                return BadRequest(new { mensaje = "No se puede eliminar el usuario porque tiene datos asociados" });
            }

            bool eliminado = await _usuarioService.EliminarAsync(id);

            if (!eliminado)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            return Ok(new { mensaje = "Usuario eliminado correctamente" });
        }
    }
}