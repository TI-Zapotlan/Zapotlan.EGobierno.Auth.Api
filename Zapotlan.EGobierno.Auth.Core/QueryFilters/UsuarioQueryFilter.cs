using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapotlan.EGobierno.Auth.Core.QueryFilters
{
    public class UsuarioQueryFilter
    {
        public Guid? AreaID { get; set; }
        public Guid? UsuarioActualizacionID { get; set; }
        public string? Codigo { get; set; }
        public string? Username { get; set; }
        public string? Nombre { get; set;}
        public int? Estatus { get; set; }
        public int? Rol { get; set; }

        public bool? IncluirSubAreas { get; set; }

        public int? Orden { get; set; }
        public int? OrdenDesc { get; set; }

        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
