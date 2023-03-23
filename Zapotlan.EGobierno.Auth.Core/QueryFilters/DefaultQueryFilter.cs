using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapotlan.EGobierno.Auth.Core.QueryFilters
{
    public abstract class DefaultQueryFilter
    {
        public int? PageSize { get; set; }

        public int? PageNumber { get; set; }
    }
}
