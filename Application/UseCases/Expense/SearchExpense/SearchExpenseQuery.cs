using MediatR;
using Peddle.Foundation.Common.Dtos;

namespace NetCoreApp.Application.UseCases.Expense.SearchExpense;

public class SearchExpenseQuery : IRequest<Result<IEnumerable<Domain.Entities.Expense>, Exception>>
{
    public string SearchText { get; set; }
}