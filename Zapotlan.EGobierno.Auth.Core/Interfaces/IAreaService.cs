using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.QueryFilters;

namespace Zapotlan.EGobierno.Auth.Core.Interfaces
{
    public interface IAreaService
    {
        PagedList<Area> Gets(AreaQueryFilter filters);
        Task<Area?> GetAsync(Guid id);
    }
}
