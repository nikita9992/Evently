namespace Evently.API.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;

        public int ActividadId { get; set; }
        public Actividad Actividad { get; set; } = null!;
    }
}