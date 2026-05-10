using Evently.API.Data;
using Evently.API.DTOs.Usuario;
using Evently.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Evently.API.Services
{
    // Servicio que gestiona los usuarios del sistema
    public class UsuarioService : IUsuarioService
    {
        private readonly EventlyDbContext _contexto;

        public UsuarioService(EventlyDbContext contexto)
        {
            _contexto = contexto;
        }

        // Obtener todos los usuarios para el panel de administración
        public async Task<List<UsuarioAdminDto>> ObtenerTodosAsync()
        {
            return await _contexto.Usuarios
                .AsNoTracking()
                .Select(u => new UsuarioAdminDto
                {
                    IdUsuario = u.IdUsuario,
                    Email = u.Email,
                    Rol = u.Rol
                })
                .ToListAsync();
        }

        // Obtener un usuario por su id
        public async Task<UsuarioAdminDto?> ObtenerPorIdAsync(int id)
        {
            Usuario? usuario = await _contexto.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return null;
            }

            return new UsuarioAdminDto
            {
                IdUsuario = usuario.IdUsuario,
                Email = usuario.Email,
                Rol = usuario.Rol
            };
        }

        // Crear un nuevo usuario desde el panel de administración
        public async Task<UsuarioAdminDto?> CrearAsync(CrearUsuarioDto crearUsuarioDto)
        {
            bool existeEmail = await _contexto.Usuarios
                .AnyAsync(u => u.Email == crearUsuarioDto.Email);

            if (existeEmail)
            {
                return null;
            }

            Usuario usuario = new Usuario
            {
                Email = crearUsuarioDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(crearUsuarioDto.Password),
                Rol = string.IsNullOrWhiteSpace(crearUsuarioDto.Rol) ? "usuario" : crearUsuarioDto.Rol
            };

            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();

            return new UsuarioAdminDto
            {
                IdUsuario = usuario.IdUsuario,
                Email = usuario.Email,
                Rol = usuario.Rol
            };
        }

        // Cambiar únicamente el rol de un usuario
        public async Task<UsuarioAdminDto?> CambiarRolAsync(int id, CambiarRolUsuarioDto cambiarRolUsuarioDto)
        {
            Usuario? usuario = await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return null;
            }

            if (cambiarRolUsuarioDto.Rol != "usuario" &&
                cambiarRolUsuarioDto.Rol != "administrador")
            {
                return null;
            }

            usuario.Rol = cambiarRolUsuarioDto.Rol;

            await _contexto.SaveChangesAsync();

            return new UsuarioAdminDto
            {
                IdUsuario = usuario.IdUsuario,
                Email = usuario.Email,
                Rol = usuario.Rol
            };
        }

        // Comprueba si el usuario tiene pedidos, comentarios o valoraciones asociadas
        public async Task<bool> TieneDatosAsociadosAsync(int id)
        {
            Cliente? cliente = await _contexto.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdUsuario == id);

            if (cliente != null)
            {
                bool tienePedidos = await _contexto.Pedidos
                    .AnyAsync(p => p.IdCliente == cliente.IdCliente);

                if (tienePedidos)
                {
                    return true;
                }
            }

            bool tieneComentarios = await _contexto.Comentarios
                .AnyAsync(c => c.IdUsuario == id);

            if (tieneComentarios)
            {
                return true;
            }

            bool tieneValoraciones = await _contexto.Valoraciones
                .AnyAsync(v => v.IdUsuario == id);

            if (tieneValoraciones)
            {
                return true;
            }

            return false;
        }

        // Eliminar un usuario
        public async Task<bool> EliminarAsync(int id)
        {
            Usuario? usuario = await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return false;
            }

            _contexto.Usuarios.Remove(usuario);
            await _contexto.SaveChangesAsync();

            return true;
        }
    }
}