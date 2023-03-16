using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapotlan.EGobierno.Auth.Core.Entities
{
    public class DerechoGrupo
    {
        public int DerechoID { get; set; }
        public Guid GrupoID { get; set; }

        // RELATIONS

        public virtual Derecho Derecho { get; set; }
        public virtual Grupo Grupo { get; set; }
    }
}
