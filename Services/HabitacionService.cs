using Microsoft.EntityFrameworkCore;
using CostaAzul.API.Models;
using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;

namespace CostaAzul.API.Services
{
    public class HabitacionService : IHabitacionService
    {
        private readonly CostaAzulContext _context;

        public HabitacionService(CostaAzulContext context)
        {
            _context = context;
        }

        public IEnumerable<Habitacion> GetAll()
        {
            return _context.Habitaciones
                .Include(h => h.Hotel)
                .Include(h => h.Reservaciones)
                .ToList();
        }

        public Habitacion GetById(int id)
        {
            return _context.Habitaciones
                .Include(h => h.Hotel)
                .Include(h => h.Reservaciones)
                .FirstOrDefault(h => h.Id == id);
        }

        public async Task<Habitacion> Create(Habitacion habitacion)
        {
            _context.Habitaciones.Add(habitacion);
            await _context.SaveChangesAsync();
            return habitacion;
        }

        public async Task<Habitacion> Update(int id, Habitacion habitacion)
        {
            var existingHabitacion = await _context.Habitaciones.FindAsync(id);
            if (existingHabitacion == null)
                throw new Exception($"Habitación con ID {id} no encontrada.");

            existingHabitacion.HotelId = habitacion.HotelId;
            existingHabitacion.Numero = habitacion.Numero;
            existingHabitacion.Tipo = habitacion.Tipo;
            existingHabitacion.PrecioPorPersona = habitacion.PrecioPorPersona;
            existingHabitacion.Disponibilidad = habitacion.Disponibilidad;
            existingHabitacion.Capacidad = habitacion.Capacidad;
            existingHabitacion.Descripcion = habitacion.Descripcion;

            await _context.SaveChangesAsync();
            return existingHabitacion;
        }

        public async Task<bool> Delete(int id)
        {
            var habitacion = await _context.Habitaciones.FindAsync(id);
            if (habitacion == null)
                return false;

            _context.Habitaciones.Remove(habitacion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
