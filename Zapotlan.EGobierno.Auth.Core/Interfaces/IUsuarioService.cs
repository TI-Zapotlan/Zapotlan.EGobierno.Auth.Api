using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.QueryFilters;

namespace Zapotlan.EGobierno.Auth.Core.Interfaces
{
    public interface IUsuarioService
    {
        PagedList<Usuario> Gets(UsuarioQueryFilter filters);
        Task<Usuario?> GetAsync(Guid id);
        Task AddAsync(Usuario item);
        Task<bool> UpdateAsync(Usuario item);
        Task<bool> DeleteAsync(Guid id);
    }
}
