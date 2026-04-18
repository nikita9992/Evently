using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evently.API.Models
{
    // Representa una actividad de ocio disponible en el escaparate
    public class Actividad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdActividad { get; set; }

        [Required]
        [ForeignKey("Categoria")]
        public int IdCategoria { get; set; }

        [Required]
        [MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        public DateTime? FechaActiv { get; set; }

        public int? CupoMaximo { get; set; }

        //Navegación hacia Categoria
        public Categoria Categoria { get; set; } = null!;

        //Una actividad puede estar en muchos pedidos
        public ICollection<DetallePedido> DetallesPedido { get; set; } = new List<DetallePedido>();
    }
}