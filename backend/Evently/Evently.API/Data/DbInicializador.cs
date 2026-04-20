using Evently.API.Models;

namespace Evently.API.Data
{
    public static class DbInicializador
    {
        public static void Inicializar(EventlyDbContext context)
        {
            if (context.Estados.Any()) return;

            var estados = new Estado[]
            {
                new Estado { NombreEstado = "Pendiente", DescripEstado = "Pedido recibido, pendiente de confirmación" },
                new Estado { NombreEstado = "Confirmado", DescripEstado = "Pedido confirmado y reserva asegurada" },
                new Estado { NombreEstado = "Cancelado", DescripEstado = "Pedido cancelado por el usuario o el sistema" }
            };
            context.Estados.AddRange(estados);
            context.SaveChanges();

            var categorias = new Categoria[]
            {
                new Categoria { NombreCatego = "Deportes", Descripcion = "Actividades deportivas y de aventura" },
                new Categoria { NombreCatego = "Cultura", Descripcion = "Museos, teatro, exposiciones y arte" },
                new Categoria { NombreCatego = "Gastronomía", Descripcion = "Catas, talleres de cocina y experiencias gastronómicas" },
                new Categoria { NombreCatego = "Naturaleza", Descripcion = "Rutas, senderismo y actividades al aire libre" }
            };
            context.Categorias.AddRange(categorias);
            context.SaveChanges();

            var actividades = new Actividad[]
            {
                new Actividad { Titulo = "Kayak en el mar", Descripcion = "Ruta en kayak por la costa mediterránea", Precio = 35.00m, IdCategoria = categorias[0].IdCategoria, CupoMaximo = 10 },
                new Actividad { Titulo = "Visita al MACA", Descripcion = "Visita guiada al Museo de Arte Contemporáneo de Alicante", Precio = 12.00m, IdCategoria = categorias[1].IdCategoria, CupoMaximo = 20 },
                new Actividad { Titulo = "Cata de vinos", Descripcion = "Cata de vinos de la DO Alicante con maridaje", Precio = 45.00m, IdCategoria = categorias[2].IdCategoria, CupoMaximo = 15 },
                new Actividad { Titulo = "Senderismo Sierra Helada", Descripcion = "Ruta de senderismo por el Parque Natural Sierra Helada", Precio = 18.00m, IdCategoria = categorias[3].IdCategoria, CupoMaximo = 25 }
            };
            context.Actividades.AddRange(actividades);
            context.SaveChanges();
        }
    }
}