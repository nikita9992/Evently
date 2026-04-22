namespace Evently.Web.Models
{
    // Modelo de pedido confirmado
    public class PedidoDto
    {
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public int IdEstado { get; set; }
        public string NombreEstado { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaConfirm { get; set; }
        public List<DetallePedidoResumenDto> Detalles { get; set; } = new();
    }

    // Detalle de cada actividad dentro del pedido
    public class DetallePedidoResumenDto
    {
        public int IdActividad { get; set; }
        public string TituloActividad { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal ImporteLinea { get; set; }
    }

    //Para confirmar pedido desde el carrito
    public class ConfirmarPedidoDto
    {
        public int IdCliente { get; set; }
        public List<ItemCarritoDto> Actividades { get; set; } = new();
    }

    //Cada actividad del carrito
    public class ItemCarritoDto
    {
        public int IdActividad { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

    //Modelo del carrito en localStorage
    public class ItemCarrito
    {
        public int IdActividad { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal ImporteLinea => Precio * Cantidad;
    }
}