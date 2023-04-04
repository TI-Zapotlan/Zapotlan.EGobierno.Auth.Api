using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.QueryFilters;

namespace Zapotlan.EGobierno.Auth.Core.Interfaces
{
    public interface IDerechoService
    {
        PagedList<Derecho> Gets(DerechoQueryFilter filters);

        Task<Derecho?> GetAsync(int id);

        Task<Derecho> AddAsync(Derecho item);

        Task<Derecho> UpdateAsync(Derecho item);

        Task<bool> DeleteAsync(int id);
    }
}
