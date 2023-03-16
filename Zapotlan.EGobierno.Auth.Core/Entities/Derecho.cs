using System.ComponentModel.DataAnnotations.Schema;

namespace Zapotlan.EGobierno.Auth.Core.Entities
{
    public class Derecho : BaseEntity
    {
        [NotMapped]
        public override Guid ID { get => base.ID; set => base.ID = value; }

        public int DerechoID { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public int Acceso { get; set; }

        // RELATIONS

        [ForeignKey("UsuarioActualizacionID")]
        public virtual Usuario? UsuarioActualizacion { get; set; }

        public virtual ICollection<Usuario>? Usuarios { get; set; }
        public virtual ICollection<Grupo>? Grupos { get; set; }
    }
}
