using MediatR;

namespace NetCoreApp.Application.UseCases.ExpenseCategory.GetExpenseCategory;

public class GetExpenseCategoryByIdQuery : IRequest<Domain.Entities.ExpenseCategory>
{
    public int Id { get; set; }
}