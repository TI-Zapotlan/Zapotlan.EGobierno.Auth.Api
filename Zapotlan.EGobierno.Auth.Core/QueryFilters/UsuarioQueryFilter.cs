using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Enumerations;

namespace Zapotlan.EGobierno.Auth.Core.QueryFilters
{
    public class UsuarioQueryFilter : DefaultQueryFilter
    {
        public Guid? AreaID { get; set; }
        public Guid? UsuarioActualizacionID { get; set; }
        public Guid? GrupoID { get; set; }
        public int? DerechoID { get; set; }
        public string? Codigo { get; set; }
        public string? Username { get; set; }
        public string? Nombre { get; set;}
        public UsuarioEstatusType? Estatus { get; set; } = UsuarioEstatusType.Ninguno;
        public UsuarioRolType? Rol { get; set; } = UsuarioRolType.Ninguno;

        public bool? IncluirSubAreas { get; set; }

        public UsuarioOrderFilterType? Orden { get; set; } = UsuarioOrderFilterType.Ninguno;
    }
}
