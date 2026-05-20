using Evently.API.DTOs.Cliente;
using Evently.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Evently.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // Devuelve todos los clientes con sus datos relacionados
        [HttpGet]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();
            return Ok(clientes);
        }

        // Devuelve un cliente concreto con sus pedidos
        [HttpGet("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> GetCliente(int id)
        {
            var cliente = await _clienteService.ObtenerPorIdAsync(id);

            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado" });

            return Ok(cliente);
        }

        // Devuelve el cliente asociado a un usuario concreto
        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> GetClientePorUsuario(int idUsuario)
        {
            var idUsuarioStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(idUsuarioStr, out int idUsuarioToken))
                return Unauthorized();

            var esAdmin = User.IsInRole("administrador");

            if (idUsuarioToken != idUsuario && !esAdmin)
                return Forbid();

            var cliente = await _clienteService.ObtenerPorUsuarioAsync(idUsuario);

            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado para ese usuario" });

            return Ok(cliente);
        }

        // Crea el cliente asociado al usuario que ha iniciado sesión
        [HttpPost]
        public async Task<IActionResult> PostCliente([FromBody] CrearClienteDto crearClienteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var idUsuarioStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(idUsuarioStr, out int idUsuario))
                return Unauthorized();

            crearClienteDto.IdUsuario = idUsuario;

            var cliente = await _clienteService.CrearAsync(crearClienteDto);

            return CreatedAtAction(nameof(GetCliente),
                new { id = cliente.IdCliente }, cliente);
        }

        // Actualiza los datos de un cliente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, [FromBody] CrearClienteDto crearClienteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clienteActual = await _clienteService.ObtenerPorIdAsync(id);

            if (clienteActual == null)
                return NotFound(new { mensaje = "Cliente no encontrado" });

            var idUsuarioStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(idUsuarioStr, out int idUsuario))
                return Unauthorized();

            var esAdmin = User.IsInRole("administrador");

            if (clienteActual.IdUsuario != idUsuario && !esAdmin)
                return Forbid();

            crearClienteDto.IdUsuario = clienteActual.IdUsuario;

            var cliente = await _clienteService.EditarAsync(id, crearClienteDto);

            return Ok(cliente);
        }

        // Elimina un cliente
        [HttpDelete("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var resultado = await _clienteService.EliminarAsync(id);

            if (!resultado)
                return NotFound(new { mensaje = "Cliente no encontrado" });

            return Ok(new { mensaje = "Cliente eliminado correctamente" });
        }
    }
}