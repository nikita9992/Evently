using Evently.API.Data;
using Evently.API.DTOs.DetallePedido;
using Evently.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Evently.API.Services
{
    // Servicio que gestiona las líneas de actividad dentro de un pedido
    public class DetallePedidoService : IDetallePedidoService
    {
        private readonly EventlyDbContext _contexto;

        public DetallePedidoService(EventlyDbContext contexto)
        {
            _contexto = contexto;
        }

        // Obtener todas las líneas de todos los pedidos
        public async Task<List<DetallePedidoDto>> ObtenerTodosAsync()
        {
            return await _contexto.DetallesPedido
                .Include(d => d.Actividad)
                .Select(d => new DetallePedidoDto
                {
                    IdPedido = d.IdPedido,
                    IdActividad = d.IdActividad,
                    TituloActividad = d.Actividad.Titulo,
                    PrecioActividad = d.Actividad.Precio,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    ImporteLinea = d.Cantidad * d.PrecioUnitario
                })
                .ToListAsync();
        }

        // Obtener una línea concreta por IdPedido e IdActividad
        public async Task<DetallePedidoDto?> ObtenerPorIdAsync(int idPedido, int idActividad)
        {
            var detalle = await _contexto.DetallesPedido
                .Include(d => d.Actividad)
                .FirstOrDefaultAsync(d => d.IdPedido == idPedido && d.IdActividad == idActividad);

            if (detalle == null) return null;

            return new DetallePedidoDto
            {
                IdPedido = detalle.IdPedido,
                IdActividad = detalle.IdActividad,
                TituloActividad = detalle.Actividad.Titulo,
                PrecioActividad = detalle.Actividad.Precio,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = detalle.PrecioUnitario,
                ImporteLinea = detalle.Cantidad * detalle.PrecioUnitario
            };
        }

        // Obtener todas las líneas de un pedido concreto
        public async Task<List<DetallePedidoDto>> ObtenerPorPedidoAsync(int idPedido)
        {
            return await _contexto.DetallesPedido
                .Include(d => d.Actividad)
                .Where(d => d.IdPedido == idPedido)
                .Select(d => new DetallePedidoDto
                {
                    IdPedido = d.IdPedido,
                    IdActividad = d.IdActividad,
                    TituloActividad = d.Actividad.Titulo,
                    PrecioActividad = d.Actividad.Precio,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    ImporteLinea = d.Cantidad * d.PrecioUnitario
                })
                .ToListAsync();
        }

        // Añadir una línea de actividad a un pedido
        public async Task<DetallePedidoDto> CrearAsync(CrearDetallePedidoDto crearDetallePedidoDto)
        {
            var nuevoDetalle = new DetallePedido
            {
                IdPedido = crearDetallePedidoDto.IdPedido,
                IdActividad = crearDetallePedidoDto.IdActividad,
                Cantidad = crearDetallePedidoDto.Cantidad,
                PrecioUnitario = crearDetallePedidoDto.PrecioUnitario
            };

            _contexto.DetallesPedido.Add(nuevoDetalle);
            await _contexto.SaveChangesAsync();

            await _contexto.Entry(nuevoDetalle).Reference(d => d.Actividad).LoadAsync();

            return new DetallePedidoDto
            {
                IdPedido = nuevoDetalle.IdPedido,
                IdActividad = nuevoDetalle.IdActividad,
                TituloActividad = nuevoDetalle.Actividad.Titulo,
                PrecioActividad = nuevoDetalle.Actividad.Precio,
                Cantidad = nuevoDetalle.Cantidad,
                PrecioUnitario = nuevoDetalle.PrecioUnitario,
                ImporteLinea = nuevoDetalle.Cantidad * nuevoDetalle.PrecioUnitario
            };
        }

        // Editar una línea de pedido existente
        public async Task<DetallePedidoDto?> EditarAsync(int idPedido, int idActividad, CrearDetallePedidoDto crearDetallePedidoDto)
        {
            var detalle = await _contexto.DetallesPedido
                .Include(d => d.Actividad)
                .FirstOrDefaultAsync(d => d.IdPedido == idPedido && d.IdActividad == idActividad);

            if (detalle == null) return null;

            detalle.Cantidad = crearDetallePedidoDto.Cantidad;
            detalle.PrecioUnitario = crearDetallePedidoDto.PrecioUnitario;

            await _contexto.SaveChangesAsync();

            return new DetallePedidoDto
            {
                IdPedido = detalle.IdPedido,
                IdActividad = detalle.IdActividad,
                TituloActividad = detalle.Actividad.Titulo,
                PrecioActividad = detalle.Actividad.Precio,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = detalle.PrecioUnitario,
                ImporteLinea = detalle.Cantidad * detalle.PrecioUnitario
            };
        }

        // Eliminar una línea de pedido
        public async Task<bool> EliminarAsync(int idPedido, int idActividad)
        {
            var detalle = await _contexto.DetallesPedido
                .FirstOrDefaultAsync(d => d.IdPedido == idPedido && d.IdActividad == idActividad);

            if (detalle == null) return false;

            _contexto.DetallesPedido.Remove(detalle);
            await _contexto.SaveChangesAsync();

            return true;
        }
    }
}