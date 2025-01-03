using MediatR;

namespace NetCoreApp.Application.UseCases.ExpenseCategory.DeleteExpenseCategory;

public class DeleteExpenseCategoryCommand : IRequest
{
    public int Id { get; set; }
}