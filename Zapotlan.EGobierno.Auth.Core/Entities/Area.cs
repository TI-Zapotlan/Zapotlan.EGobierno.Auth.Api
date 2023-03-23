﻿using Zapotlan.EGobierno.Auth.Core.Enumerations;

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

        public AreaEstatusTipo Activo { get; set; }

        public AreaEstatusTipo? Estatus { get; set; }

        public AreaTipo Tipo { get; set; }

        public DateTime FechaAlta { get; set; }

        // RELATIONS

        public virtual Area? AreaPadre { get; set; }

        public virtual ICollection<Area>? AreasHijo { get; set; }

        public virtual ICollection<Empleado>? Empleados { get; set; }

        public virtual ICollection<Usuario>? Usuarios { get; set; }
    }
}
