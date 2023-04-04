using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Enumerations;

namespace Zapotlan.EGobierno.Auth.Core.DTOs
{
    public class AreaDto
    {
        public Guid ID { get; set; }

        public Guid? AreaPadreID { get; set; }

        public string? Clave { get; set; }

        public string? Abreviacion { get; set; }

        public string? Nombre { get; set; }

        public string? NombreNomina { get; set; }

        public string? Descripcion { get; set; }

        public AreaEstatusType Activo { get; set; }

        public AreaEstatusType? Estatus { get; set; }

        public AreaType Tipo { get; set; }

        public DateTime FechaAlta { get; set; }

        public Guid UsuarioActualizacionID { get; set; }

        public DateTime FechaActualizacion { get; set; }
    }
}
