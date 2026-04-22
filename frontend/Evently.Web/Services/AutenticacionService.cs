using Evently.Web.Models;
using System.Net.Http.Json;

namespace Evently.Web.Services
{
    // Servicio para gestionar el login y registro
    public class AutenticacionService
    {
        private readonly HttpClient _http;

        public AutenticacionService(HttpClient http)
        {
            _http = http;
        }

        //Registrar nuevo usuario
        public async Task<RespuestaAuthDto?> RegistrarAsync(RegistroDto registroDto)
        {
            var respuesta = await _http.PostAsJsonAsync("api/autenticacion/registro", registroDto);
            if (!respuesta.IsSuccessStatusCode) return null;
            return await respuesta.Content.ReadFromJsonAsync<RespuestaAuthDto>();
        }

        //Iniciar sesión
        public async Task<RespuestaAuthDto?> LoginAsync(LoginDto loginDto)
        {
            var respuesta = await _http.PostAsJsonAsync("api/autenticacion/login", loginDto);
            if (!respuesta.IsSuccessStatusCode) return null;
            return await respuesta.Content.ReadFromJsonAsync<RespuestaAuthDto>();
        }
    }
}