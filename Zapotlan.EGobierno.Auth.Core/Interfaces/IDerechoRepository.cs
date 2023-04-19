using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Core.Interfaces
{
    public interface IDerechoRepository : IRepository<Derecho>
    {
        Task<Derecho?> GetSingleAsync(int id);

        Task<bool> ExistNameAsync(string name, int exceptionID = default);

        Task<bool> ExistIDAsync(int id);
    }
}
