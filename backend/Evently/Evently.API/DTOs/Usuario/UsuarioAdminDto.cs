namespace Evently.API.DTOs.Usuario
{
    // DTO para mostrar usuarios en el panel de administración
    public class UsuarioAdminDto
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}