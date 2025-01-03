using MediatR;

namespace NetCoreApp.Application.UseCases.ExpenseCategory.GetAllExpenseCategories;

public class GetAllExpenseCategoriesQuery : IRequest<List<Domain.Entities.ExpenseCategory>>
{
    
}