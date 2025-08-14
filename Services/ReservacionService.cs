using Microsoft.EntityFrameworkCore;
using CostaAzul.API.Models;
using CostaAzul.API.DTOs;
using CostaAzul.API.Models.Entities;

namespace CostaAzul.API.Services
{
    public class ReservacionService : IReservacionService
    {
        private readonly CostaAzulContext _context;

        public ReservacionService(CostaAzulContext context)
        {
            _context = context;
        }

        public IEnumerable<Reservacion> GetAll()
        {
            return _context.Reservaciones
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .Include(r => r.Facturaciones)
                .Include(r => r.Pagos)
                .ToList();
        }

        public Reservacion GetById(int id)
        {
            return _context.Reservaciones
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .Include(r => r.Facturaciones)
                .Include(r => r.Pagos)
                .FirstOrDefault(r => r.Id == id);
        }

        public async Task<ReservacionRespuesta> Create(Reservacion reservacion)
        {
            var habitacion = await _context.Habitaciones.FindAsync(reservacion.HabitacionId);
            if (habitacion == null)
                throw new Exception("La habitación especificada no existe.");

            if (reservacion.CantidadPersonas > habitacion.Capacidad)
                throw new Exception($"La habitación tiene una capacidad máxima de {habitacion.Capacidad} personas.");

            bool existeSolapamiento = await _context.Reservaciones
                .AnyAsync(r =>
                    r.HabitacionId == reservacion.HabitacionId &&
                    reservacion.FechaInicio < r.FechaFin &&
                    reservacion.FechaFin > r.FechaInicio
                );

            if (existeSolapamiento)
                throw new Exception("La habitación ya está reservada para las fechas seleccionadas.");

            reservacion.FechaReserva = DateTime.UtcNow;

            reservacion.Monto = CalcularMontoTotal(
                habitacion.PrecioPorPersona,
                reservacion.CantidadPersonas,
                reservacion.FechaInicio,
                reservacion.FechaFin
            );

            int cantidadDias = (reservacion.FechaFin.ToDateTime(TimeOnly.MinValue) - reservacion.FechaInicio.ToDateTime(TimeOnly.MinValue)).Days;
            if (cantidadDias <= 0) cantidadDias = 1;

            reservacion.Facturaciones ??= new List<Facturacion>();

            reservacion.Facturaciones.Add(new Facturacion
            {
                MontoTotal = reservacion.Monto,
                Fecha = DateTime.UtcNow,
                Estado = "Pendiente",
                NumeroFactura = "FAC-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                DetalleFacturas = new List<DetalleFactura>
        {
            new DetalleFactura
            {
                Descripcion = $"Hospedaje del {reservacion.FechaInicio:dd/MM/yyyy} al {reservacion.FechaFin:dd/MM/yyyy} en habitación #{habitacion.Numero} para {reservacion.CantidadPersonas} persona(s).",
                PrecioUnitario = habitacion.PrecioPorPersona,
                CantidadDias = cantidadDias,
                Subtotal = habitacion.PrecioPorPersona * reservacion.CantidadPersonas * cantidadDias
            }
        }
            });

            _context.Reservaciones.Add(reservacion);
            await _context.SaveChangesAsync();

            return new ReservacionRespuesta
            {
                Reservacion = reservacion,
                Mensaje = "Reservación y factura con detalle creada exitosamente."
            };
        }


        public async Task<Reservacion> Update(int id, Reservacion reservacion)
        {
            var existingReservacion = await _context.Reservaciones
                .Include(r => r.Habitacion)
                .Include(r => r.Facturaciones)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (existingReservacion == null)
                throw new Exception($"Reservación con ID {id} no encontrada.");

            // 🔒 Verificar si la factura ya está pagada
            var factura = existingReservacion.Facturaciones.FirstOrDefault();
            if (factura != null && factura.Estado == "Pagado")
                throw new Exception("No se puede editar una reservación que ya ha sido pagada.");

            existingReservacion.UsuarioId = reservacion.UsuarioId;
            existingReservacion.HabitacionId = reservacion.HabitacionId;
            existingReservacion.FechaInicio = reservacion.FechaInicio;
            existingReservacion.FechaFin = reservacion.FechaFin;
            existingReservacion.CantidadPersonas = reservacion.CantidadPersonas;

            // Obtener habitación actualizada
            var habitacion = await _context.Habitaciones.FindAsync(reservacion.HabitacionId);
            if (habitacion == null)
                throw new Exception("La habitación especificada no existe.");

            // Recalcular el monto total
            existingReservacion.Monto = CalcularMontoTotal(
                habitacion.PrecioPorPersona,
                reservacion.CantidadPersonas,
                reservacion.FechaInicio,
                reservacion.FechaFin
            );

            // Actualizar factura asociada (suponiendo 1 a 1)
            if (factura != null)
            {
                factura.MontoTotal = existingReservacion.Monto;
                factura.Fecha = DateTime.UtcNow;
                factura.Estado = "Actualizada"; // opcional
                // Puedes conservar el NumeroFactura
            }

            await _context.SaveChangesAsync();
            return existingReservacion;
        }


        public async Task<bool> Delete(int id)
        {
            var reservacion = await _context.Reservaciones.FindAsync(id);
            if (reservacion == null)
                return false;

            _context.Reservaciones.Remove(reservacion);
            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ Método centralizado para calcular el monto total
        private decimal CalcularMontoTotal(decimal precioPorPersona, int cantidadPersonas, DateOnly fechaInicio, DateOnly fechaFin)
        {
            // Calcular el número de días, siempre debe ser al menos 1
            int diasReservados = (fechaFin.ToDateTime(TimeOnly.MinValue) - fechaInicio.ToDateTime(TimeOnly.MinValue)).Days;

            // Validación de número de días reservados
            if (diasReservados <= 0)
                diasReservados = 1; // Considerar al menos un día si el rango es incorrecto

            // Calcular el monto total
            decimal montoTotal = precioPorPersona * cantidadPersonas * diasReservados;
            return montoTotal;
        }

    }
}
