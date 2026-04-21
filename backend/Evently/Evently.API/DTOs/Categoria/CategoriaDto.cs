namespace Evently.API.DTOs.Categoria
{
    //para mostrar una categoría al cliente
    public class CategoriaDto
    {
        public int IdCategoria { get; set; }
        public string NombreCatego { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}