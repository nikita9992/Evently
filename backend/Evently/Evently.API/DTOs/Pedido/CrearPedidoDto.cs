using System.ComponentModel.DataAnnotations;

namespace Evently.API.DTOs.Pedido
{
    // DTO para crear un nuevo pedido
    public class CrearPedidoDto
    {
        [Required(ErrorMessage = "El IdCliente es obligatorio")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El IdEstado es obligatorio")]
        public int IdEstado { get; set; }
    }
}