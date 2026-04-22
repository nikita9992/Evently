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
    public class PedidosController : ControllerBase
    {
        private readonly EventlyDbContext _context;

        public PedidosController(EventlyDbContext context)
        {
            _context = context;
        }

        // Devuelve todos los pedidos con sus datos relacionados
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

        // Devuelve un pedido concreto con todos sus datos
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

        // Devuelve todos los pedidos de un cliente concreto
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

        // Actualiza un pedido existente
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

        // Crea un nuevo pedido
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
            pedido.FechaCreacion = DateTime.UtcNow;
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPedido", new { id = pedido.IdPedido }, pedido);
        }

        // Elimina un pedido
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

        // Confirma el pedido: comprueba que el cliente tiene datos y crea el pedido
        [HttpPost("confirmar")]
        public async Task<ActionResult<Pedido>> ConfirmarPedido(int idCliente, int idEstado)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.IdCliente == idCliente);

            if (cliente == null)
            {
                return BadRequest("El cliente no existe o no tiene datos personales.");
            }

            var estado = await _context.Estados
                .FirstOrDefaultAsync(e => e.IdEstado == idEstado);

            if (estado == null)
            {
                return BadRequest("El estado indicado no existe.");
            }

            var pedido = new Pedido
            {
                IdCliente = idCliente,
                IdEstado = idEstado,
                FechaCreacion = DateTime.UtcNow,
                FechaConfirm = DateTime.UtcNow
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPedido", new { id = pedido.IdPedido }, pedido);
        }
    }
}