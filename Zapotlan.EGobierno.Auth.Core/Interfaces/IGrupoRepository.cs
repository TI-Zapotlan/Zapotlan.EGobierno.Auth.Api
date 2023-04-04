using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Core.Interfaces
{
    public interface IGrupoRepository : IRepository<Grupo>
    {
        Task<bool> ExistNameAsync(string name, Guid exceptionID = default(Guid));
    }
}
