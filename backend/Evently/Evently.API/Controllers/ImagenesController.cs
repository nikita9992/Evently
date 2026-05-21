using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evently.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagenesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public ImagenesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // POST api/imagenes/subir
        [HttpPost("subir")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> SubirImagen(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest(new { mensaje = "No se ha enviado ningún archivo" });

            var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();

            if (!extensionesPermitidas.Contains(extension))
                return BadRequest(new { mensaje = "Formato no permitido. Usa JPG, PNG o WebP" });

            if (archivo.Length > 5 * 1024 * 1024)
                return BadRequest(new { mensaje = "El archivo no puede superar 5 MB" });

            var carpeta = Path.Combine(_env.WebRootPath, "img", "imagenes");
            Directory.CreateDirectory(carpeta);

            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var url = $"{baseUrl}/img/imagenes/{nombreArchivo}";
            return Ok(new { url });
        }
    }
}