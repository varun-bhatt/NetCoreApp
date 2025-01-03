using FluentValidation;
using NetCoreApp.Domain.ErrorResponseProvider;
using Peddle.Foundation.Common.Extensions;

namespace NetCoreApp.Application.UseCases.Expense.CreateExpense;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseRequestDto>
{
    public CreateExpenseValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x).NotNull()
            .WithErrorCode(ErrorResponsesProvider.UnhandledException.Code);
        
        RuleFor(x => x).Must(x => !x.Name.IsEmpty())
            .WithErrorCode(ErrorResponsesProvider.InvalidExpenseName.Code);
        
        RuleFor(x => x).Must(x => !x.Category.IsEmpty())
            .WithErrorCode(ErrorResponsesProvider.InvalidExpenseCategoryName.Code);
    }
}