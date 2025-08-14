using CostaAzul.API.Models.Entities;


namespace CostaAzul.API.Services
{
    public interface IUsuarioService
    {
        IEnumerable<Usuario> GetAll();
        Usuario GetById(int id);
        Task<Usuario> Create(Usuario usuario);
        Task<Usuario> Update(int id, Usuario usuario);
        Task<bool> Delete(int id);
    }
}
