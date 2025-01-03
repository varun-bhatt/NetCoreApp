namespace NetCoreApp.Domain.ErrorResponseProvider
{
    public interface IErrorResponsesProvider
    {
        ErrorResponsesProvider GetErrorResponse(string errorCode);
    }
}