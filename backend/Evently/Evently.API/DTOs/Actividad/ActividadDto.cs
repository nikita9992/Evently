namespace Evently.API.DTOs.Actividad
{
    // DTO para mostrar una actividad al cliente
    public class ActividadDto
    {
        public int IdActividad { get; set; }
        public int IdCategoria { get; set; }
        public string NombreCatego { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public DateTime? FechaActiv { get; set; }
        public int? CupoMaximo { get; set; }
    }
}