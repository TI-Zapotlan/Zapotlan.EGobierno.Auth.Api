using Zapotlan.EGobierno.Auth.Core.CustomEntities;

namespace Zapotlan.EGobierno.Auth.Api.Responses
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public Metadata Meta { get; set; }

        public ApiResponse(T data) 
        {
            Data = data;
        }
    }
}
