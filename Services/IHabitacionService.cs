using CostaAzul.API.Models.Entities;


namespace CostaAzul.API.Services
{
    public interface IHabitacionService
    {
        IEnumerable<Habitacion> GetAll();
        Habitacion GetById(int id);
        Task<Habitacion> Create(Habitacion habitacion);
        Task<Habitacion> Update(int id, Habitacion habitacion);
        Task<bool> Delete(int id);
    }
}
