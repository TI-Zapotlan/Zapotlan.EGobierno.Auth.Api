using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Entities;

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
        public DateTime FechaActualizacion { get; set; }
        public string NombreUsuarioActualizacion { get; set; } = string.Empty;
        public int UsuariosCount { get; set; } = 0;
        public int DerechosCount { get; set; } = 0;
    }

    public class GrupoDetailsDto : GrupoDto
    {
        public string NombreUsuarioActualizacion { get; set; } = string.Empty;
        public virtual ICollection<UsuarioListDto>? Usuarios { get; set; }
        public virtual ICollection<DerechoListDto>? Derechos { get; set; }   
    }

    public class GrupoInsertDto
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public Guid UsuarioActualizacionID { get; set; }
    }

    public class GrupoUpdateDto
    {
        public Guid ID { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public Guid UsuarioActualizacionID { get; set; }
    }
}
