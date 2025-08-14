using Microsoft.EntityFrameworkCore;
using CostaAzul.API.Models;
using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;

namespace CostaAzul.API.Services
{
    public class HotelService : IHotelService
    {
        private readonly CostaAzulContext _context;

        public HotelService(CostaAzulContext context)
        {
            _context = context;
        }

        public IEnumerable<Hotel> GetAll()
        {
            return _context.Hoteles
                .Include(h => h.Reservaciones)
                .Include(h => h.Habitaciones)
                .Include(h => h.Opiniones)
                .ToList();
        }

        public Hotel GetById(int id)
        {
            return _context.Hoteles
                .Include(h => h.Reservaciones)
                .Include(h => h.Habitaciones)
                .Include(h => h.Opiniones)
                .FirstOrDefault(h => h.Id == id);
        }

        public async Task<Hotel> Create(Hotel hotel)
        {
            _context.Hoteles.Add(hotel);
            await _context.SaveChangesAsync();
            return hotel;
        }

        public async Task<Hotel> Update(int id, Hotel hotel)
        {
            var existingHotel = await _context.Hoteles.FindAsync(id);
            if (existingHotel == null)
                throw new Exception($"Hotel con ID {id} no encontrado.");

            existingHotel.Nombre = hotel.Nombre;
            existingHotel.Direccion = hotel.Direccion;
            existingHotel.Ciudad = hotel.Ciudad;
            existingHotel.Pais = hotel.Pais;
            existingHotel.Descripcion = hotel.Descripcion;
            existingHotel.Calificacion = hotel.Calificacion;
            existingHotel.ImageUrl = hotel.ImageUrl;

            await _context.SaveChangesAsync();
            return existingHotel;
        }

        public async Task<bool> Delete(int id)
        {
            var hotel = await _context.Hoteles.FindAsync(id);
            if (hotel == null)
                return false;

            _context.Hoteles.Remove(hotel);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
