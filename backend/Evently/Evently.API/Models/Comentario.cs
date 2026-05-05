using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evently.API.Models
{
    // Comentario de un usuario sobre una actividad
    public class Comentario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdComentario { get; set; }

        [Required]
        [ForeignKey("Actividad")]
        public int IdActividad { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Texto { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navegación
        public Actividad Actividad { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
    }
}