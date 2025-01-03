using FluentValidation;
using NetCoreApp.Domain.ErrorResponseProvider;
using Peddle.Foundation.Common.Extensions;
using Peddle.Foundation.Validation.ValidationBuilders;

namespace NetCoreApp.Application.UseCases.ListAllExpenses
{
    public class GetExpensesValidator : AbstractValidator<GetExpensesQuery>
    {
        public GetExpensesValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.GetExpensesRequest).NotNull()
                .WithErrorCode(ErrorResponsesProvider.UnhandledException.Code);

            RuleFor(x => x.GetExpensesRequest.StartDate)
                .LessThanOrEqualTo(x => x.GetExpensesRequest.EndDate)
                .When(x => !x.GetExpensesRequest.StartDate.IsEmpty() && !x.GetExpensesRequest.EndDate.IsEmpty())
                .WithErrorCode(ErrorResponsesProvider.InvalidStartDate.Code);

            RuleFor(x => x.GetExpensesRequest.MinAmount)
                .LessThanOrEqualTo(x => x.GetExpensesRequest.MaxAmount)
                .When(x => x.GetExpensesRequest.MinAmount.HasValue && x.GetExpensesRequest.MaxAmount.HasValue)
                .WithErrorCode("ERR003");

            RuleFor(x => x.GetExpensesRequest.SortOrder)
                .Must(sortOrder => sortOrder == "asc" || sortOrder == "desc")
                .WithErrorCode(ErrorResponsesProvider.InvalidSortOrder.Code);
            
            RuleFor(x => x.GetExpensesRequest.StartDate)
                .Validate24HourISO8601DateTime()
                .When(x => !x.GetExpensesRequest.StartDate.IsEmpty())
                .WithErrorCode(ErrorResponsesProvider.InvalidStartDate.Code);
            
            RuleFor(x => x.GetExpensesRequest.EndDate)
                .Validate24HourISO8601DateTime()
                .When(x => !x.GetExpensesRequest.EndDate.IsEmpty())
                .WithErrorCode(ErrorResponsesProvider.InvalidEndDate.Code);
        }
    }
}