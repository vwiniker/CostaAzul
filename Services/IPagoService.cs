using CostaAzul.API.Models.Entities;


namespace CostaAzul.API.Services
{
    public interface IPagoService
    {
        Task<Pago> RealizarPago(Pago pago);
        Task<IEnumerable<Pago>> GetAll();
        Task<Pago?> GetById(int id);
        Task<IEnumerable<Pago>> GetByReservacion(int reservacionId);
        Task<IEnumerable<Pago>> GetByFacturacion(int facturacionId);
    }
}
