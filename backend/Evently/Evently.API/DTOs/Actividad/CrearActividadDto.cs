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

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MaxLength(500, ErrorMessage = "La descripción no puede superar 500 caracteres")]
        public string? Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        [MaxLength(100, ErrorMessage = "La ciudad no puede superar 100 caracteres")]
        public string Ciudad { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ubicación es obligatoria")]
        [MaxLength(150, ErrorMessage = "La ubicación no puede superar 150 caracteres")] 
        public string Ubicacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
        public decimal Precio { get; set; }

        public string PrecioCadena
        {
            get { return Convert.ToString(Precio).Replace(',', '.'); }
            set { Precio = Convert.ToDecimal(value.Replace('.', ',')); }
        }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha de la actividad es obligatoria")]
        public DateTime? FechaActiv { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El cupo máximo debe ser mayor que 0")]
        public int? CupoMaximo { get; set; }

        public bool EsDestacada { get; set; }

        public List<string> Imagenes { get; set; } = new();
    }
}