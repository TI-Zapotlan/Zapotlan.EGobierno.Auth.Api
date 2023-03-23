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
    public interface IPersonaService
    {
        PagedList<Persona> Gets(PersonaQueryFilter filters);
        Task<Persona?> GetAsync(Guid id);
    }
}
