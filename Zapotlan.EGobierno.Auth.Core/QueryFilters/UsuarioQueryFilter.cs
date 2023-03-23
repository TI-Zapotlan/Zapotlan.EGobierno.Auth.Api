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
        public string? Codigo { get; set; }
        public string? Username { get; set; }
        public string? Nombre { get; set;}
        public UsuarioEstatusTipo? Estatus { get; set; } = UsuarioEstatusTipo.Ninguno;
        public UsuarioRolTipo? Rol { get; set; } = UsuarioRolTipo.Ninguno;

        public bool? IncluirSubAreas { get; set; }

        public UsuarioOrdenFilterTipo? Orden { get; set; } = UsuarioOrdenFilterTipo.Ninguno;
    }
}
