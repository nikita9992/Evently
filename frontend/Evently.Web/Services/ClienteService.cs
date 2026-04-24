using Evently.Web.Models;
using System.Net.Http.Json;

namespace Evently.Web.Services
{

    public class ClienteService
    {
        private readonly HttpClient _http;

        public ClienteService(HttpClient http)
        {
            _http = http;
        }

        // Obtener datos del cliente por idUsuario
        public async Task<ClienteDto?> ObtenerPorUsuarioAsync(int idUsuario, string token)
        {
            try
            {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var respuesta = await _http.GetAsync($"api/clientes/usuario/{idUsuario}");

                if (!respuesta.IsSuccessStatusCode) return null;

                return await respuesta.Content.ReadFromJsonAsync<ClienteDto>();
            }
            catch
            {
                return null;
            }
        }

        //Crear datos personales
        public async Task<ClienteDto?> CrearAsync(CrearClienteDto dto, string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var respuesta = await _http.PostAsJsonAsync("api/clientes", dto);
            if (!respuesta.IsSuccessStatusCode) return null;
            return await respuesta.Content.ReadFromJsonAsync<ClienteDto>();
        }

        //Editar datos personales
        public async Task<ClienteDto?> EditarAsync(int id, CrearClienteDto dto, string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var respuesta = await _http.PutAsJsonAsync($"api/clientes/{id}", dto);
            if (!respuesta.IsSuccessStatusCode) return null;
            return await respuesta.Content.ReadFromJsonAsync<ClienteDto>();
        }
        // Obtener todos los clientes 
        public async Task<List<ClienteDto>> ObtenerTodosAsync(string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var resultado = await _http.GetFromJsonAsync<List<ClienteDto>>("api/clientes");
            return resultado ?? new List<ClienteDto>();
        }
    }
}