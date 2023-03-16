using System.ComponentModel.DataAnnotations.Schema;

namespace Zapotlan.EGobierno.Auth.Core.Entities
{
    public class Grupo : BaseEntity
    {
        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        // RELATIONS
        //[ForeignKey("IdUsuarioActualizacion")] - En OnModelCreating es builder.Ignore
        public virtual Usuario? UsuarioActualizacion { get; set; }

        public virtual ICollection<Usuario>? Usuarios { get; set; }
        public virtual ICollection<Derecho>? Derechos { get; set; }
    }
}
