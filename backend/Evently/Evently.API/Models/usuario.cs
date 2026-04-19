namespace Evently.API.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = "usuario";

        public Cliente? Cliente { get; set; }
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}