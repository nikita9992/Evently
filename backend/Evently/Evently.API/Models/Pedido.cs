using Microsoft.AspNetCore.Identity;

namespace Evently.API.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }

        public string UsuarioId { get; set; } = string.Empty;
        public IdentityUser Usuario { get; set; } = null!;

        public int EstadoId { get; set; }
        public Estado Estado { get; set; } = null!;

        public ICollection<DetallePedido> DetallesPedido { get; set; } = new List<DetallePedido>();
    }
}