using Evently.Web.Models;
using Microsoft.JSInterop;

namespace Evently.Web.Services
{
    // Servicio que guarda el estado de autenticación del usuario
    public class EstadoAuthService
    {
        private readonly IJSRuntime _js;

        public EstadoAuthService(IJSRuntime js)
        {
            _js = js;
        }

        public string Token { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Rol { get; private set; } = string.Empty;
        public int IdUsuario { get; private set; }
        public DateTime Expiracion { get; private set; }

        public bool EstaAutenticado =>
            !string.IsNullOrEmpty(Token) && Expiracion > DateTime.UtcNow;

        public bool EsAdministrador =>
            EstaAutenticado && Rol == "administrador";

        public event Action? OnCambio;

        // Cargar estado desde localStorage al iniciar la app
        public async Task CargarDesdeLocalStorageAsync()
        {
            try
            {
                var token = await _js.InvokeAsync<string>("obtenerToken");
                if (!string.IsNullOrEmpty(token))
                {
                    var usuario = await _js.InvokeAsync<UsuarioLocalStorage>("obtenerUsuario");
                    Token = token;
                    Email = usuario.Email;
                    Rol = usuario.Rol;
                    IdUsuario = usuario.IdUsuario;
                    Expiracion = DateTime.Parse(usuario.Expiracion);
                }
            }
            catch { }
        }

        // Guardar datos después del login
        public async Task IniciarSesionAsync(RespuestaAuthDto respuesta)
        {
            Token = respuesta.Token;
            Email = respuesta.Email;
            Rol = respuesta.Rol;
            IdUsuario = respuesta.IdUsuario;
            Expiracion = respuesta.Expiracion;

            await _js.InvokeVoidAsync("guardarToken", respuesta.Token);
            await _js.InvokeVoidAsync("guardarUsuario",
                respuesta.Email,
                respuesta.Rol,
                respuesta.IdUsuario,
                respuesta.Expiracion.ToString("O"));

            OnCambio?.Invoke();
        }

        // Cerrar sesión
        public async Task CerrarSesionAsync()
        {
            Token = string.Empty;
            Email = string.Empty;
            Rol = string.Empty;
            IdUsuario = 0;
            Expiracion = DateTime.MinValue;

            await _js.InvokeVoidAsync("eliminarToken");
            OnCambio?.Invoke();
        }
    }

    // Clase auxiliar para deserializar datos del localStorage
    public class UsuarioLocalStorage
    {
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public int IdUsuario { get; set; }
        public string Expiracion { get; set; } = string.Empty;
    }
}