using System.ComponentModel.DataAnnotations;

namespace Evently.API.DTOs.Estado
{
    // DTO para crear o editar un estado
    public class CrearEstadoDto
    {
        [Required(ErrorMessage = "El nombre del estado es obligatorio")]
        [MaxLength(80, ErrorMessage = "El nombre no puede superar 80 caracteres")]
        public string NombreEstado { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "La descripción no puede superar 255 caracteres")]
        public string? DescripEstado { get; set; }
    }
}