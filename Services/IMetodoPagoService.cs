using CostaAzul.API.Models.Entities;


namespace CostaAzul.API.Services
{
    public interface IMetodoPagoService
    {
        IEnumerable<MetodoPago> GetAll();
        MetodoPago GetById(int id);
        Task<MetodoPago> Create(MetodoPago metodoPago);
        Task<MetodoPago> Update(int id, MetodoPago metodoPago);
        Task<bool> Delete(int id);
    }
}
