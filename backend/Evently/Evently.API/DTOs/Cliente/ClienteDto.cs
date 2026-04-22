namespace Evently.API.DTOs.Cliente
{
    // DTO para mostrar los datos de un cliente
    public class ClienteDto
    {
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public string EmailUsuario { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? CodPostal { get; set; }
        public string? Ciudad { get; set; }
        public string? Direccion { get; set; }
        public string? InfoAdicional { get; set; }
    }
}