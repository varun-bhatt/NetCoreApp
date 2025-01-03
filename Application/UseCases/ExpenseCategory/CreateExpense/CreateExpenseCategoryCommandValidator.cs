using FluentValidation;
using NetCoreApp.Domain.ErrorResponseProvider;

namespace NetCoreApp.Application.UseCases.ExpenseCategory.CreateExpense;

public class CreateExpenseCategoryCommandValidator : AbstractValidator<CreateExpenseCategoryCommand>
{
    public CreateExpenseCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(20)
            .WithErrorCode(ErrorResponsesProvider.InvalidExpenseCategoryName.Code);
    }
}