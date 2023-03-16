namespace Zapotlan.EGobierno.Auth.Core.Entities
{
    public abstract class BaseEntity
    {
        public virtual Guid ID { get; set; }

        public Guid UsuarioActualizacionID { get; set; }

        public DateTime FechaActualizacion { get; set; }

    }
}
