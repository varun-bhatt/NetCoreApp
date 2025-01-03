using Newtonsoft.Json;

namespace NetCoreApp.Domain.ErrorResponseProvider.ApiErrorResponse
{
    /// <summary>
    /// API Error Response Model used for Serialization
    /// </summary>
    public class ApiErrorResponse
    {
        public ApiErrorResponse()
        {
        }

        public ApiErrorResponse(string code, string message, string data = null)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        public string Code { get; set; }
        [JsonIgnore] public string Data { get; set; }
        public string Message { get; set; }
    }
}