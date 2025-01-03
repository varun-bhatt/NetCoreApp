using System.Net;

namespace NetCoreApp.Domain.ErrorResponseProvider
{
    /// <summary>
    /// Common ErrorResponse Provider for Business Exception with HttpStatus code.
    /// 
    /// </summary>
    public class ErrorResponsesProvider : IErrorResponsesProvider
    {
        public ErrorResponsesProvider()
        {
        }

        public static List<ErrorResponsesProvider> ErrorResponses { get; } = new List<ErrorResponsesProvider>();

        public ErrorResponsesProvider(string code, string message, string data, HttpStatusCode httpStatusCode)
        {
            Message = message;
            Data = data;
            Code = code;
            HttpStatusCode = httpStatusCode;
            ErrorResponses.Add(this);
        }
        
        public static ErrorResponsesProvider InvalidSearchText = new ErrorResponsesProvider("invalid_search_text", "Search Text is invalid", "invalid_search_text", HttpStatusCode.BadRequest);

        public static ErrorResponsesProvider UnhandledException = new ErrorResponsesProvider(
            "internal_server_error",
            "Something went wrong", null,
            System.Net.HttpStatusCode.InternalServerError);
        public  static ErrorResponsesProvider InvalidOfferDatabaseIds = new ErrorResponsesProvider(
            "invalid_offer_database_ids",
            "Offer DatabaseIds not found", null,
            System.Net.HttpStatusCode.NotFound);
        
        public static readonly ErrorResponsesProvider NotFound = new(
            null, null, "not_found", HttpStatusCode.NotFound);
        
        public static readonly ErrorResponsesProvider InvalidExpenseCategoryName = new(
            "invalid_category_name", "Expense category name invalid", "invalid_category_name", HttpStatusCode.BadRequest);
        
        public static readonly ErrorResponsesProvider InvalidExpenseName = new(
            "invalid_expense_name", "Expense name invalid", "invalid_expense_name", HttpStatusCode.BadRequest);

        public static readonly ErrorResponsesProvider InvalidStartDate = new("invalid_start_at",
            "Start date time invalid", "invalid_start_at", HttpStatusCode.BadRequest);

        public static readonly ErrorResponsesProvider InvalidEndDate = new("invalid_end_at", "End date time invalid",
            "invalid_end_at", HttpStatusCode.BadRequest);

        public static readonly ErrorResponsesProvider InvalidSortOrder = new("invalid_sort_order", "Sort order invalid",
            "invalid_sort_order", HttpStatusCode.BadRequest);
        
        public static readonly ErrorResponsesProvider InvalidUserId = new("invalid_user_id", "User Id invalid",
            "invalid_user_id", HttpStatusCode.BadRequest);

        public string Message { get; set; }
        public string Code { get; set; }
        public string Data { get; set; }
        public HttpStatusCode HttpStatusCode { get; }

        public ErrorResponsesProvider GetErrorResponse(string errorCode)
        {
            return ErrorResponses.First(er => er.Code.ToLowerInvariant().Equals(errorCode.ToLowerInvariant()));
        }


    }

}