using Evently.API.Data;
using Evently.API.DTOs.Auth;
using Evently.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Evently.API.Services
{
    //Servicio que gestiona el registro y login de usuarios
    public class AutenticacionService : IAutenticacionService
    {
        private readonly EventlyDbContext _contexto;
        private readonly IConfiguration _configuracion;

        public AutenticacionService(EventlyDbContext contexto, IConfiguration configuracion)
        {
            _contexto = contexto;
            _configuracion = configuracion;
        }

        //Registro
        public async Task<RespuestaAuthDto?> RegistrarAsync(RegistroDto registroDto)
        {
            var usuarioExiste = await _contexto.Usuarios
                .AnyAsync(u => u.Email == registroDto.Email);

            if (usuarioExiste)
                return null; 

            var nuevoUsuario = new Usuario
            {
                Email = registroDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registroDto.Password),
                Rol = "usuario" 
            };
            _contexto.Usuarios.Add(nuevoUsuario);
            await _contexto.SaveChangesAsync();

            return GenerarRespuestaAuth(nuevoUsuario);
        }

        //Login
        public async Task<RespuestaAuthDto?> LoginAsync(LoginDto loginDto)
        {

            var usuario = await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.Password))
                return null;

            return GenerarRespuestaAuth(usuario);
        }

        //Genera el token JWT
        private RespuestaAuthDto GenerarRespuestaAuth(Usuario usuario)
        {
            var clave = _configuracion["Jwt:Clave"]!;
            var emisor = _configuracion["Jwt:Emisor"]!;
            var audiencia = _configuracion["Jwt:Audiencia"]!;
            var expiracion = DateTime.UtcNow.AddHours(
                                   double.Parse(_configuracion["Jwt:ExpiracionHoras"]!));
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email,          usuario.Email),
                new Claim(ClaimTypes.Role,           usuario.Rol)
            };

            var claveSegura = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave));
            var credenciales = new SigningCredentials(claveSegura, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: emisor,
                audience: audiencia,
                claims: claims,
                expires: expiracion,
                signingCredentials: credenciales
            );

            return new RespuestaAuthDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = usuario.Email,
                Rol = usuario.Rol,
                Expiracion = expiracion,
                IdUsuario = usuario.IdUsuario 
            };
        }
    }
}