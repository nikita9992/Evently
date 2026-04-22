namespace Evently.Web.Models
{
    //Modelo de categoría
    public class CategoriaDto
    {
        public int IdCategoria { get; set; }
        public string NombreCatego { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    //Modelo para crear o editar una categoría
    public class CrearCategoriaDto
    {
        public string NombreCatego { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}