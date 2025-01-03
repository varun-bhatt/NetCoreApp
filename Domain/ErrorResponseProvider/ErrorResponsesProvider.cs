﻿using System.Net;

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

        public static ErrorResponsesProvider InvalidInstantOffer = new ErrorResponsesProvider(
            "instant_offer_not_found",
            "Requested InstantOffer Not Found", null,
            HttpStatusCode.NotFound);


        public  static ErrorResponsesProvider InvalidInstantOfferId = new ErrorResponsesProvider(
            "invalid_instant_offer_id",
            "InstantofferId is invalid", null,
            System.Net.HttpStatusCode.BadRequest);

        public  static ErrorResponsesProvider UnhandledException = new ErrorResponsesProvider(
            "internal_server_error",
            "Something went wrong", null,
            System.Net.HttpStatusCode.InternalServerError);
        public  static ErrorResponsesProvider InvalidOfferDatabaseIds = new ErrorResponsesProvider(
            "invalid_offer_database_ids",
            "Offer DatabaseIds not found", null,
            System.Net.HttpStatusCode.NotFound);

        public string Message { get; set; }
        public string Code { get; set; }
        public string Data { get; set; }
        public HttpStatusCode HttpStatusCode { get; }

        public  ErrorResponsesProvider GetErrorResponse(string errorCode)
        {
            return ErrorResponses.First(er => er.Code.ToLowerInvariant().Equals(errorCode.ToLowerInvariant()));
        }

        
    }
}