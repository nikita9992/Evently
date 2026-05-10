using System.ComponentModel.DataAnnotations;

namespace Evently.API.DTOs.Usuario
{
    // DTO para crear usuarios desde el panel de administración
    public class CrearUsuarioDto
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
        [MaxLength(255, ErrorMessage = "El email no puede superar 255 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MaxLength(255, ErrorMessage = "La contraseña no puede superar 255 caracteres")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es obligatorio")]
        [MaxLength(50, ErrorMessage = "El rol no puede superar 50 caracteres")]
        public string Rol { get; set; } = "usuario";
    }
}