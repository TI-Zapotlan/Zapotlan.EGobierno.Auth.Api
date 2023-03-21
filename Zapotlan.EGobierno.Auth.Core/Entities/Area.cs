namespace Zapotlan.EGobierno.Auth.Core.Entities
{
    public class Area : BaseEntity
    {   
        public Guid? AreaPadreID { get; set; }

        public string? Clave { get; set; }

        public string? Abreviacion { get; set; }

        public string? Nombre { get; set; }

        public string? NombreNomina { get; set; }

        public string? Descripcion { get; set; }

        public int Activo { get; set; }

        public int Estatus { get; set; }

        public int Tipo { get; set; }

        public DateTime FechaAlta { get; set; }

        // RELATIONS

        // public virtual Area? AreaPadre { get; set; }

        // public virtual ICollection<Empleado>? Empleados { get; set; }

        public virtual ICollection<Usuario>? Usuarios { get; set; }
    }
}
