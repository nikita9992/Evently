namespace Evently.Web.Models
{
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

    public class CrearActividadDto
    {
        public int IdCategoria { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Ciudad { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string PrecioCadena
        {
            get
            {
                if (Precio == 0)
                    return string.Empty;

                return Convert.ToString(Precio).Replace(',', '.');
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    Precio = 0;
                else
                    Precio = Convert.ToDecimal(value.Replace('.', ','));
            }
        }
        public DateTime? FechaActiv { get; set; }
        public int? CupoMaximo { get; set; }
        public bool EsDestacada { get; set; }
        public List<string> Imagenes { get; set; } = new();
    }
}