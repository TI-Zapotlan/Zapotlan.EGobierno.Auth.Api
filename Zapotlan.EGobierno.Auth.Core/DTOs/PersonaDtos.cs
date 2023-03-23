using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Enumerations;

namespace Zapotlan.EGobierno.Auth.Core.DTOs
{
    public class PersonaDto
    {
        public Guid ID { get; set; }

        public string? Prefijo { get; set; }

        public string Nombres { get; set; } = string.Empty;

        public string? PrimerApellido { get; set; }

        public string? SegundoApellido { get; set; }

        public string? CURP { get; set; }

        public PersonaEstadoVidaTipo EstadoVida { get; set; }

        public Guid UsuarioActualizacionID { get; set; }

        public DateTime FechaActualizacion { get; set; }
    }

    public class PersonaListDto
    {
        public Guid ID { get; set; }

        public string? NombreCompleto { get; set; }

        public string? NombrePorApellido { get; set; }

        public string? CURP { get; set; }

        public PersonaEstadoVidaTipo EstadoVida { get; set; }

        public string? UsuarioActualizacionNombre {get; set;}

        public DateTime FechaActualizacion { get; set; }
    }

    public class PersonaDetailsDto
    {
        public Guid ID { get; set; }

        public string? Prefijo { get; set; }

        public string Nombres { get; set; } = string.Empty;

        public string? PrimerApellido { get; set; }

        public string? SegundoApellido { get; set; }

        public string? CURP { get; set; }

        public PersonaEstadoVidaTipo EstadoVida { get; set; }

        public string? UsuarioActualizacionNombre { get; set; }

        public Guid UsuarioActualizacionID { get; set; }

        public DateTime FechaActualizacion { get; set; }
    }
}
