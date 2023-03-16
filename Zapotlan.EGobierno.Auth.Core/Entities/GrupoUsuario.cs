using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapotlan.EGobierno.Auth.Core.Entities
{
    public class GrupoUsuario
    {
        public Guid GrupoID { get; set; }
        public Guid UsuarioID { get; set; }

        // RELATIONS

        public virtual Usuario Usuario { get; set; }
        public virtual Grupo Grupo { get; set; }
    }
}
