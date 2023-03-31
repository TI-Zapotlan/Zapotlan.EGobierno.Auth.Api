using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapotlan.EGobierno.Auth.Core.DTOs
{
    public class GrupoDto
    {
        public Guid ID { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public Guid UsuarioActualizacionID { get; set; }

        public DateTime FechaActualizacion { get; set; }
    }

    public class GrupoListDto
    {
        public Guid ID { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public string? NombreUsuarioActualizacion { get; set; }

        public DateTime FechaActualizacion { get; set; }
    }
}
