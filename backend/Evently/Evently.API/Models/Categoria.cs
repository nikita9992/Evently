using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evently.API.Models
{

    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCategoria { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreCatego { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Descripcion { get; set; }

        // Una categoría tiene muchas actividades 
        public ICollection<Actividad> Actividades { get; set; } = new List<Actividad>();
    }
}