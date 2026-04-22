using System.ComponentModel.DataAnnotations;

namespace Evently.API.DTOs.Pedido
{
    // DTO que recibe el frontend al confirmar el carrito
    public class ConfirmarPedidoDto
    {
        [Required]
        public int IdCliente { get; set; }


        [Required]
        public List<ItemCarritoDto> Actividades { get; set; } = new();
    }

    // Representa una actividad dentro del carrito
    public class ItemCarritoDto
    {
        [Required]
        public int IdActividad { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal PrecioUnitario { get; set; }
    }
}