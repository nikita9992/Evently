using Evently.Web;
using Evently.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Conexión con el backend API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7174/")
});

// MudBlazor
builder.Services.AddMudServices();

//Servicios del frontend
builder.Services.AddScoped<AutenticacionService>();
builder.Services.AddScoped<ActividadService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<EstadoAuthService>();
builder.Services.AddScoped<EstadoService>();
builder.Services.AddScoped<ComentarioService>();
builder.Services.AddScoped<ValoracionService>();
builder.Services.AddScoped<ImagenService>();


var cultura = new System.Globalization.CultureInfo("es-ES");
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultura;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultura;

await builder.Build().RunAsync();