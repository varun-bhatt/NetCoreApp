using MediatR;
using NetCoreApp.Domain.ErrorResponseProvider;
using NetCoreApp.Infrastructure.Persistence;
using Peddle.Foundation.Common.Dtos;

namespace NetCoreApp.Application.UseCases.ExpenseCategory.DeleteExpenseCategory;

public class DeleteExpenseCategoryHandler : IRequestHandler<DeleteExpenseCategoryCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteExpenseCategoryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.ExpenseCategories.FindAsync(request.Id);
        if (category == null) throw new BusinessException(ErrorResponsesProvider.NotFound.Code, request.Id);
        category.LastModifiedAt = DateTime.UtcNow;
        category.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}