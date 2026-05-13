using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evently.API.Models
{
    // Representa un pedido o reserva realizado por un cliente
    // El pedido se crea cuando el usuario confirma el carrito
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPedido { get; set; }

        //Relación con Cliente
        [Required]
        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }

        //Relación con Estado
        [Required]
        [ForeignKey("Estado")]
        public int IdEstado { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaConfirm { get; set; } = DateTime.UtcNow;

        public Cliente Cliente { get; set; } = null!;
        public Estado Estado { get; set; } = null!;

        //Un pedido tiene muchas líneas
        public ICollection<DetallePedido> DetallesPedido { get; set; } = new List<DetallePedido>();
    }
}