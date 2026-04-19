using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Evently.API.Data;
using Evently.API.Models;

namespace Evently.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePedidosController : ControllerBase
    {
        private readonly EventlyDbContext _context;

        public DetallePedidosController(EventlyDbContext context)
        {
            _context = context;
        }

        // GET: api/DetallePedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePedido>>> GetDetallesPedido()
        {
            return await _context.DetallesPedido
                .Include(d => d.Pedido)
                .Include(d => d.Actividad)
                .ToListAsync();
        }

        // GET: api/DetallePedidos/5
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

        // GET: api/DetallePedidos/pedido/5
        [HttpGet("pedido/{idPedido}")]
        public async Task<ActionResult<IEnumerable<DetallePedido>>> GetDetallesPorPedido(int idPedido)
        {
            return await _context.DetallesPedido
                .Include(d => d.Actividad)
                .Where(d => d.IdPedido == idPedido)
                .ToListAsync();
        }

        // POST: api/DetallePedidos
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

        // PUT: api/DetallePedidos/5
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

        // DELETE: api/DetallePedidos/5
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