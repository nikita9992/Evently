using Evently.API.DTOs.Pedido;

namespace Evently.API.Services
{
    // Interfaz que define los métodos para gestionar pedidos
    public interface IPedidoService
    {
        Task<List<PedidoDto>> ObtenerTodosAsync();
        Task<PedidoDto?> ObtenerPorIdAsync(int id);
        Task<List<PedidoDto>> ObtenerPorClienteAsync(int idCliente);
        Task<PedidoDto> CrearAsync(CrearPedidoDto crearPedidoDto);
        Task<PedidoDto?> EditarAsync(int id, CrearPedidoDto crearPedidoDto);
        Task<bool> EliminarAsync(int id);
        Task<PedidoDto?> ConfirmarAsync(ConfirmarPedidoDto confirmarDto);
    }
}