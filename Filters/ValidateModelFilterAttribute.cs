using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ApiErrorResponse = NetCoreApp.Domain.ErrorResponseProvider.ApiErrorResponse.ApiErrorResponse;

namespace NetCoreApp.Filters
{
    public class ValidateModelFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool hasInvalidParam = false;
            bool IsNullable(Type type) => Nullable.GetUnderlyingType(type) != null;

            if (context?.ActionDescriptor != null)
            {
                //Here we skip the below categories parameters from validations,
                //1.Optional parameter- It can be null
                //2.Nullable parameter- It can be null (int?, bool?)
                //3.Primitive type - Default value will be assigned to it and we can validate it using fluent validation and show our validation message and code
                //4.String - It can be null. In some cases we can take some action based on null value.
                var paramsToValidate = context.ActionDescriptor.Parameters.Where(pd =>
                  {
                      //We need to convert ParameterDescriptor to ControllerParameterDescriptor to access ParameterInfo
                      var cpd = (ControllerParameterDescriptor)pd;
                      return !cpd.ParameterInfo.IsOptional && !IsNullable(cpd.ParameterInfo.ParameterType)
                      && !cpd.ParameterType.IsPrimitive && cpd.ParameterType != typeof(string);
                  }).Select(pd => pd).ToList();

                //Check for null and invalid param values
                foreach (ParameterDescriptor paramDesc in paramsToValidate)
                {

                    //Params which has invalid values it can not be available in ActionArguments
                    if (!context.ActionArguments.ContainsKey(paramDesc.Name))
                    {
                        hasInvalidParam = true;
                        break;
                    }//Check for null values
                    else if (context.ActionArguments[paramDesc.Name] == null)
                    {
                        hasInvalidParam = true;
                        break;
                    }
                }
            }

            //Return error response for if any invalid param found
            if (hasInvalidParam)
            {
                var responseModel = new ApiErrorResponse
                {
                    Message = "One or more parameters are not provided or provided value is not valid",
                    Code = "invalid_request_parameters"
                };

                context.Result = new ContentResult()
                {
                    Content = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions { IgnoreNullValues = true }),
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ContentType = "application/json"
                };
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
