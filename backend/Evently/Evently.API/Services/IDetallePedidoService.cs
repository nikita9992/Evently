using Evently.API.DTOs.DetallePedido;

namespace Evently.API.Services
{
    // Interfaz que define los métodos para gestionar las líneas de un pedido
    public interface IDetallePedidoService
    {
        Task<List<DetallePedidoDto>> ObtenerTodosAsync();
        Task<DetallePedidoDto?> ObtenerPorIdAsync(int idPedido, int idActividad);
        Task<List<DetallePedidoDto>> ObtenerPorPedidoAsync(int idPedido);
        Task<DetallePedidoDto> CrearAsync(CrearDetallePedidoDto crearDetallePedidoDto);
        Task<DetallePedidoDto?> EditarAsync(int idPedido, int idActividad, CrearDetallePedidoDto crearDetallePedidoDto);
        Task<bool> EliminarAsync(int idPedido, int idActividad);
    }
}