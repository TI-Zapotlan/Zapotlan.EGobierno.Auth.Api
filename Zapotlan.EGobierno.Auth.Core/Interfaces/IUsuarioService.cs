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
        IEnumerable<Usuario> Gets();
        Task<Usuario?> GetAsync(Guid id);
        Task AddAsync(Usuario item);
        Task<bool> UpdateAsync(Usuario item);
        Task<bool> DeleteAsync(Guid id);
    }
}
