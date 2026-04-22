using Evently.API.DTOs.DetallePedido;
using Evently.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evently.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DetallePedidosController : ControllerBase
    {
        private readonly IDetallePedidoService _detallePedidoService;

        public DetallePedidosController(IDetallePedidoService detallePedidoService)
        {
            _detallePedidoService = detallePedidoService;
        }

        // Devuelve todos los detalles de pedidos
        [HttpGet]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> GetDetallesPedido()
        {
            var detalles = await _detallePedidoService.ObtenerTodosAsync();
            return Ok(detalles);
        }

        // Devuelve un detalle de pedido concreto
        [HttpGet("{idPedido}/{idActividad}")]
        public async Task<IActionResult> GetDetallePedido(int idPedido, int idActividad)
        {
            var detalle = await _detallePedidoService.ObtenerPorIdAsync(idPedido, idActividad);

            if (detalle == null)
                return NotFound(new { mensaje = "Detalle de pedido no encontrado" });

            return Ok(detalle);
        }

        // Devuelve todas las líneas de un pedido concreto
        [HttpGet("pedido/{idPedido}")]
        public async Task<IActionResult> GetDetallesPorPedido(int idPedido)
        {
            var detalles = await _detallePedidoService.ObtenerPorPedidoAsync(idPedido);
            return Ok(detalles);
        }

        // Añade una línea a un pedido
        [HttpPost]
        public async Task<IActionResult> PostDetallePedido([FromBody] CrearDetallePedidoDto crearDetallePedidoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var detalle = await _detallePedidoService.CrearAsync(crearDetallePedidoDto);
            return CreatedAtAction(nameof(GetDetallePedido),
                new { idPedido = detalle.IdPedido, idActividad = detalle.IdActividad }, detalle);
        }

        // Actualiza una línea de pedido
        [HttpPut("{idPedido}/{idActividad}")]
        public async Task<IActionResult> PutDetallePedido(int idPedido, int idActividad, [FromBody] CrearDetallePedidoDto crearDetallePedidoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var detalle = await _detallePedidoService.EditarAsync(idPedido, idActividad, crearDetallePedidoDto);

            if (detalle == null)
                return NotFound(new { mensaje = "Detalle de pedido no encontrado" });

            return Ok(detalle);
        }

        // Elimina una línea de pedido
        [HttpDelete("{idPedido}/{idActividad}")]
        public async Task<IActionResult> DeleteDetallePedido(int idPedido, int idActividad)
        {
            var resultado = await _detallePedidoService.EliminarAsync(idPedido, idActividad);

            if (!resultado)
                return NotFound(new { mensaje = "Detalle de pedido no encontrado" });

            return Ok(new { mensaje = "Línea de pedido eliminada correctamente" });
        }
    }
}