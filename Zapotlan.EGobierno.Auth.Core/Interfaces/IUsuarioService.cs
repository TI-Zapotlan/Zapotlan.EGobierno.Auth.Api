using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Core.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> Gets();
        Task<Usuario?> Get(Guid id);
        Task Insert(Usuario item);
        Task<bool> Update(Usuario item);
        Task<bool> Delete(Guid id);
    }
}
