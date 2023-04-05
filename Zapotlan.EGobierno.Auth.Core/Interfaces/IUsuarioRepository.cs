using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Core.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<bool> ExistUsernameAsync(string username, Guid exceptionID = default);

        Task DeleteTmpByUpdaterUserIDAsync(Guid id);

        Task<bool> IsUserValid(Guid id);

        Task<Usuario?> LoginAsync(string username, string password);

        Task<bool> HasPermisionAsync(Guid id, int derechoID);

        //Task<bool> AddGrupo(Grupo item);

        Task<bool> AddDerechoAsync(Guid id, Derecho item);

        //Task<bool> RemoveGrupo(Grupo item);

        //Task<bool> RemoveDerecho(Derecho item);
    }
}
