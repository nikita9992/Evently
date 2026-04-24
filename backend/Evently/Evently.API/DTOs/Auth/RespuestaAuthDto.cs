namespace Evently.API.DTOs.Auth
{
    //Respuesta que devuelve la API después de login o registro exitoso
    public class RespuestaAuthDto
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public DateTime Expiracion { get; set; }
        public int IdUsuario { get; set; }
    }
}