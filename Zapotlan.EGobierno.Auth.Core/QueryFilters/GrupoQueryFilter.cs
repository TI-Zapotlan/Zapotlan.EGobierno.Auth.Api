using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Enumerations;

namespace Zapotlan.EGobierno.Auth.Core.QueryFilters
{
    public class GrupoQueryFilter : DefaultQueryFilter
    {
        public Guid? UsuarioID { get; set; }
        public int? DerechoID { get; set; }
        public Guid? UsuarioActualizacionID { get; set; }
        public string? Texto { get; set; }

        public GrupoOrderFilterType? Orden { get; set; }
    }
}
