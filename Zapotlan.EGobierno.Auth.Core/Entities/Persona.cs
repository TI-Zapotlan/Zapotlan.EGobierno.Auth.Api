using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Enumerations;

namespace Zapotlan.EGobierno.Auth.Core.Entities
{
    public class Persona : BaseEntity
    {
        public string? Prefijo { get; set; }

        public string Nombres { get; set; } = string.Empty;

        public string? PrimerApellido { get; set; }

        public string? SegundoApellido { get; set; }

        public string? CURP { get; set; }

        public PersonaEstadoVidaType EstadoVida { get; set; }

    }
}
