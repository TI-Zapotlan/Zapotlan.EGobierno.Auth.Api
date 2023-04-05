using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.QueryFilters;

namespace Zapotlan.EGobierno.Auth.Core.Interfaces
{
    public interface IUsuarioService
    {
        PagedList<Usuario> Gets(UsuarioQueryFilter filters);

        Task<Usuario?> GetAsync(Guid id);

        Task<Usuario> AddAsync(Usuario item);

        Task<Usuario> UpdateAsync(Usuario item);

        Task<bool> DeleteAsync(Guid id);

        Task<bool> AddDerechoAsync(Guid id, int derechoID);

        // AUTH

        Task<Usuario?> LoginAsync(string username, string password);

        Task<bool> HasPermisionAsync(Guid id, int derecho);
    }
}
