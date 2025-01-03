using MediatR;
using Microsoft.EntityFrameworkCore;
using NetCoreApp.Infrastructure.Persistence;

namespace NetCoreApp.Application.UseCases.ExpenseCategory.GetAllExpenseCategories;

public class GetAllExpenseCategoriesHandler : IRequestHandler<GetAllExpenseCategoriesQuery, List<Domain.Entities.ExpenseCategory>>
{
    private readonly ApplicationDbContext _context;

    public GetAllExpenseCategoriesHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Domain.Entities.ExpenseCategory>> Handle(GetAllExpenseCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.ExpenseCategories.ToListAsync(cancellationToken);
    }
}