using Evently.API.Data;
using Evently.API.DTOs.Pedido;
using Evently.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Evently.API.Services
{
    // Servicio que gestiona los pedidos del sistema
    public class PedidoService : IPedidoService
    {
        private readonly EventlyDbContext _contexto;

        public PedidoService(EventlyDbContext contexto)
        {
            _contexto = contexto;
        }

        // Obtener todos los pedidos con sus datos relacionados
        public async Task<List<PedidoDto>> ObtenerTodosAsync()
        {
            return await _contexto.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Estado)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.Actividad)
                .Select(p => new PedidoDto
                {
                    IdPedido = p.IdPedido,
                    IdCliente = p.IdCliente,
                    NombreCliente = p.Cliente.Nombre + " " + p.Cliente.Apellidos,
                    IdEstado = p.IdEstado,
                    NombreEstado = p.Estado.NombreEstado,
                    FechaCreacion = p.FechaCreacion,
                    FechaConfirm = p.FechaConfirm,
                    Detalles = p.DetallesPedido.Select(d => new DetallePedidoResumenDto
                    {
                        IdActividad = d.IdActividad,
                        TituloActividad = d.Actividad.Titulo,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        ImporteLinea = d.Cantidad * d.PrecioUnitario
                    }).ToList()
                })
                .ToListAsync();
        }

        // Obtener un pedido concreto con todos sus datos
        public async Task<PedidoDto?> ObtenerPorIdAsync(int id)
        {
            var pedido = await _contexto.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Estado)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.Actividad)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null) return null;

            return new PedidoDto
            {
                IdPedido = pedido.IdPedido,
                IdCliente = pedido.IdCliente,
                NombreCliente = pedido.Cliente.Nombre + " " + pedido.Cliente.Apellidos,
                IdEstado = pedido.IdEstado,
                NombreEstado = pedido.Estado.NombreEstado,
                FechaCreacion = pedido.FechaCreacion,
                FechaConfirm = pedido.FechaConfirm,
                Detalles = pedido.DetallesPedido.Select(d => new DetallePedidoResumenDto
                {
                    IdActividad = d.IdActividad,
                    TituloActividad = d.Actividad.Titulo,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    ImporteLinea = d.Cantidad * d.PrecioUnitario
                }).ToList()
            };
        }

        // Obtener todos los pedidos de un cliente concreto
        public async Task<List<PedidoDto>> ObtenerPorClienteAsync(int idCliente)
        {
            return await _contexto.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Estado)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.Actividad)
                .Where(p => p.IdCliente == idCliente)
                .Select(p => new PedidoDto
                {
                    IdPedido = p.IdPedido,
                    IdCliente = p.IdCliente,
                    NombreCliente = p.Cliente.Nombre + " " + p.Cliente.Apellidos,
                    IdEstado = p.IdEstado,
                    NombreEstado = p.Estado.NombreEstado,
                    FechaCreacion = p.FechaCreacion,
                    FechaConfirm = p.FechaConfirm,
                    Detalles = p.DetallesPedido.Select(d => new DetallePedidoResumenDto
                    {
                        IdActividad = d.IdActividad,
                        TituloActividad = d.Actividad.Titulo,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        ImporteLinea = d.Cantidad * d.PrecioUnitario
                    }).ToList()
                })
                .ToListAsync();
        }

        // Crear un nuevo pedido
        public async Task<PedidoDto> CrearAsync(CrearPedidoDto crearPedidoDto)
        {
            var nuevoPedido = new Pedido
            {
                IdCliente = crearPedidoDto.IdCliente,
                IdEstado = crearPedidoDto.IdEstado,
                FechaCreacion = DateTime.UtcNow,
                FechaConfirm = DateTime.UtcNow
            };

            _contexto.Pedidos.Add(nuevoPedido);
            await _contexto.SaveChangesAsync();

            await _contexto.Entry(nuevoPedido).Reference(p => p.Cliente).LoadAsync();
            await _contexto.Entry(nuevoPedido).Reference(p => p.Estado).LoadAsync();

            return new PedidoDto
            {
                IdPedido = nuevoPedido.IdPedido,
                IdCliente = nuevoPedido.IdCliente,
                NombreCliente = nuevoPedido.Cliente.Nombre + " " + nuevoPedido.Cliente.Apellidos,
                IdEstado = nuevoPedido.IdEstado,
                NombreEstado = nuevoPedido.Estado.NombreEstado,
                FechaCreacion = nuevoPedido.FechaCreacion,
                FechaConfirm = nuevoPedido.FechaConfirm,
                Detalles = new List<DetallePedidoResumenDto>()
            };
        }

        // Editar un pedido existente
        public async Task<PedidoDto?> EditarAsync(int id, CrearPedidoDto crearPedidoDto)
        {
            var pedido = await _contexto.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Estado)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.Actividad)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null) return null;

            pedido.IdCliente = crearPedidoDto.IdCliente;
            pedido.IdEstado = crearPedidoDto.IdEstado;

            await _contexto.SaveChangesAsync();

            return new PedidoDto
            {
                IdPedido = pedido.IdPedido,
                IdCliente = pedido.IdCliente,
                NombreCliente = pedido.Cliente.Nombre + " " + pedido.Cliente.Apellidos,
                IdEstado = pedido.IdEstado,
                NombreEstado = pedido.Estado.NombreEstado,
                FechaCreacion = pedido.FechaCreacion,
                FechaConfirm = pedido.FechaConfirm,
                Detalles = pedido.DetallesPedido.Select(d => new DetallePedidoResumenDto
                {
                    IdActividad = d.IdActividad,
                    TituloActividad = d.Actividad.Titulo,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    ImporteLinea = d.Cantidad * d.PrecioUnitario
                }).ToList()
            };
        }

        // Eliminar un pedido
        public async Task<bool> EliminarAsync(int id)
        {
            var pedido = await _contexto.Pedidos
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null) return false;

            _contexto.Pedidos.Remove(pedido);
            await _contexto.SaveChangesAsync();

            return true;
        }

        // Confirma el pedido: comprueba que el cliente existe y crea el pedido confirmado
        public async Task<PedidoDto?> ConfirmarAsync(int idCliente, int idEstado)
        {
            var cliente = await _contexto.Clientes
                .FirstOrDefaultAsync(c => c.IdCliente == idCliente);

            if (cliente == null) return null;

            var estado = await _contexto.Estados
                .FirstOrDefaultAsync(e => e.IdEstado == idEstado);

            if (estado == null) return null;

            var pedido = new Pedido
            {
                IdCliente = idCliente,
                IdEstado = idEstado,
                FechaCreacion = DateTime.UtcNow,
                FechaConfirm = DateTime.UtcNow
            };

            _contexto.Pedidos.Add(pedido);
            await _contexto.SaveChangesAsync();

            return new PedidoDto
            {
                IdPedido = pedido.IdPedido,
                IdCliente = pedido.IdCliente,
                NombreCliente = cliente.Nombre + " " + cliente.Apellidos,
                IdEstado = pedido.IdEstado,
                NombreEstado = estado.NombreEstado,
                FechaCreacion = pedido.FechaCreacion,
                FechaConfirm = pedido.FechaConfirm,
                Detalles = new List<DetallePedidoResumenDto>()
            };
        }
    }
}