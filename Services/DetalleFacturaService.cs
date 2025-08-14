using CostaAzul.API.Models.Entities;
using CostaAzul.API.Models;
using Microsoft.EntityFrameworkCore;


namespace CostaAzul.API.Services
{
    public class DetalleFacturaService : IDetalleFacturaService
    {
        private readonly CostaAzulContext _context;

        public DetalleFacturaService(CostaAzulContext context)
        {
            _context = context;
        }

        public async Task<DetalleFactura> Create(DetalleFactura detalle)
        {
            // Validar existencia de la factura
            var factura = await _context.Facturaciones.FindAsync(detalle.FacturacionId);
            if (factura == null)
                throw new Exception("La factura especificada no existe.");

            // Agregar el detalle (solo para descripción, no suma monto)
            _context.DetalleFacturas.Add(detalle);

            await _context.SaveChangesAsync();
            return detalle;
        }


        public async Task<IEnumerable<DetalleFactura>> GetAll()
        {
            return await _context.DetalleFacturas
                .Include(df => df.Facturacion)
                .ThenInclude(f => f.Pagos)
                .ToListAsync();
        }

        public async Task<DetalleFactura?> GetById(int id)
        {
            return await _context.DetalleFacturas
                .Include(df => df.Facturacion)
                .ThenInclude(f => f.Pagos)
                .FirstOrDefaultAsync(df => df.Id == id);
        }

        public async Task<IEnumerable<DetalleFactura>> GetByFacturacion(int facturacionId)
        {
            return await _context.DetalleFacturas
                .Where(df => df.FacturacionId == facturacionId)
                .Include(df => df.Facturacion)
                .ToListAsync();
        }

        public async Task<DetalleFactura> Update(int id, DetalleFactura detalle)
        {
            var existente = await _context.DetalleFacturas
                .Include(d => d.Facturacion)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (existente == null)
                throw new Exception($"DetalleFactura con ID {id} no encontrado.");

            // Obtener la factura relacionada
            var factura = existente.Facturacion;
            if (factura == null)
                throw new Exception("Factura no encontrada.");

            // Actualizar datos
            existente.Descripcion = detalle.Descripcion;
            existente.Monto = detalle.Monto;

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<bool> Delete(int id)
        {
            var detalle = await _context.DetalleFacturas.FindAsync(id);
            if (detalle == null) return false;

            // Obtener factura asociada
            var factura = await _context.Facturaciones.FindAsync(detalle.FacturacionId);
            if (factura != null)
            {
                // Restar el monto del detalle al total de la factura
                factura.MontoTotal -= detalle.Monto;
            }

            _context.DetalleFacturas.Remove(detalle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
