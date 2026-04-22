using Evently.API.Data;
using Evently.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Base de datos Neon.tech
builder.Services.AddDbContext<EventlyDbContext>(opciones =>
    opciones.UseNpgsql(
        builder.Configuration.GetConnectionString("ConexionEvently")
    )
);

//JWT Autenticación
var claveJwt = builder.Configuration["Jwt:Clave"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones =>
    {
        opciones.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Emisor"],
            ValidAudience = builder.Configuration["Jwt:Audiencia"],
            IssuerSigningKey = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(claveJwt))
        };
    });

builder.Services.AddAuthorization();

//Servicios de la aplicación
builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IActividadService, ActividadService>();
builder.Services.AddScoped<IEstadoService, EstadoService>();

//Controladores
builder.Services.AddControllers();

builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy("PolicyEvently", politica =>
        politica.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
});

//Swagger con soporte JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Evently API",
        Version = "v1",
        Description = "API REST para la plataforma Evently de actividades de ocio"
    });

    // Permitir enviar token JWT desde Swagger
    opciones.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce el token JWT así: Bearer {tu_token}"
    });

    opciones.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EventlyDbContext>();
    db.Database.Migrate();
}

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opciones =>
    {
        opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "Evently API v1");
        opciones.RoutePrefix = string.Empty;
    });
}

app.UseCors("PolicyEvently");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Inicializamos la base de datos con datos de prueba
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EventlyDbContext>();
    DbInicializador.Inicializar(context);
}

app.Run();