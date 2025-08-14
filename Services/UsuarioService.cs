using Microsoft.EntityFrameworkCore;
using CostaAzul.API.Models;
using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;

namespace Hotelify.API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly CostaAzulContext _context;
        private readonly HashingService _hashingService;

        public UsuarioService(CostaAzulContext context, HashingService hashingService)
        {
            _context = context;
            _hashingService = hashingService;
        }

        public IEnumerable<Usuario> GetAll()
        {
            return _context.Usuarios.ToList();
        }

        public Usuario GetById(int id)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Id == id);
        }

        public async Task<Usuario> Create(Usuario usuario)
        {
            usuario.CreatedAt = DateTime.UtcNow; // Asignar la fecha de creación
            usuario.PasswordHash = _hashingService.HashPassword(usuario.PasswordHash);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> Update(int id, Usuario usuario)
        {
            var existingUser = await _context.Usuarios.FindAsync(id);
            if (existingUser == null)
                throw new Exception($"Usuario con ID {id} no encontrado.");

            existingUser.NombreUsuario = usuario.NombreUsuario;
            existingUser.Correo = usuario.Correo;
            existingUser.PasswordHash = usuario.PasswordHash;
            existingUser.RolId = usuario.RolId;

            if (!string.IsNullOrEmpty(usuario.PasswordHash))
            {
                existingUser.PasswordHash = _hashingService.HashPassword(usuario.PasswordHash);
            }

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}