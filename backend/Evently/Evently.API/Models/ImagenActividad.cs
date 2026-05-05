using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evently.API.Models
{
    // Representa imagenes asociadas a una actividad
    public class ImagenActividad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdImagen { get; set; }

        [Required]
        [ForeignKey("Actividad")]
        public int IdActividad { get; set; }

        [Required]
        [MaxLength(500)]
        public string Url { get; set; } = string.Empty;

        public int Orden { get; set; } = 0;

        // Relacion con actividades
        public Actividad Actividad { get; set; } = null!;
    }
}