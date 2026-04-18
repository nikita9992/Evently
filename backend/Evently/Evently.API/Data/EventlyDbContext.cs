using Evently.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Evently.API.Data
{
    public class EventlyDbContext : DbContext
    {
        public EventlyDbContext(DbContextOptions<EventlyDbContext> opciones)
            : base(opciones) { }

        //Tablas de la base de datos
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Clave primaria compuesta para DetallePedido
            modelBuilder.Entity<DetallePedido>()
                .HasKey(dp => new { dp.IdPedido, dp.IdActividad });

            //Relación Usuario (1) > Cliente (0,1)
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Usuario)
                .WithOne(u => u.Cliente)
                .HasForeignKey<Cliente>(c => c.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            //Relación Categoria (1) > Actividades (N)
            modelBuilder.Entity<Actividad>()
                .HasOne(a => a.Categoria)
                .WithMany(c => c.Actividades)
                .HasForeignKey(a => a.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict);

            //Relación Cliente (1) > Pedidos (N)
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.IdCliente)
                .OnDelete(DeleteBehavior.Cascade);

            //Relación Estado (1) > Pedidos (N)
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Estado)
                .WithMany(e => e.Pedidos)
                .HasForeignKey(p => p.IdEstado)
                .OnDelete(DeleteBehavior.Restrict);

            //Relación DetallePedido(N) > Pedido(1)
            modelBuilder.Entity<DetallePedido>()
                .HasOne(dp => dp.Pedido)
                .WithMany(p => p.DetallesPedido)
                .HasForeignKey(dp => dp.IdPedido)
                .OnDelete(DeleteBehavior.Cascade);

            //Relación DetallePedido(N) > Actividad(1)
            modelBuilder.Entity<DetallePedido>()
                .HasOne(dp => dp.Actividad)
                .WithMany(a => a.DetallesPedido)
                .HasForeignKey(dp => dp.IdActividad)
                .OnDelete(DeleteBehavior.Restrict);

            //Datos iniciales: estados del sistema
            modelBuilder.Entity<Estado>().HasData(
                new Estado
                {
                    IdEstado = 1,
                    NombreEstado = "Confirmado",
                    DescripEstado = "Pedido confirmado por el usuario"
                },
                new Estado
                {
                    IdEstado = 2,
                    NombreEstado = "Cancelado",
                    DescripEstado = "Pedido cancelado"
                }
            );
        }
    }
}