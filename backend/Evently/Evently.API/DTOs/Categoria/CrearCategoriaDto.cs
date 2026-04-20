using System.ComponentModel.DataAnnotations;

namespace Evently.API.DTOs.Categoria
{
    //Para crear o editar una categoría
    public class CrearCategoriaDto
    {
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres")]
        public string NombreCatego { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "La descripción no puede superar 255 caracteres")]
        public string? Descripcion { get; set; }
    }
}