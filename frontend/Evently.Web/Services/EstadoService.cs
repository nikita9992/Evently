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

        public async Task<List<EstadoDto>> ObtenerTodosAsync(string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var resultado = await _http.GetFromJsonAsync<List<EstadoDto>>("api/estados");
            return resultado ?? new List<EstadoDto>();
        }
    }
}