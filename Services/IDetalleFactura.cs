using CostaAzul.API.Models.Entities;


namespace CostaAzul.API.Services
{
    public interface IDetalleFacturaService
    {
        Task<DetalleFactura> Create(DetalleFactura detalle);
        Task<IEnumerable<DetalleFactura>> GetAll();
        Task<DetalleFactura?> GetById(int id);
        Task<DetalleFactura> Update(int id, DetalleFactura detalle);
        Task<IEnumerable<DetalleFactura>> GetByFacturacion(int facturacionId);
        Task<bool> Delete(int id);
    }
}