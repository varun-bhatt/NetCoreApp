using MediatR;

namespace NetCoreApp.Application.UseCases.ExpenseCategory.CreateExpense;

public class CreateExpenseCategoryCommand : IRequest<int>
{
    public string Name { get; set; }
}