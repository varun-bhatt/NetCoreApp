using MediatR;
using NetCoreApp.Application.UseCases.Expense.DeleteExpense;
using NetCoreApp.Domain.ErrorResponseProvider;
using NetCoreApp.Infrastructure.Persistence;
using Peddle.Foundation.Common.Dtos;

public class DeleteExpenseHandler : IRequestHandler<DeleteExpenseCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteExpenseHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = _context.Expenses.FirstOrDefault(x=>x.Id == request.Id);
        if (expense == null) throw new BusinessException(ErrorResponsesProvider.NotFound.Code, request.Id);
        expense.LastModifiedAt = DateTime.UtcNow;
        expense.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}