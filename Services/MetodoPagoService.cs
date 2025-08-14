using Microsoft.EntityFrameworkCore;
using CostaAzul.API.Models;
using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;

namespace CostaAzul.API.Services
{
    public class MetodoPagoService : IMetodoPagoService
    {
        private readonly CostaAzulContext _context;

        public MetodoPagoService(CostaAzulContext context)
        {
            _context = context;
        }

        public IEnumerable<MetodoPago> GetAll()
        {
            return _context.MetodosPagos
                .ToList();
        }

        public MetodoPago GetById(int id)
        {
            return _context.MetodosPagos
                .FirstOrDefault(mp => mp.Id == id);
        }

        public async Task<MetodoPago> Create(MetodoPago metodoPago)
        {
            _context.MetodosPagos.Add(metodoPago);
            await _context.SaveChangesAsync();
            return metodoPago;
        }

        public async Task<MetodoPago> Update(int id, MetodoPago metodoPago)
        {
            var existingMetodoPago = await _context.MetodosPagos.FindAsync(id);
            if (existingMetodoPago == null)
                throw new Exception($"Metodo de pago con ID {id} no encontrado.");

            existingMetodoPago.Tipo = metodoPago.Tipo;
            existingMetodoPago.Detalle = metodoPago.Detalle;

            await _context.SaveChangesAsync();
            return existingMetodoPago;
        }

        public async Task<bool> Delete(int id)
        {
            var metodoPago = await _context.MetodosPagos.FindAsync(id);
            if (metodoPago == null)
                return false;

            _context.MetodosPagos.Remove(metodoPago);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
