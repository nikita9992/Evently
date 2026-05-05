using System.ComponentModel.DataAnnotations;

namespace Evently.API.DTOs.Comentario
{
    // DTO para mostrar un comentario
    public class ComentarioDto
    {
        public int IdComentario { get; set; }
        public int IdActividad { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
    }

    // DTO para crear un comentario
    public class CrearComentarioDto
    {
        [Required(ErrorMessage = "El texto es obligatorio")]
        [MaxLength(1000, ErrorMessage = "El comentario no puede superar 1000 caracteres")]
        [MinLength(10, ErrorMessage = "El comentario debe tener al menos 10 caracteres")]
        public string Texto { get; set; } = string.Empty;
    }
}