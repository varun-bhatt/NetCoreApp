using MediatR;

namespace NetCoreApp.Application.UseCases.ExpenseCategory.UpdateExpenseCategory;

public class UpdateExpenseCategoryCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
}