using System.ComponentModel.DataAnnotations.Schema;
using Zapotlan.EGobierno.Auth.Core.Enumerations;

namespace Zapotlan.EGobierno.Auth.Core.Entities
{
    public class Usuario : BaseEntity
    {        
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

        public DateTime FechaAlta { get; set; }

        public DateTime? FechaVigencia { get; set; }

        public string? ArchivoCartaResponsabilidad { get; set; }

        // RELATIONS

        //[ForeignKey("AreaID")]
        public virtual Area? Area { get; set; }

        //[ForeignKey("EmpleadoID")]
        public virtual Empleado? Empleado { get; set; }

        //[ForeignKey("PersonaID")]
        public virtual Persona? Persona { get; set; }

        //[ForeignKey("UsuarioActualizacionID")]
        public virtual Usuario? UsuarioActualizacion { get; set; }

        public virtual ICollection<Grupo>? Grupos { get; set; }
        public virtual ICollection<Derecho>? Derechos { get; set; }
    }
}
