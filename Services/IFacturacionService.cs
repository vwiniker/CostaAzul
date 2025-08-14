using CostaAzul.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CostaAzul.API.Services
{
    public interface IFacturacionService
    {
        Task<Facturacion> Create(Facturacion facturacion);
        Task<IEnumerable<Facturacion>> GetAll();
        Task<Facturacion?> GetById(int id);
    }
}