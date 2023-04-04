using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Enumerations;

namespace Zapotlan.EGobierno.Auth.Core.QueryFilters
{
    public class DerechoQueryFilter : DefaultQueryFilter
    {
        public Guid? UsuarioID { get; set; }
        public Guid? GrupoID { get; set; }
        public string? Texto { get; set; }
        public DerechoAccesoType? Acceso { get; set; }
        public Guid? UsuarioActualizacionID { get; set; }

        public DerechoOrderFilterType? Orden { get; set; }
    }
}
