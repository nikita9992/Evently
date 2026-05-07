using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Evently.API.Data;
using Evently.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace Evently.API.Controllers
{
    //Controlador para gestionar usuarios. Solo accesible por administradores.
    [Authorize(Roles = "administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly EventlyDbContext _context;

        public UsuariosController(EventlyDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        // Obtenemos todos los usuarios de la base de datos.
        // Con Include traemos sus datos personales
        // AsNoTracking se usa porque solo consultamos, no modificamos.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = _context.Usuarios
                .Include(u => u.Cliente);

            return await usuarios.AsNoTracking().ToListAsync();
        }

        // GET: api/Usuarios/5
        // Buscamos un usuario por su id.
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        // Modificamos los datos de un usuario
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        // Creamos un nuevo usuario desde el panel de administrador.
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.IdUsuario }, usuario);
        }

        // DELETE: api/Usuarios/5
        // Eliminamos un usuario por su id.
        // Primero comprobamos que existe antes de borrarlo.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }
    }
}
