namespace Evently.API.DTOs.Actividad
{
    // DTO para filtrar actividades por categoría
    public class FiltroActividadDto
    {
        public int? IdCategoria { get; set; }

        public string? Titulo { get; set; }
    }
}