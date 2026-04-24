using Evently.Web;
using Evently.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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
builder.Services.AddSingleton<EstadoAuthService>();

await builder.Build().RunAsync();