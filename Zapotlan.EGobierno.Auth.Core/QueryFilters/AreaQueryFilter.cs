using Zapotlan.EGobierno.Auth.Core.Enumerations;

namespace Zapotlan.EGobierno.Auth.Core.QueryFilters
{
    public class AreaQueryFilter : DefaultQueryFilter
    {
        public Guid? AreaPadreID { get; set; }
        public string? Nombre { get; set; } = string.Empty;
        public AreaEstatusTipo? Activo { get; set; }

        public AreaOrdenFilterTipo? Orden { get; set; } = AreaOrdenFilterTipo.Ninguno;
    }
}
