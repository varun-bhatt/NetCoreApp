using MediatR;
using NetCoreApp.Infrastructure.Persistence;

namespace NetCoreApp.Application.UseCases.ExpenseCategory.CreateExpense;

public class CreateExpenseCategoryHandler : IRequestHandler<CreateExpenseCategoryCommand, int>
{
    private readonly ApplicationDbContext _context;

    public CreateExpenseCategoryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Domain.Entities.ExpenseCategory { Name = request.Name,IsDeleted = false, CreatedAt = DateTime.Now , LastModifiedAt = DateTime.UtcNow};
        _context.ExpenseCategories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);
        return category.Id;
    }
}