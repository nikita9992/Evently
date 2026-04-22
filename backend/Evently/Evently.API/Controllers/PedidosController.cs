using Evently.API.DTOs.Pedido;
using Evently.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evently.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        // Devuelve todos los pedidos con sus datos relacionados
        [HttpGet]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> GetPedidos()
        {
            var pedidos = await _pedidoService.ObtenerTodosAsync();
            return Ok(pedidos);
        }

        // Devuelve un pedido concreto con todos sus datos
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPedido(int id)
        {
            var pedido = await _pedidoService.ObtenerPorIdAsync(id);

            if (pedido == null)
                return NotFound(new { mensaje = "Pedido no encontrado" });

            return Ok(pedido);
        }

        // Devuelve todos los pedidos de un cliente concreto
        [HttpGet("cliente/{idCliente}")]
        public async Task<IActionResult> GetPedidosPorCliente(int idCliente)
        {
            var pedidos = await _pedidoService.ObtenerPorClienteAsync(idCliente);
            return Ok(pedidos);
        }

        // Crea un nuevo pedido
        [HttpPost]
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
        public async Task<IActionResult> PutPedido(int id, [FromBody] CrearPedidoDto crearPedidoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pedido = await _pedidoService.EditarAsync(id, crearPedidoDto);

            if (pedido == null)
                return NotFound(new { mensaje = "Pedido no encontrado" });

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

        // Confirma el pedido con las actividades del carrito
        [HttpPost("confirmar")]
        public async Task<IActionResult> ConfirmarPedido([FromBody] ConfirmarPedidoDto confirmarDto)
        {
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