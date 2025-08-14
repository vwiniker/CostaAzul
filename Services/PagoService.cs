using CostaAzul.API.Models;
using Microsoft.EntityFrameworkCore;
using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;

public class PagoService : IPagoService
{
    private readonly CostaAzulContext _context;

    public PagoService(CostaAzulContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pago>> GetAll()
    {
        return await _context.Pagos
            .Include(p => p.Reservacion)
            .Include(p => p.MetodoPago)
            .Include(p => p.Facturacion)
            .ToListAsync();
    }

    public async Task<Pago?> GetById(int id)
    {
        return await _context.Pagos
            .Include(p => p.Reservacion)
            .Include(p => p.MetodoPago)
            .Include(p => p.Facturacion)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pago> RealizarPago(Pago pago)
    {
        // Validar existencia de reservación
        var reservacion = await _context.Reservaciones
            .Include(r => r.Facturaciones)
            .FirstOrDefaultAsync(r => r.Id == pago.ReservacionId);

        if (reservacion == null)
            throw new Exception("Reservación no encontrada.");

        // 🔧 Obtener la factura asociada
        var factura = reservacion.Facturaciones.FirstOrDefault();
        if (factura == null)
            throw new Exception("No hay factura asociada a esta reservación.");

        // Asociar el pago a la factura
        pago.FacturacionId = factura.Id;
        pago.FechaPago = DateTime.UtcNow;

        // 🔒 Importante: evitar que se agregue una nueva factura por accidente
        pago.Facturacion = null;

        _context.Pagos.Add(pago);
        await _context.SaveChangesAsync();

        // Recalcular estado de la factura
        await ActualizarEstadoFactura(factura.Id);

        return pago;
    }


    private async Task ActualizarEstadoFactura(int facturaId)
    {
        var factura = await _context.Facturaciones
            .Include(f => f.Pagos)
            .FirstOrDefaultAsync(f => f.Id == facturaId);

        if (factura == null) return;

        var totalPagado = factura.Pagos.Sum(p => p.MontoTotal);

        if (totalPagado >= factura.MontoTotal)
        {
            factura.Estado = "Pagado";
        }
        else if (totalPagado == 0)
        {
            factura.Estado = "Pendiente";
        }
        else if (totalPagado < factura.MontoTotal)
        {
            factura.Estado = "Parcial";
        }

        await _context.SaveChangesAsync();
    }



    public async Task<IEnumerable<Pago>> GetByReservacion(int reservacionId)
    {
        return await _context.Pagos
            .Where(p => p.ReservacionId == reservacionId)
            .Include(p => p.MetodoPago)
            .Include(p => p.Facturacion)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pago>> GetByFacturacion(int facturacionId)
    {
        return await _context.Pagos
            .Where(p => p.FacturacionId == facturacionId)
            .Include(p => p.Reservacion)
            .Include(p => p.MetodoPago)
            .ToListAsync();
    }
}
