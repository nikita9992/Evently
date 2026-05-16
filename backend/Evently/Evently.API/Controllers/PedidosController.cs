using Evently.API.DTOs.Pedido;
using Evently.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Evently.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;
        private readonly IClienteService _clienteService;

        public PedidosController(IPedidoService pedidoService, IClienteService clienteService)
        {
            _pedidoService = pedidoService;
            _clienteService = clienteService;
        }

        // Devuelve todos los pedidos con sus datos relacionados
        [HttpGet]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> GetPedidos()
        {
            var pedidos = await _pedidoService.ObtenerTodosAsync();
            return Ok(pedidos);
        }

        // Devuelve los pedidos del cliente asociado al usuario que ha iniciado sesión
        [HttpGet("cliente-actual")]
        public async Task<IActionResult> GetPedidosClienteActual()
        {
            var idUsuarioTexto = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(idUsuarioTexto, out int idUsuario))
                return Unauthorized();

            var cliente = await _clienteService.ObtenerPorUsuarioAsync(idUsuario);

            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado para el usuario actual" });

            var pedidos = await _pedidoService.ObtenerPorClienteAsync(cliente.IdCliente);

            return Ok(pedidos);
        }

        // Devuelve un pedido concreto con todos sus datos
        [HttpGet("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> GetPedido(int id)
        {
            var pedido = await _pedidoService.ObtenerPorIdAsync(id);

            if (pedido == null)
                return NotFound(new { mensaje = "Pedido no encontrado" });

            return Ok(pedido);
        }

        // Devuelve todos los pedidos de un cliente concreto
        [HttpGet("cliente/{idCliente}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> GetPedidosPorCliente(int idCliente)
        {
            var pedidos = await _pedidoService.ObtenerPorClienteAsync(idCliente);
            return Ok(pedidos);
        }

        // Crea un nuevo pedido
        [HttpPost]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> PostPedido([FromBody] CrearPedidoDto crearPedidoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pedido = await _pedidoService.CrearAsync(crearPedidoDto);
            return CreatedAtAction(nameof(GetPedido),
                new { id = pedido.IdPedido }, pedido);
        }

        // Actualiza un pedido existente
        [HttpPut("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> PutPedido(int id, [FromBody] CrearPedidoDto crearPedidoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pedido = await _pedidoService.EditarAsync(id, crearPedidoDto);

            if (pedido == null)
                return NotFound(new { mensaje = "Pedido no encontrado" });

            return Ok(pedido);
        }

        // Cambia solo el estado de un pedido
        [HttpPut("{id}/estado")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> CambiarEstadoPedido(int id, [FromBody] CambiarEstadoPedidoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pedido = await _pedidoService.CambiarEstadoAsync(id, dto.IdEstado);

            if (pedido == null)
            {
                return BadRequest(new { mensaje = "No se ha podido cambiar el estado del pedido" });
            }

            return Ok(pedido);
        }

        // Elimina un pedido
        [HttpDelete("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var resultado = await _pedidoService.EliminarAsync(id);

            if (!resultado)
                return NotFound(new { mensaje = "Pedido no encontrado" });

            return Ok(new { mensaje = "Pedido eliminado correctamente" });
        }

        // Confirma el pedido del cliente asociado al usuario que ha iniciado sesión
        [HttpPost("confirmar")]
        public async Task<IActionResult> ConfirmarPedido([FromBody] ConfirmarPedidoDto confirmarDto)
        {
            var idUsuarioTexto = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(idUsuarioTexto, out int idUsuario))
                return Unauthorized();

            var cliente = await _clienteService.ObtenerPorUsuarioAsync(idUsuario);

            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado para el usuario actual" });

            confirmarDto.IdCliente = cliente.IdCliente;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pedido = await _pedidoService.ConfirmarAsync(confirmarDto);

            if (pedido == null)
                return BadRequest(new { mensaje = "El cliente no existe o el carrito está vacío" });

            return CreatedAtAction(nameof(GetPedido),
                new { id = pedido.IdPedido }, pedido);
        }
    }
}