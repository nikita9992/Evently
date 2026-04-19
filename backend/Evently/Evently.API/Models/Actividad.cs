namespace Evently.API.Models
{
    public class Actividad
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public bool Activa { get; set; } = true;

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;

    
        public ICollection<DetallePedido> DetallesPedido { get; set; } = new List<DetallePedido>();
    }
}