using CostaAzul.API.DTOs;
using CostaAzul.API.Models.Entities;

namespace CostaAzul.API.Services
{
    public interface IReservacionService
    {
        IEnumerable<Reservacion> GetAll();
        Reservacion GetById(int id);
        Task<ReservacionRespuesta> Create(Reservacion reservacion);

        Task<Reservacion> Update(int id, Reservacion reservacion);
        Task<bool> Delete(int id);
    }
}
