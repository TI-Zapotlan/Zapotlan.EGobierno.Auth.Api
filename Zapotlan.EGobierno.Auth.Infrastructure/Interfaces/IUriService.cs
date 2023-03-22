using Zapotlan.EGobierno.Auth.Core.QueryFilters;

namespace Zapotlan.EGobierno.Auth.Infrastructure.Interfaces
{
    public interface IUriService
    {
        Uri GetUsuarioPaginationUri(UsuarioQueryFilter filtres, string actionUrl);
    }
}
