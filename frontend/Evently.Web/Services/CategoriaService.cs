using Evently.Web.Models;
using System.Net.Http.Json;

namespace Evently.Web.Services
{
    public class CategoriaService
    {
        private readonly HttpClient _http;

        public CategoriaService(HttpClient http)
        {
            _http = http;
        }

        //Obtener todas las categorías
        public async Task<List<CategoriaDto>> ObtenerTodasAsync()
        {
            var resultado = await _http.GetFromJsonAsync<List<CategoriaDto>>("api/categorias");
            return resultado ?? new List<CategoriaDto>();
        }

        //Crear categoría
        public async Task<CategoriaDto?> CrearAsync(CrearCategoriaDto dto, string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var respuesta = await _http.PostAsJsonAsync("api/categorias", dto);
            if (!respuesta.IsSuccessStatusCode) return null;
            return await respuesta.Content.ReadFromJsonAsync<CategoriaDto>();
        }

        // Editar categoría
        public async Task<CategoriaDto?> EditarAsync(int id, CrearCategoriaDto dto, string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var respuesta = await _http.PutAsJsonAsync($"api/categorias/{id}", dto);
            if (!respuesta.IsSuccessStatusCode) return null;
            return await respuesta.Content.ReadFromJsonAsync<CategoriaDto>();
        }

        //Eliminar categoría 
        public async Task<bool> EliminarAsync(int id, string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var respuesta = await _http.DeleteAsync($"api/categorias/{id}");
            return respuesta.IsSuccessStatusCode;
        }
    }
}