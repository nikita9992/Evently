using System.ComponentModel.DataAnnotations;

namespace Evently.API.DTOs.Cliente
{
    // DTO para crear o actualizar los datos de un cliente
    public class CrearClienteDto
    {
        [Required(ErrorMessage = "El IdUsuario es obligatorio")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [MaxLength(150, ErrorMessage = "Los apellidos no pueden superar 150 caracteres")]
        public string Apellidos { get; set; } = string.Empty;

        [MaxLength(30, ErrorMessage = "El teléfono no puede superar 30 caracteres")]
        public string? Telefono { get; set; }

        [MaxLength(15, ErrorMessage = "El código postal no puede superar 15 caracteres")]
        public string? CodPostal { get; set; }

        [MaxLength(100, ErrorMessage = "La ciudad no puede superar 100 caracteres")]
        public string? Ciudad { get; set; }

        [MaxLength(200, ErrorMessage = "La dirección no puede superar 200 caracteres")]
        public string? Direccion { get; set; }

        [MaxLength(100, ErrorMessage = "La info adicional no puede superar 100 caracteres")]
        public string? InfoAdicional { get; set; }
    }
}