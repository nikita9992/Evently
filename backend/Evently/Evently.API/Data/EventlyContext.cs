using Evently.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Evently.API.Data
{
    public class EventlyContext : DbContext
    {
        public EventlyContext(DbContextOptions<EventlyContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Usuario)
                .WithOne(u => u.Cliente)
                .HasForeignKey<Cliente>(c => c.UsuarioId);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Pedidos)
                .HasForeignKey(p => p.UsuarioId);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Estado)
                .WithMany(e => e.Pedidos)
                .HasForeignKey(p => p.EstadoId);

            modelBuilder.Entity<Actividad>()
                .HasOne(a => a.Categoria)
                .WithMany(c => c.Actividades)
                .HasForeignKey(a => a.CategoriaId);

            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Pedido)
                .WithMany(p => p.DetallesPedido)
                .HasForeignKey(d => d.PedidoId);

            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Actividad)
                .WithMany(a => a.DetallesPedido)
                .HasForeignKey(d => d.ActividadId);
        }
    }
}