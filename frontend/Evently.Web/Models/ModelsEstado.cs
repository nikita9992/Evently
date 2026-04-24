namespace Evently.Web.Models
{
    public class EstadoDto
    {
        public int IdEstado { get; set; }
        public string NombreEstado { get; set; } = string.Empty;
        public string? DescripEstado { get; set; }
    }
}