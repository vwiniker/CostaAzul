using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CostaAzul.API.Models;

namespace CostaAzul.API.Services
{
    public class FacturacionService : IFacturacionService
    {
        private readonly CostaAzulContext _context;

        public FacturacionService(CostaAzulContext context)
        {
            _context = context;
        }

        public async Task<Facturacion> GetById(int id)
        {
            var factura = await _context.Facturaciones
                .Include(f => f.Reservacion)
                .Include(f => f.Pagos)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factura == null)
                throw new Exception("Factura no encontrada.");

            return factura;
        }

        public async Task<IEnumerable<Facturacion>> GetAll()
        {
            return await _context.Facturaciones
                .Include(f => f.Reservacion)
                .Include(f => f.Pagos)
                .ToListAsync();
        }

        public async Task<Facturacion> Create(Facturacion facturacion)
        {
            var reservacion = await _context.Reservaciones
                .Include(r => r.Pagos)
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(r => r.Id == facturacion.ReservacionId);

            if (reservacion == null)
                throw new Exception("La reservación especificada no existe.");

            var yaExisteFactura = await _context.Facturaciones
                .AnyAsync(f => f.ReservacionId == facturacion.ReservacionId);

            if (yaExisteFactura)
                throw new Exception("Ya existe una factura para esta reservación.");

            var nuevaFactura = new Facturacion
            {
                ReservacionId = facturacion.ReservacionId,
                NumeroFactura = "FAC-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                Fecha = DateTime.UtcNow,
                MontoTotal = reservacion.Monto,
                Estado = reservacion.Pagos.Sum(p => p.MontoTotal) >= reservacion.Monto ? "Pagado" : "Pendiente",
                DetalleFacturas = new List<DetalleFactura>()
            };

            nuevaFactura.DetalleFacturas.Add(new DetalleFactura
            {
                Descripcion = $"Hospedaje del {reservacion.FechaInicio:dd/MM/yyyy} al {reservacion.FechaFin:dd/MM/yyyy} " +
                              $"en habitación #{reservacion.Habitacion?.Numero} para {reservacion.CantidadPersonas} persona(s).",
                Monto = reservacion.Monto
            });

            _context.Facturaciones.Add(nuevaFactura);
            await _context.SaveChangesAsync();

            return nuevaFactura;
        }
    }
}

