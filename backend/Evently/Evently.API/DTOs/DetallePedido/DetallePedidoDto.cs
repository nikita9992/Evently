namespace Evently.API.DTOs.DetallePedido
{
    // DTO para mostrar una línea de pedido con todos sus datos
    public class DetallePedidoDto
    {
        public int IdPedido { get; set; }
        public int IdActividad { get; set; }
        public string TituloActividad { get; set; } = string.Empty;
        public decimal PrecioActividad { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal ImporteLinea { get; set; }
    }
}