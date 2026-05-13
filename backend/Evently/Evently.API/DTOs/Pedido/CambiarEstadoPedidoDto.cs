using System.ComponentModel.DataAnnotations;

namespace Evently.API.DTOs.Pedido
{
    public class CambiarEstadoPedidoDto
    {
        [Required]
        public int IdEstado { get; set; }
    }
}
