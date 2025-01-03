using FluentValidation;
using NetCoreApp.Domain.ErrorResponseProvider;
using Peddle.Foundation.Common.Extensions;

namespace NetCoreApp.Application.UseCases.Expense.SearchExpense;

public class SearchExpenseValidator : AbstractValidator<SearchExpenseQuery>
{
    public SearchExpenseValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(p => p.SearchText)
            .Must(x => !x.IsEmpty()).WithErrorCode(ErrorResponsesProvider.InvalidSearchText.Code);
    }
}