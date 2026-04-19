using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Evently.API.Data;
using Evently.API.Models;

namespace Evently.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly EventlyDbContext _context;

        public PedidosController(EventlyDbContext context)
        {
            _context = context;
        }

        // GET: api/Pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Estado)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.Actividad)
                .ToListAsync();
        }

        // GET: api/Pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Estado)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.Actividad)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return pedido;
        }

        // GET: api/Pedidos/cliente/5
        [HttpGet("cliente/{idCliente}")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidosPorCliente(int idCliente)
        {
            return await _context.Pedidos
                .Include(p => p.Estado)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.Actividad)
                .Where(p => p.IdCliente == idCliente)
                .ToListAsync();
        }

        // PUT: api/Pedidos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(int id, Pedido pedido)
        {
            if (id != pedido.IdPedido)
            {
                return BadRequest();
            }

            _context.Entry(pedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
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

        // POST: api/Pedidos
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
            pedido.FechaCreacion = DateTime.UtcNow;
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPedido", new { id = pedido.IdPedido }, pedido);
        }

        // DELETE: api/Pedidos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.IdPedido == id);
        }
    }
}