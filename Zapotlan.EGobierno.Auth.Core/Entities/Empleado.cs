namespace Zapotlan.EGobierno.Auth.Core.Entities
{
    public class Empleado : BaseEntity
    {
        public Guid? PersonaID { get; set; }

        public Guid? AreaID { get; set; }

        public Guid? AreaComisionID { get; set; }

        public string? Codigo { get; set; }

        public string? TipoTrabajador { get; set; }

        public string? ClavePuesto { get; set; }

        public string? NombrePuesto { get; set; }

        public decimal? SueldoDiario { get; set; }

        public string? ModalidadIMSS { get; set; }

        public DateTime? FechaIngreso { get; set; }
        
        public string? TipoNomina { get; set; }

        public DateTime FechaAlta { get; set; }

        public int Estatus { get; set; }

        // RELATIONS

        public virtual Area? Area { get; set; }

        public virtual Area? AreaComision { get; set; }

        // public virtual Persona Persona { get; set; }
    }
}
