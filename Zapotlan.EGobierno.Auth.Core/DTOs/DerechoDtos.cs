using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Enumerations;

namespace Zapotlan.EGobierno.Auth.Core.DTOs
{
    public class DerechoDto
    {
        public int ID { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public DerechoAccesoTipo Acceso { get; set; }

        public Guid UsuarioActualizacionID { get; set; }

        public DateTime FechaActualizacion { get; set; }
    }

    public class DerechoListDto
    { 
        public int ID { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public DerechoAccesoTipo Acceso { get; set; }

        public string? NombreUsuarioActualizacion { get; set; }

        public DateTime FechaActualizacion { get; set; }
    }
}
