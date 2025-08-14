using CostaAzul.API.Models.Entities;


namespace CostaAzul.API.Services
{
    public interface IHotelService
    {
        IEnumerable<Hotel> GetAll();
        Hotel GetById(int id);
        Task<Hotel> Create(Hotel hotel);
        Task<Hotel> Update(int id, Hotel hotel);
        Task<bool> Delete(int id);
    }
}
