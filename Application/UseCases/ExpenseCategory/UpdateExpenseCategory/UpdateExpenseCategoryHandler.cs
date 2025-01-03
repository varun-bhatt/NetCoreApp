using MediatR;
using NetCoreApp.Domain.ErrorResponseProvider;
using NetCoreApp.Infrastructure.Persistence;
using Peddle.Foundation.Common.Dtos;

namespace NetCoreApp.Application.UseCases.ExpenseCategory.UpdateExpenseCategory;

public class UpdateExpenseCategoryHandler : IRequestHandler<UpdateExpenseCategoryCommand>
{
    private readonly ApplicationDbContext _context;

    public UpdateExpenseCategoryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.ExpenseCategories.FindAsync(request.Id);
        if (category == null) throw new BusinessException(ErrorResponsesProvider.NotFound.Code, request.Id);
        category.Name = request.Name;
        category.LastModifiedAt =DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}