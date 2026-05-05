using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evently.API.Models
{
    // Valoración de un usuario sobre una actividad
    public class Valoracion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdValoracion { get; set; }

        [Required]
        [ForeignKey("Actividad")]
        public int IdActividad { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [Range(1, 5)]
        public int Puntuacion { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navegación
        public Actividad Actividad { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
    }
}