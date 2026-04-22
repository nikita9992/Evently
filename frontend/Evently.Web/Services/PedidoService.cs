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
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var respuesta = await _http.PostAsJsonAsync("api/pedidos/confirmar", dto);
            if (!respuesta.IsSuccessStatusCode) return null;
            return await respuesta.Content.ReadFromJsonAsync<PedidoDto>();
        }

        //Obtener pedidos de un cliente
        public async Task<List<PedidoDto>> ObtenerPorClienteAsync(int idCliente, string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var resultado = await _http.GetFromJsonAsync<List<PedidoDto>>(
                $"api/pedidos/cliente/{idCliente}");
            return resultado ?? new List<PedidoDto>();
        }

        //Obtener todos los pedidos
        public async Task<List<PedidoDto>> ObtenerTodosAsync(string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var resultado = await _http.GetFromJsonAsync<List<PedidoDto>>("api/pedidos");
            return resultado ?? new List<PedidoDto>();
        }
    }
}