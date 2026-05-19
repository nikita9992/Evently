namespace Evently.API.DTOs.Actividad
{
    // DTO para filtrar actividades 
    public class FiltroActividadDto
    {
        public int? IdCategoria { get; set; }

        public string? Titulo { get; set; }

        public string? Ciudad { get; set; }
    }
}