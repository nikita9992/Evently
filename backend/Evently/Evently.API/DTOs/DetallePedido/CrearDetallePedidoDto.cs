using System.ComponentModel.DataAnnotations;

namespace Evently.API.DTOs.DetallePedido
{
    // DTO para añadir una línea de actividad a un pedido
    public class CrearDetallePedidoDto
    {
        [Required(ErrorMessage = "El IdPedido es obligatorio")]
        public int IdPedido { get; set; }

        [Required(ErrorMessage = "El IdActividad es obligatorio")]
        public int IdActividad { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
        public decimal PrecioUnitario { get; set; }
    }
}