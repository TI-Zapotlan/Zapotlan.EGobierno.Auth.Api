using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Core.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> Gets();
        Task<Usuario> Get(Guid id);
        Task Insert(Usuario item);
    }
}
