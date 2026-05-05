using System.ComponentModel.DataAnnotations;

namespace Evently.API.DTOs.Valoracion
{
    // DTO para mostrar una valoración
    public class ValoracionDto
    {
        public int IdValoracion { get; set; }
        public int IdActividad { get; set; }
        public int IdUsuario { get; set; }
        public int Puntuacion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    // DTO para crear o actualizar una valoración
    public class CrearValoracionDto
    {
        [Required(ErrorMessage = "La puntuación es obligatoria")]
        [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5")]
        public int Puntuacion { get; set; }
    }

    // DTO con el resumen de valoraciones de una actividad
    public class ResumenValoracionDto
    {
        public double Media { get; set; }
        public int TotalValoraciones { get; set; }
        public int? ValoracionUsuario { get; set; }
    }
}