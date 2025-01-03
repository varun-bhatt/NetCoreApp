using System.Text.Json;
using Peddle.Foundation.Common.Dtos;
using NetCoreApp.Domain.ErrorResponseProvider;
using ApiErrorResponse = NetCoreApp.Domain.ErrorResponseProvider.ApiErrorResponse;

namespace NetCoreApp.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        private readonly IErrorResponsesProvider _errorResponsesProvider;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger,
            IErrorResponsesProvider errorResponsesProvider)
        {
            _next = next;
            _logger = logger;
            _errorResponsesProvider = errorResponsesProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                LogLevel logLevel;
                ErrorResponsesProvider errorResponse;
                switch (error)
                {
                    case BusinessException b:
                        //business logic exceptions
                        errorResponse = _errorResponsesProvider.GetErrorResponse(b.Code);
                        response.StatusCode = (int) errorResponse.HttpStatusCode;
                        logLevel = LogLevel.Information;
                        break;
                    default:
                        // server error
                        errorResponse = _errorResponsesProvider.GetErrorResponse(ErrorResponsesProvider.UnhandledException.Code);
                        response.StatusCode = (int) errorResponse.HttpStatusCode;
                        logLevel = LogLevel.Warning;
                        break;
                }

                var result =
                    JsonSerializer.Serialize(
                        new ApiErrorResponse.ApiErrorResponse
                        {
                            Code = errorResponse.Code,
                            Message = errorResponse.Message,
                            Data = errorResponse.Data
                        },
                        new JsonSerializerOptions {IgnoreNullValues = true});
                _logger.Log(logLevel, "Failed to process the request {@Result} {Error}", result, error.StackTrace);
                await response.WriteAsync(result);
            }
        }
    }
}