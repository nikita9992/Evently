using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evently.API.Models
{
    //Representa cada línea de actividad dentro de un pedido

    public class DetallePedido
    {
        //Clave foránea hacia Pedido
        [Required]
        [ForeignKey("Pedido")]
        public int IdPedido { get; set; }

        //Clave foránea hacia Actividad
        [Required]
        [ForeignKey("Actividad")]
        public int IdActividad { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; }

        [NotMapped]
        public decimal ImporteLinea => Cantidad * PrecioUnitario;

        public Pedido Pedido { get; set; } = null!;

        public Actividad Actividad { get; set; } = null!;
    }
}