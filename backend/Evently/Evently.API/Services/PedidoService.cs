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
        public async Task<PedidoDto?> ObtenerPorIdAsync(int id, int idUsuario, bool esAdmin)
        {
            var pedido = await _contexto.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Estado)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.Actividad)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null) return null;

            if (!esAdmin && pedido.Cliente.IdUsuario != idUsuario) return null;

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
        public async Task<List<PedidoDto>?> ObtenerPorClienteAsync(int idCliente, int idUsuario, bool esAdmin)
        {
            var cliente = await _contexto.Clientes
                .FirstOrDefaultAsync(c => c.IdCliente == idCliente);

            if (cliente == null) return null;

            if (!esAdmin && cliente.IdUsuario != idUsuario) return null;

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

            await _contexto.Entry(pedido).Reference(p => p.Estado).LoadAsync();
            await _contexto.Entry(pedido).Reference(p => p.Cliente).LoadAsync();

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

        // Cambia el estado de un pedido
        public async Task<PedidoDto?> CambiarEstadoAsync(int idPedido, int idEstado)
        {
            var pedido = await _contexto.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Estado)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.Actividad)
                .FirstOrDefaultAsync(p => p.IdPedido == idPedido);

            if (pedido == null)
            {
                return null;
            }

            var estadoNuevo = await _contexto.Estados
                .FirstOrDefaultAsync(e => e.IdEstado == idEstado);

            if (estadoNuevo == null)
            {
                return null;
            }

            var estadoAnteriorOcupaPlazas = EstadoOcupaPlazas(pedido.Estado.NombreEstado);
            var estadoNuevoOcupaPlazas = EstadoOcupaPlazas(estadoNuevo.NombreEstado);

            if (!estadoAnteriorOcupaPlazas && estadoNuevoOcupaPlazas)
            {
                if (!HayPlazasParaPedido(pedido))
                {
                    return null;
                }

                OcuparPlazasPedido(pedido);
            }

            if (estadoAnteriorOcupaPlazas && !estadoNuevoOcupaPlazas)
            {
                DevolverPlazasPedido(pedido);
            }

            pedido.IdEstado = estadoNuevo.IdEstado;
            pedido.Estado = estadoNuevo;

            await _contexto.SaveChangesAsync();

            return CrearPedidoDto(
                pedido,
                pedido.Cliente,
                estadoNuevo,
                pedido.DetallesPedido.ToList());
        }

        // Indica si un estado debe ocupar plazas
        private bool EstadoOcupaPlazas(string nombreEstado)
        {
            return nombreEstado == "Confirmado";
        }

        // Comprueba si hay plazas para volver a confirmar un pedido
        private bool HayPlazasParaPedido(Pedido pedido)
        {
            foreach (var detalle in pedido.DetallesPedido)
            {
                var actividad = detalle.Actividad;

                if (actividad.CupoMaximo.HasValue)
                {
                    var plazasDisponibles = actividad.CupoMaximo.Value - actividad.PlazasOcupadas;

                    if (detalle.Cantidad > plazasDisponibles)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // Suma las plazas ocupadas de las actividades del pedido
        private void OcuparPlazasPedido(Pedido pedido)
        {
            foreach (var detalle in pedido.DetallesPedido)
            {
                detalle.Actividad.PlazasOcupadas += detalle.Cantidad;
            }
        }

        // Devuelve las plazas ocupadas cuando el pedido deja de estar confirmado
        private void DevolverPlazasPedido(Pedido pedido)
        {
            foreach (var detalle in pedido.DetallesPedido)
            {
                var plazas = detalle.Actividad.PlazasOcupadas - detalle.Cantidad;

                if (plazas < 0)
                {
                    plazas = 0;
                }

                detalle.Actividad.PlazasOcupadas = plazas;
            }
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

        // Confirma el pedido con las actividades del carrito (localStorage)
        public async Task<PedidoDto?> ConfirmarAsync(ConfirmarPedidoDto confirmarDto, int idUsuario)
        {
            var cliente = await ObtenerClienteAsync(confirmarDto.IdCliente);

            if (cliente == null)
            {
                return null;
            }

            if (cliente.IdUsuario != idUsuario)
            {
                return null;
            }

            if (confirmarDto.Actividades == null || !confirmarDto.Actividades.Any())
            {
                return null;
            }

            var actividades = await ObtenerActividadesValidasAsync(confirmarDto.Actividades);

            if (actividades == null)
            {
                return null;
            }

            var estado = await ObtenerEstadoConfirmadoAsync();

            if (estado == null)
            {
                return null;
            }

            var pedido = CrearPedido(confirmarDto.IdCliente, estado.IdEstado);

            _contexto.Pedidos.Add(pedido);
            await _contexto.SaveChangesAsync();

            AñadirDetallesYActualizarPlazas(pedido, confirmarDto.Actividades, actividades);

            await _contexto.SaveChangesAsync();

            var detalles = await _contexto.DetallesPedido
                .Include(d => d.Actividad)
                .Where(d => d.IdPedido == pedido.IdPedido)
                .ToListAsync();

            return CrearPedidoDto(pedido, cliente, estado, detalles);
        }

        // Busca el cliente que va a realizar el pedido
        private async Task<Cliente?> ObtenerClienteAsync(int idCliente)
        {
            return await _contexto.Clientes
                .FirstOrDefaultAsync(c => c.IdCliente == idCliente);
        }

        // Busca el estado Confirmado
        private async Task<Estado?> ObtenerEstadoConfirmadoAsync()
        {
            return await _contexto.Estados
                .FirstOrDefaultAsync(e => e.NombreEstado == "Confirmado");
        }

        // Comprueba que las actividades existen, que las cantidades son correctas y que hay plazas
        private async Task<List<Actividad>?> ObtenerActividadesValidasAsync(List<ItemCarritoDto> items)
        {
            var actividades = new List<Actividad>();

            foreach (var item in items)
            {
                if (item.Cantidad <= 0)
                {
                    return null;
                }

                var actividad = await _contexto.Actividades
                    .FirstOrDefaultAsync(a => a.IdActividad == item.IdActividad);

                if (actividad == null)
                {
                    return null;
                }

                if (actividad.CupoMaximo.HasValue)
                {
                    var plazasDisponibles = actividad.CupoMaximo.Value - actividad.PlazasOcupadas;

                    if (item.Cantidad > plazasDisponibles)
                    {
                        return null;
                    }
                }

                actividades.Add(actividad);
            }

            return actividades;
        }

        // Crea el pedido principal
        private Pedido CrearPedido(int idCliente, int idEstado)
        {
            return new Pedido
            {
                IdCliente = idCliente,
                IdEstado = idEstado,
                FechaCreacion = DateTime.UtcNow,
                FechaConfirm = DateTime.UtcNow
            };
        }

        // Añade las líneas del pedido y actualiza las plazas ocupadas
        private void AñadirDetallesYActualizarPlazas(Pedido pedido, List<ItemCarritoDto> items, List<Actividad> actividades)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var actividad = actividades[i];

                var detalle = new DetallePedido
                {
                    IdPedido = pedido.IdPedido,
                    IdActividad = item.IdActividad,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = actividad.Precio
                };

                _contexto.DetallesPedido.Add(detalle);

                actividad.PlazasOcupadas += item.Cantidad;
            }
        }

        // Prepara el DTO que se devuelve al frontend
        private PedidoDto CrearPedidoDto(Pedido pedido, Cliente cliente, Estado estado, List<DetallePedido> detalles)
        {
            return new PedidoDto
            {
                IdPedido = pedido.IdPedido,
                IdCliente = pedido.IdCliente,
                NombreCliente = cliente.Nombre + " " + cliente.Apellidos,
                IdEstado = pedido.IdEstado,
                NombreEstado = estado.NombreEstado,
                FechaCreacion = pedido.FechaCreacion,
                FechaConfirm = pedido.FechaConfirm,
                Detalles = detalles.Select(d => new DetallePedidoResumenDto
                {
                    IdActividad = d.IdActividad,
                    TituloActividad = d.Actividad.Titulo,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    ImporteLinea = d.Cantidad * d.PrecioUnitario
                }).ToList()
            };
        }

    }
}