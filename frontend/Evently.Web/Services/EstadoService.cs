using Evently.Web.Models;
using System.Net.Http.Json;

namespace Evently.Web.Services
{
    public class EstadoService
    {
        private readonly HttpClient _http;

        public EstadoService(HttpClient http)
        {
            _http = http;
        }

        // Obtiene todos los estados del backend
        public async Task<List<EstadoDto>> ObtenerTodosAsync(string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var resultado = await _http.GetFromJsonAsync<List<EstadoDto>>("api/estados");
            return resultado ?? new List<EstadoDto>();
        }

        // Crea un nuevo estado en el backend
        public async Task<EstadoDto?> CrearAsync(EstadoDto estado, string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var respuesta = await _http.PostAsJsonAsync("api/estados", new
            {
                nombreEstado = estado.NombreEstado,
                descripEstado = estado.DescripEstado
            });
            if (!respuesta.IsSuccessStatusCode)
                return null;
            return await respuesta.Content.ReadFromJsonAsync<EstadoDto>();
        }

        // Edita un estado existente en el backend
        public async Task<EstadoDto?> EditarAsync(EstadoDto estado, string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var respuesta = await _http.PutAsJsonAsync($"api/estados/{estado.IdEstado}", new
            {
                nombreEstado = estado.NombreEstado,
                descripEstado = estado.DescripEstado
            });
            if (!respuesta.IsSuccessStatusCode)
                return null;
            return await respuesta.Content.ReadFromJsonAsync<EstadoDto>();
        }

        // Elimina un estado por su id
        public async Task<(bool exito, string mensaje)> EliminarAsync(int idEstado, string token)
        {
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var respuesta = await _http.DeleteAsync($"api/estados/{idEstado}");

            if (respuesta.IsSuccessStatusCode)
                return (true, "Estado eliminado correctamente");

            var error = await respuesta.Content.ReadFromJsonAsync<RespuestaError>();
            return (false, error?.Mensaje ?? "Error al eliminar el estado");
        }

        private class RespuestaError
        {
            public string? Mensaje { get; set; }
        }
    }
}