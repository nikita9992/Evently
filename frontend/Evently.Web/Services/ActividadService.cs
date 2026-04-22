using Evently.Web.Models;
using System.Net.Http.Json;

namespace Evently.Web.Services
{
    public class ActividadService
    {
        private readonly HttpClient _http;

        public ActividadService(HttpClient http)
        {
            _http = http;
        }

        // Obtener todas las actividades con filtros 
        public async Task<List<ActividadDto>> ObtenerTodasAsync(
            int? idCategoria = null,
            string? titulo = null)
        {
            var url = "api/actividades?";
            if (idCategoria.HasValue) url += $"idCategoria={idCategoria}&";
            if (!string.IsNullOrEmpty(titulo)) url += $"titulo={titulo}";

            var resultado = await _http.GetFromJsonAsync<List<ActividadDto>>(url);
            return resultado ?? new List<ActividadDto>();
        }

        // Obtener una actividad por ID
        public async Task<ActividadDto?> ObtenerPorIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<ActividadDto>($"api/actividades/{id}");
        }

        // Crear actividad
        public async Task<ActividadDto?> CrearAsync(CrearActividadDto dto, string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var respuesta = await _http.PostAsJsonAsync("api/actividades", dto);
            if (!respuesta.IsSuccessStatusCode) return null;
            return await respuesta.Content.ReadFromJsonAsync<ActividadDto>();
        }

        //Editar actividad 
        public async Task<ActividadDto?> EditarAsync(int id, CrearActividadDto dto, string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var respuesta = await _http.PutAsJsonAsync($"api/actividades/{id}", dto);
            if (!respuesta.IsSuccessStatusCode) return null;
            return await respuesta.Content.ReadFromJsonAsync<ActividadDto>();
        }

        //Eliminar actividad 
        public async Task<bool> EliminarAsync(int id, string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var respuesta = await _http.DeleteAsync($"api/actividades/{id}");
            return respuesta.IsSuccessStatusCode;
        }
    }
}