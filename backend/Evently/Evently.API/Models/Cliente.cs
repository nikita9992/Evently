using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evently.API.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCliente { get; set; }

        // Relación con Usuario (clave foránea)
        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Apellidos { get; set; } = string.Empty;

        [MaxLength(30)]
        public string? Telefono { get; set; }

        [MaxLength(15)]
        public string? CodPostal { get; set; }

        [MaxLength(100)]
        public string? Ciudad { get; set; }

        [MaxLength(200)]
        public string? Direccion { get; set; }

        [MaxLength(100)]
        public string? InfoAdicional { get; set; }

        // Navegación hacia Usuario
        public Usuario Usuario { get; set; } = null!;

        // Navegación hacia Pedidos (un cliente puede tener muchos pedidos)
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
