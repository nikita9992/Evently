namespace Evently.Web.Models
{
    public class ComentarioDto
    {
        public int IdComentario { get; set; }
        public int IdActividad { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
    }

    public class CrearComentarioDto
    {
        public string Texto { get; set; } = string.Empty;
    }
}