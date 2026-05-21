using System.Net.Http.Headers;
using System.Text.Json;

namespace Evently.Web.Services
{
    public class ImagenService
    {
        private readonly HttpClient _http;

        public ImagenService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string?> SubirImagenAsync(Stream stream, string nombreArchivo, string token)
        {
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/*");
            content.Add(fileContent, "archivo", nombreArchivo);

            var request = new HttpRequestMessage(HttpMethod.Post, "api/imagenes/subir")
            {
                Content = content
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("url").GetString();
        }
    }
}