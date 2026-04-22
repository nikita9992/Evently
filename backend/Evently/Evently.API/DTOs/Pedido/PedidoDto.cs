namespace Evently.API.DTOs.Pedido
{
    // DTO para mostrar un pedido con todos sus datos
    public class PedidoDto
    {
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public int IdEstado { get; set; }
        public string NombreEstado { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaConfirm { get; set; }
        public List<DetallePedidoResumenDto> Detalles { get; set; } = new();
    }

    // Resumen de cada línea dentro del pedido
    public class DetallePedidoResumenDto
    {
        public int IdActividad { get; set; }
        public string TituloActividad { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal ImporteLinea { get; set; }
    }
}