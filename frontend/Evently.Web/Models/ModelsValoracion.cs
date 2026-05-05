namespace Evently.Web.Models
{
    public class ValoracionDto
    {
        public int IdValoracion { get; set; }
        public int IdActividad { get; set; }
        public int IdUsuario { get; set; }
        public int Puntuacion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class CrearValoracionDto
    {
        public int Puntuacion { get; set; }
    }

    public class ResumenValoracionDto
    {
        public double Media { get; set; }
        public int TotalValoraciones { get; set; }
        public int? ValoracionUsuario { get; set; }
    }
}