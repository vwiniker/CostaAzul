using CostaAzul.API.Models.Entities;


namespace CostaAzul.API.Services
{
    public interface IOpinionService
    {
        IEnumerable<Opinion> GetAll();
        Opinion GetById(int id);
        Task<Opinion> Create(Opinion opinion);
        Task<Opinion> Update(int id, Opinion opinion);
        Task<bool> Delete(int id);
    }
}
