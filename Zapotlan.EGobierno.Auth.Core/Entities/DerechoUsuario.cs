using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapotlan.EGobierno.Auth.Core.Entities
{
    public class DerechoUsuario
    {
        public int DerechoID { get; set; }
        public Guid UsuarioID { get; set; }

        // RELATIONS

        public virtual Derecho Derecho { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
