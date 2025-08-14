using Microsoft.EntityFrameworkCore;
using CostaAzul.API.Models;
using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;

namespace Hotelify.API.Services
{
    public class RolesService : IRolesService
    {
        private readonly CostaAzulContext _context;

        public RolesService(CostaAzulContext context)
        {
            _context = context;
        }

        public IEnumerable<Roles> GetAll()
        {
            return _context.Roles.ToList();
        }

        public Roles GetById(int id)
        {
            return _context.Roles.FirstOrDefault(r => r.Id == id);
        }

        public async Task<Roles> Create(Roles role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Roles> Update(int id, Roles role)
        {
            var existingRole = await _context.Roles.FindAsync(id);
            if (existingRole == null)
                throw new Exception($"Rol con ID {id} no encontrado.");

            existingRole.Tipo = role.Tipo;
            existingRole.Descripcion = role.Descripcion;

            await _context.SaveChangesAsync();
            return existingRole;
        }

        public async Task<bool> Delete(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
