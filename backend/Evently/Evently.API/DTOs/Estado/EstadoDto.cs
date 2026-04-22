namespace Evently.API.DTOs.Estado
{
    // DTO para mostrar un estado al cliente
    public class EstadoDto
    {
        public int IdEstado { get; set; }
        public string NombreEstado { get; set; } = string.Empty;
        public string? DescripEstado { get; set; }
    }
}