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
    public interface IGrupoService
    {
        PagedList<Grupo> Gets(GrupoQueryFilter filters);

        Task<Grupo?> GetAsync(Guid id);

        Task<Grupo> AddAsync(Grupo item);

        Task<Grupo> UpdateAsync(Grupo item);

        Task<bool> DeleteAsync(Guid id);
    }
}
