using Evently.Web.Models;
using System.Net.Http.Json;

namespace Evently.Web.Services
{
    public class ComentarioService
    {
        private readonly HttpClient _http;

        public ComentarioService(HttpClient http)
        {
            _http = http;
        }

        // Obtener comentarios de una actividad
        public async Task<List<ComentarioDto>> ObtenerPorActividadAsync(int idActividad)
        {
            try
            {
                var resultado = await _http.GetFromJsonAsync<List<ComentarioDto>>(
                    $"api/comentarios/actividad/{idActividad}");
                return resultado ?? new List<ComentarioDto>();
            }
            catch { return new List<ComentarioDto>(); }
        }

        // Crear comentario
        public async Task<ComentarioDto?> CrearAsync(
            int idActividad, CrearComentarioDto dto, string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var respuesta = await _http.PostAsJsonAsync(
                $"api/comentarios/actividad/{idActividad}", dto);

            if (!respuesta.IsSuccessStatusCode) return null;
            return await respuesta.Content.ReadFromJsonAsync<ComentarioDto>();
        }

        // Eliminar comentario
        public async Task<bool> EliminarAsync(int idComentario, string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var respuesta = await _http.DeleteAsync(
                $"api/comentarios/{idComentario}");
            return respuesta.IsSuccessStatusCode;
        }
    }
}