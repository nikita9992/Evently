using System.ComponentModel.DataAnnotations;

namespace Evently.API.DTOs.Actividad
{
    // DTO para crear o editar una actividad 
    public class CrearActividadDto
    {
        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [MaxLength(150, ErrorMessage = "El título no puede superar 150 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "La descripción no puede superar 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
        public decimal Precio { get; set; }

        public DateTime? FechaActiv { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El cupo máximo debe ser mayor que 0")]
        public int? CupoMaximo { get; set; }
    }
}