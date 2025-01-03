using FluentValidation;
using NetCoreApp.Domain.ErrorResponseProvider;

namespace NetCoreApp.Application.UseCases.ExpenseCategory.UpdateExpenseCategory;

public class UpdateExpenseCategoryCommandValidator : AbstractValidator<UpdateExpenseCategoryCommand>
{
    public UpdateExpenseCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(20)
            .WithErrorCode(ErrorResponsesProvider.InvalidExpenseCategoryName.Code);
    }
}