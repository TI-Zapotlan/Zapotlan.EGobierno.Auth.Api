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
        public int DerechoID { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DerechoAccesoType Acceso { get; set; }
        public Guid? UsuarioActualizacionID { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }

    public class DerechoListDto
    {
        public int DerechoID { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DerechoAccesoType Acceso { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        public string? NombreUsuarioActualizacion { get; set; }
        public int UsuariosCount { get; set; }
        public int GruposCount { get; set; }
    }

    public class DerechoDetailsDto : DerechoDto
    {
        public string? NombreUsuarioActualizacion { get; set; }
        public virtual ICollection<UsuarioListDto>? Usuarios { get; set; }
        public virtual ICollection<GrupoListDto>? Grupos { get; set; }
    }

    public class DerechoInsertDto
    {
        public int DerechoID { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DerechoAccesoType Acceso { get; set; }
        public Guid UsuarioActualizacionID { get; set; }
    }

    public class DerechoUpdateDto
    {
        public int DerechoID { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DerechoAccesoType Acceso { get; set; }
        public Guid UsuarioActualizacionID { get; set; }
    }
}
