namespace Evently.Web.Models
{
    //Modelo para iniciar sesión
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    //Modelo para registrarse
    public class RegistroDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    //Respuesta del servidor al hacer login o registro
    public class RespuestaAuthDto
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public DateTime Expiracion { get; set; }
        public int IdUsuario { get; set; }
    }
}