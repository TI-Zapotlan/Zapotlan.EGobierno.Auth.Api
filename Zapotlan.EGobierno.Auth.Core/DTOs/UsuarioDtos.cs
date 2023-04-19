using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Enumerations;

namespace Zapotlan.EGobierno.Auth.Core.DTOs
{
    public class UsuarioDto
    {
        public Guid ID { get; set; }
        public Guid? AreaID { get; set; }
        public Guid? EmpleadoID { get; set; }
        public Guid PersonaID { get; set; }
        public Guid? UsuarioJefeID { get; set; }
        public string? Username { get; set; }
        public string? Correo { get; set; }
        public string? Puesto { get; set; }
        public UsuarioEstatusType Estatus { get; set; }
        public UsuarioRolType Rol { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaVigencia { get; set; }
        public string? ArchivoCartaResponsabilidad { get; set; }
        public Guid UsuarioActualizacionID { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }

    public class UsuarioListDto
    {
        public Guid ID { get; set; }
        public string? Codigo { get; set; }
        public string? Username { get; set; }
        public string? Correo { get; set; }
        public string? Puesto { get; set; }
        public UsuarioEstatusType Estatus { get; set; }
        public UsuarioRolType Rol { get; set; }
        public string? NombreCompleto { get; set; }
        public string? NombreArea { get; set; }
    }

    public class UsuarioDetailDto : UsuarioDto
    {   
        public string? Codigo { get; set; }
        public string? Prefijo { get; set; }
        public string? Nombres { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? NombreArea { get; set; }
        public string? NombreUsernameActualizacion { get; set; }

        public ICollection<GrupoListDto>? Grupos { get; set; }
        public ICollection<DerechoListDto>? Derechos { get; set; }
    }

    public class UsuarioInsertDto
    {
        public Guid UsuarioActualizacionID { get; set; }
    }

    public class UsuarioUpdateDto
    {
        public Guid ID { get; set; }
        public Guid PersonaID { get; set; }
        public Guid? AreaID { get; set; }
        public Guid? EmpleadoID { get; set; }
        public Guid? UsuarioJefeID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Correo { get; set; }
        public string? Puesto { get; set; }
        public UsuarioEstatusType Estatus { get; set; }
        public UsuarioRolType Rol { get; set; }
        public DateTime? FechaVigencia { get; set; }
        public string? ArchivoCartaResponsabilidad { get; set; }
        public Guid UsuarioActualizacionID { get; set; }
    }

    public class UsuarioUpdatePasswordDto
    {
        public Guid ID { get; set; }
        public string? Password { get; set; }
        public Guid UsuarioActualizacionID { get; set; }
    }

    public class UsuarioLoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UsuarioHasPermisionDto
    {
        public Guid UsuarioID { get; set; }
        public int DerechoID { get; set; }
    }

    public class UsuarioIDDto
    {
        public Guid UsuarioID { get; set; }
    }
}
