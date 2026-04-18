using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evently.API.Models
{
    //Define los posibles estados en los que puede encontrarse un pedido
    public class Estado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEstado { get; set; }

        [Required]
        [MaxLength(80)]
        public string NombreEstado { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? DescripEstado { get; set; }

        //Un estado puede tener muchos pedidos
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}