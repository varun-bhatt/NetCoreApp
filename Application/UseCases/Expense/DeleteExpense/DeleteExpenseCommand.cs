using MediatR;

namespace NetCoreApp.Application.UseCases.Expense.DeleteExpense;

public class DeleteExpenseCommand : IRequest
{
    public int Id { get; set; }
}