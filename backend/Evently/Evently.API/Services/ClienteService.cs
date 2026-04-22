using Evently.API.Data;
using Evently.API.DTOs.Cliente;
using Evently.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Evently.API.Services
{
    // Servicio que gestiona los clientes del sistema
    public class ClienteService : IClienteService
    {
        private readonly EventlyDbContext _contexto;

        public ClienteService(EventlyDbContext contexto)
        {
            _contexto = contexto;
        }

        // Obtener todos los clientes
        public async Task<List<ClienteDto>> ObtenerTodosAsync()
        {
            return await _contexto.Clientes
                .Include(c => c.Usuario)
                .Select(c => new ClienteDto
                {
                    IdCliente = c.IdCliente,
                    IdUsuario = c.IdUsuario,
                    EmailUsuario = c.Usuario.Email,
                    Nombre = c.Nombre,
                    Apellidos = c.Apellidos,
                    Telefono = c.Telefono,
                    CodPostal = c.CodPostal,
                    Ciudad = c.Ciudad,
                    Direccion = c.Direccion,
                    InfoAdicional = c.InfoAdicional
                })
                .ToListAsync();
        }

        // Obtener un cliente por su ID
        public async Task<ClienteDto?> ObtenerPorIdAsync(int id)
        {
            var cliente = await _contexto.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (cliente == null) return null;

            return new ClienteDto
            {
                IdCliente = cliente.IdCliente,
                IdUsuario = cliente.IdUsuario,
                EmailUsuario = cliente.Usuario.Email,
                Nombre = cliente.Nombre,
                Apellidos = cliente.Apellidos,
                Telefono = cliente.Telefono,
                CodPostal = cliente.CodPostal,
                Ciudad = cliente.Ciudad,
                Direccion = cliente.Direccion,
                InfoAdicional = cliente.InfoAdicional
            };
        }

        // Obtener el cliente asociado a un usuario
        public async Task<ClienteDto?> ObtenerPorUsuarioAsync(int idUsuario)
        {
            var cliente = await _contexto.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.IdUsuario == idUsuario);

            if (cliente == null) return null;

            return new ClienteDto
            {
                IdCliente = cliente.IdCliente,
                IdUsuario = cliente.IdUsuario,
                EmailUsuario = cliente.Usuario.Email,
                Nombre = cliente.Nombre,
                Apellidos = cliente.Apellidos,
                Telefono = cliente.Telefono,
                CodPostal = cliente.CodPostal,
                Ciudad = cliente.Ciudad,
                Direccion = cliente.Direccion,
                InfoAdicional = cliente.InfoAdicional
            };
        }

        // Crear un nuevo cliente
        public async Task<ClienteDto> CrearAsync(CrearClienteDto crearClienteDto)
        {
            var nuevoCliente = new Cliente
            {
                IdUsuario = crearClienteDto.IdUsuario,
                Nombre = crearClienteDto.Nombre,
                Apellidos = crearClienteDto.Apellidos,
                Telefono = crearClienteDto.Telefono,
                CodPostal = crearClienteDto.CodPostal,
                Ciudad = crearClienteDto.Ciudad,
                Direccion = crearClienteDto.Direccion,
                InfoAdicional = crearClienteDto.InfoAdicional
            };

            _contexto.Clientes.Add(nuevoCliente);
            await _contexto.SaveChangesAsync();

            await _contexto.Entry(nuevoCliente)
                .Reference(c => c.Usuario)
                .LoadAsync();

            return new ClienteDto
            {
                IdCliente = nuevoCliente.IdCliente,
                IdUsuario = nuevoCliente.IdUsuario,
                EmailUsuario = nuevoCliente.Usuario.Email,
                Nombre = nuevoCliente.Nombre,
                Apellidos = nuevoCliente.Apellidos,
                Telefono = nuevoCliente.Telefono,
                CodPostal = nuevoCliente.CodPostal,
                Ciudad = nuevoCliente.Ciudad,
                Direccion = nuevoCliente.Direccion,
                InfoAdicional = nuevoCliente.InfoAdicional
            };
        }

        // Editar los datos de un cliente
        public async Task<ClienteDto?> EditarAsync(int id, CrearClienteDto crearClienteDto)
        {
            var cliente = await _contexto.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (cliente == null) return null;

            cliente.Nombre = crearClienteDto.Nombre;
            cliente.Apellidos = crearClienteDto.Apellidos;
            cliente.Telefono = crearClienteDto.Telefono;
            cliente.CodPostal = crearClienteDto.CodPostal;
            cliente.Ciudad = crearClienteDto.Ciudad;
            cliente.Direccion = crearClienteDto.Direccion;
            cliente.InfoAdicional = crearClienteDto.InfoAdicional;

            await _contexto.SaveChangesAsync();

            return new ClienteDto
            {
                IdCliente = cliente.IdCliente,
                IdUsuario = cliente.IdUsuario,
                EmailUsuario = cliente.Usuario.Email,
                Nombre = cliente.Nombre,
                Apellidos = cliente.Apellidos,
                Telefono = cliente.Telefono,
                CodPostal = cliente.CodPostal,
                Ciudad = cliente.Ciudad,
                Direccion = cliente.Direccion,
                InfoAdicional = cliente.InfoAdicional
            };
        }

        // Eliminar un cliente
        public async Task<bool> EliminarAsync(int id)
        {
            var cliente = await _contexto.Clientes
                .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (cliente == null) return false;

            _contexto.Clientes.Remove(cliente);
            await _contexto.SaveChangesAsync();

            return true;
        }
    }
}