namespace Evently.API.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public ICollection<Actividad> Actividades { get; set; } = new List<Actividad>();
    }
}