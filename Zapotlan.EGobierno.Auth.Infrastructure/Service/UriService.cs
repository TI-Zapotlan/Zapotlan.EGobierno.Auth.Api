using Zapotlan.EGobierno.Auth.Core.QueryFilters;
using Zapotlan.EGobierno.Auth.Infrastructure.Interfaces;

namespace Zapotlan.EGobierno.Auth.Infrastructure.Service
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        // CONSTRUCTOR

        public UriService(string baseUri) {
            _baseUri = baseUri;
        }

        // METHODS

        public Uri GetUsuarioPaginationUri(UsuarioQueryFilter filtres, string actionUrl)
        {
            string baseUrl = $"{_baseUri}{actionUrl}";

            return new Uri(baseUrl);
        }
    }
}
