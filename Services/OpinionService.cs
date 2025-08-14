using Microsoft.EntityFrameworkCore;
using CostaAzul.API.Models;
using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;

namespace Hotelify.API.Services
{
    public class OpinionService : IOpinionService
    {
        private readonly CostaAzulContext _context;

        public OpinionService(CostaAzulContext context)
        {
            _context = context;
        }

        public IEnumerable<Opinion> GetAll()
        {
            return _context.Opiniones
                .Include(o => o.Usuario)
                .Include(o => o.Hotel)
                .ToList();
        }

        public Opinion GetById(int id)
        {
            return _context.Opiniones
                .Include(o => o.Usuario)
                .Include(o => o.Hotel)
                .FirstOrDefault(o => o.Id == id);
        }

        public async Task<Opinion> Create(Opinion opinion)
        {
            _context.Opiniones.Add(opinion);
            await _context.SaveChangesAsync();
            return opinion;
        }

        public async Task<Opinion> Update(int id, Opinion opinion)
        {
            var existingOpinion = await _context.Opiniones.FindAsync(id);
            if (existingOpinion == null)
                throw new Exception($"Opinion con ID {id} no encontrada.");

            existingOpinion.UsuarioId = opinion.UsuarioId;
            existingOpinion.HotelId = opinion.HotelId;
            existingOpinion.Calificacion = opinion.Calificacion;
            existingOpinion.Comentario = opinion.Comentario;
            existingOpinion.Fecha = opinion.Fecha;

            await _context.SaveChangesAsync();
            return existingOpinion;
        }

        public async Task<bool> Delete(int id)
        {
            var opinion = await _context.Opiniones.FindAsync(id);
            if (opinion == null)
                return false;

            _context.Opiniones.Remove(opinion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
