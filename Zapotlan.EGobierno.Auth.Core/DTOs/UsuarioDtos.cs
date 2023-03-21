using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapotlan.EGobierno.Auth.Core.DTOs
{
    public class UsuarioDto
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

        public int Estatus { get; set; }

        public int Rol { get; set; }

        public DateTime FechaAlta { get; set; }

        public DateTime? FechaVigencia { get; set; }

        public string? ArchivoCartaResponsabilidad { get; set; }
    }

    public class UsuarioListDto
    {
        public Guid ID { get; set; }

        public string? Codigo { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Correo { get; set; }

        public string? Puesto { get; set; }

        public int Estatus { get; set; }

        public int Rol { get; set; }

        public string? Nombres { get; set; }

        public string? PrimerApellido { get; set; }

        public string? SegundoApellido { get; set; }

        public string? NombreArea { get; set; }
    }

    public class UsuarioDetailDto
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

        public int Estatus { get; set; }

        public int Rol { get; set; }

        public DateTime FechaAlta { get; set; }

        public DateTime? FechaVigencia { get; set; }

        public string? ArchivoCartaResponsabilidad { get; set; }

        public string? NombreCompleto { get; set; }

        public string? NombreArea { get; set; }

        public string? NombreUsernameActualizacion { get; set; }

        public DateTime FechaActualizacion { get; set; }
    }

    public class UsuarioInsertDto
    {
        public Guid PersonaID { get; set; }

        public Guid? AreaID { get; set; }

        public Guid? EmpleadoID { get; set; }

        public Guid? UsuarioJefeID { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Correo { get; set; }

        public string? Puesto { get; set; }
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

        public int Estatus { get; set; }

        public int Rol { get; set; }

        public DateTime? FechaVigencia { get; set; }

        public string? ArchivoCartaResponsabilidad { get; set; }
    }

    public class UsuarioUpdatePasswordDto
    {
        public Guid ID { get; set; }

        public string? Password { get; set; }
    }

    public class UsuarioGetLoginDto
    {
        public string? Username { get; set; }

        public string? Password { get; set; }
    }


}
