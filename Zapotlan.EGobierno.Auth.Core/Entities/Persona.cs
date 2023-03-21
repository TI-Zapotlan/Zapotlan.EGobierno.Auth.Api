using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapotlan.EGobierno.Auth.Core.Entities
{
    public class Persona : BaseEntity
    {
        public string? Prefijo { get; set; }

        public string Nombres { get; set; } = string.Empty;

        public string? PrimerApellido { get; set; }

        public string? SegundoApellido { get; set; }

        public string? CURP { get; set; }

        public int EstadoVida { get; set; }

    }
}
