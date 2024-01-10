using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Core.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetSingleAsync(Guid id);

        Task<bool> ExistUsernameAsync(string username, Guid exceptionID = default);

        Task DeleteTmpByUpdaterUserIDAsync(Guid id);

        Task<bool> IsUserValid(Guid id);

        Task<Usuario?> LoginAsync(string username, string password);

        Task<bool> HasPermissionAsync(Guid id, int derechoID);

        Task AddDerechoAsync(Guid id, Derecho item);

        Task AddGrupoAsync(Guid id, Grupo item);

        //Task<bool> RemoveGrupo(Grupo item);

        //Task<bool> RemoveDerecho(Derecho item);
    }
}
