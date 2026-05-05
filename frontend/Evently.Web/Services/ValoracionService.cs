using Evently.Web.Models;
using System.Net.Http.Json;

namespace Evently.Web.Services
{
    public class ValoracionService
    {
        private readonly HttpClient _http;

        public ValoracionService(HttpClient http)
        {
            _http = http;
        }

        // Obtener resumen de valoraciones
        public async Task<ResumenValoracionDto> ObtenerResumenAsync(int idActividad)
        {
            try
            {
                var resultado = await _http.GetFromJsonAsync<ResumenValoracionDto>(
                    $"api/valoraciones/actividad/{idActividad}");
                return resultado ?? new ResumenValoracionDto();
            }
            catch { return new ResumenValoracionDto(); }
        }

        // Verificar si puede valorar
        public async Task<bool> PuedeValorarAsync(int idActividad, string token)
        {
            try
            {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var respuesta = await _http.GetFromJsonAsync<PuedeValorarDto>(
                    $"api/valoraciones/actividad/{idActividad}/puedovalorar");
                return respuesta?.PuedeValorar ?? false;
            }
            catch { return false; }
        }

        // Crear o actualizar valoración
        public async Task<bool> CrearOActualizarAsync(
            int idActividad, CrearValoracionDto dto, string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var respuesta = await _http.PostAsJsonAsync(
                $"api/valoraciones/actividad/{idActividad}", dto);
            return respuesta.IsSuccessStatusCode;
        }
    }

    // Deserializar respuesta
    public class PuedeValorarDto
    {
        public bool PuedeValorar { get; set; }
    }
}