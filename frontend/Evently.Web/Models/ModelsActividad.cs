namespace Evently.Web.Models
{
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
        public List<string> Imagenes { get; set; } = new();
        public string? ImagenPrincipal => Imagenes.FirstOrDefault();
        public double MediaValoracion { get; set; }
        public int TotalValoraciones { get; set; }
        public int? PlazasDisponibles { get; set; }
    }

    public class CrearActividadDto
    {
        public int IdCategoria { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public DateTime? FechaActiv { get; set; }
        public int? CupoMaximo { get; set; }
        public List<string> Imagenes { get; set; } = new();
    }
}