using MediatR;
using NetCoreApp.Domain.ErrorResponseProvider;
using NetCoreApp.Infrastructure.Persistence;
using Peddle.Foundation.Common.Dtos;

namespace NetCoreApp.Application.UseCases.ExpenseCategory.GetExpenseCategory;

public class GetExpenseCategoryByIdHandler : IRequestHandler<GetExpenseCategoryByIdQuery, Domain.Entities.ExpenseCategory>
{
    private readonly ApplicationDbContext _context;

    public GetExpenseCategoryByIdHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.ExpenseCategory> Handle(GetExpenseCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.ExpenseCategories.FindAsync(request.Id);
        if (category == null) throw new BusinessException(ErrorResponsesProvider.NotFound.Code, request.Id);
        return category;
    }
}