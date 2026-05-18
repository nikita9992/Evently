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
        public string? Ciudad { get; set; }
        public string? Ubicacion { get; set; }
        public decimal Precio { get; set; }
        public DateTime? FechaActiv { get; set; }
        public int? CupoMaximo { get; set; }
        public List<string> Imagenes { get; set; } = new();
        public string? ImagenPrincipal => Imagenes.FirstOrDefault();
        public double MediaValoracion { get; set; }
        public int TotalValoraciones { get; set; }
        public int? PlazasDisponibles { get; set; }
        public bool EsDestacada { get; set; }
    }
}