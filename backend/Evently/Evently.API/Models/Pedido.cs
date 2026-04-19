namespace Evently.API.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public int EstadoId { get; set; }
        public Estado Estado { get; set; } = null!;

        public ICollection<DetallePedido> DetallesPedido { get; set; } = new List<DetallePedido>();
    }
}