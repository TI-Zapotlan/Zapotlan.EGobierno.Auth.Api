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

        Task<bool> AddGrupoAsync(Guid id, Guid grupoID);

        // AUTH

        Task<Usuario?> LoginAsync(string username, string password);

        Task<bool> HasPermissionAsync(Guid id, int derechoID);
    }
}
