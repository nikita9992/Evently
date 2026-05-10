using System.ComponentModel.DataAnnotations;

namespace Evently.API.DTOs.Usuario
{
    // DTO para cambiar únicamente el rol de un usuario
    public class CambiarRolUsuarioDto
    {
        [Required(ErrorMessage = "El rol es obligatorio")]
        [MaxLength(50, ErrorMessage = "El rol no puede superar 50 caracteres")]
        public string Rol { get; set; } = string.Empty;
    }
}