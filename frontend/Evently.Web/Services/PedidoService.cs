using Evently.Web.Models;
using System.Net.Http.Json;

namespace Evently.Web.Services
{
    public class PedidoService
    {
        private readonly HttpClient _http;

        public PedidoService(HttpClient http)
        {
            _http = http;
        }

        // Confirmar pedido desde el carrito
        public async Task<PedidoDto?> ConfirmarPedidoAsync(ConfirmarPedidoDto dto, string token)
        {
            PonerToken(token);

            var respuesta = await _http.PostAsJsonAsync("api/pedidos/confirmar", dto);

            if (!respuesta.IsSuccessStatusCode) return null;
            
            return await respuesta.Content.ReadFromJsonAsync<PedidoDto>();
        }

        // Obtener pedidos de un cliente
        public async Task<List<PedidoDto>> ObtenerPorClienteAsync(int idCliente, string token)
        {
            PonerToken(token);

            var resultado = await _http.GetFromJsonAsync<List<PedidoDto>>(
                $"api/pedidos/cliente/{idCliente}");

            return resultado ?? new List<PedidoDto>();
        }

        // Obtener todos los pedidos
        public async Task<List<PedidoDto>> ObtenerTodosAsync(string token)
        {
            PonerToken(token);

            var resultado = await _http.GetFromJsonAsync<List<PedidoDto>>("api/pedidos");

            return resultado ?? new List<PedidoDto>();
        }

        // Cambiar el estado de un pedido desde administración
        public async Task<PedidoDto?> CambiarEstadoAsync(int idPedido, int idEstado, string token)
        {
            PonerToken(token);

            var dto = new { IdEstado = idEstado };

            var respuesta = await _http.PutAsJsonAsync($"api/pedidos/{idPedido}/estado", dto);

            if (!respuesta.IsSuccessStatusCode)
            {
                var mensaje = await respuesta.Content.ReadAsStringAsync();
                throw new Exception($"Error {respuesta.StatusCode}: {mensaje}");
            }

            return await respuesta.Content.ReadFromJsonAsync<PedidoDto>();
        }

        // Pone el token JWT en las peticiones protegidas
        private void PonerToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return;
            }

            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}