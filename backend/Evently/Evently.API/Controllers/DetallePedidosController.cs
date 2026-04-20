using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Evently.API.Data;
using Evently.API.Models;

namespace Evently.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePedidosController : ControllerBase
    {
        private readonly EventlyDbContext _context;

        public DetallePedidosController(EventlyDbContext context)
        {
            _context = context;
        }

        // Devuelve todos los detalles de pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePedido>>> GetDetallesPedido()
        {
            return await _context.DetallesPedido
                .Include(d => d.Pedido)
                .Include(d => d.Actividad)
                .ToListAsync();
        }

        // Devuelve un detalle de pedido concreto
        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePedido>> GetDetallePedido(int id)
        {
            var detallePedido = await _context.DetallesPedido
                .Include(d => d.Pedido)
                .Include(d => d.Actividad)
                .FirstOrDefaultAsync(d => d.IdPedido == id);

            if (detallePedido == null)
            {
                return NotFound();
            }

            return detallePedido;
        }

        // Devuelve todas las líneas de un pedido concreto
        [HttpGet("pedido/{idPedido}")]
        public async Task<ActionResult<IEnumerable<DetallePedido>>> GetDetallesPorPedido(int idPedido)
        {
            return await _context.DetallesPedido
                .Include(d => d.Actividad)
                .Where(d => d.IdPedido == idPedido)
                .ToListAsync();
        }

        // Añade una línea a un pedido
        [HttpPost]
        public async Task<ActionResult<DetallePedido>> PostDetallePedido(DetallePedido detallePedido)
        {
            _context.DetallesPedido.Add(detallePedido);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DetallePedidoExists(detallePedido.IdPedido))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDetallePedido", new { id = detallePedido.IdPedido }, detallePedido);
        }

        // Actualiza una línea de pedido
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetallePedido(int id, DetallePedido detallePedido)
        {
            if (id != detallePedido.IdPedido)
            {
                return BadRequest();
            }

            _context.Entry(detallePedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetallePedidoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Elimina una línea de pedido
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetallePedido(int id)
        {
            var detallePedido = await _context.DetallesPedido.FindAsync(id);

            if (detallePedido == null)
            {
                return NotFound();
            }

            _context.DetallesPedido.Remove(detallePedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetallePedidoExists(int id)
        {
            return _context.DetallesPedido.Any(e => e.IdPedido == id);
        }
    }
}