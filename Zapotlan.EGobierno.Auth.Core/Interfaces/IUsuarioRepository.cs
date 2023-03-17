using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Core.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        //Task<IEnumerable<Usuario>> Gets();
        //Task<Usuario?> Get(Guid id);
        //Task Insert(Usuario item);
        //Task<bool> Update(Usuario item);
        //Task<bool> Delete(Guid id);

        Task<bool> ExistUsernameAsync(string username, Guid exceptionID = default);

        Task DeleteTmpByUpdaterUserIDAsync(Guid id);
    }
}
