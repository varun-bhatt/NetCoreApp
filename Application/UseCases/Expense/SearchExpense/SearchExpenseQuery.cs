using MediatR;
using Peddle.Foundation.Common.Dtos;

namespace NetCoreApp.Application.UseCases.Expense.SearchExpense;

public class SearchExpenseQuery : IRequest<Result<List<SearchExpenseResponse>, Exception>>
{
    public string SearchText { get; set; }
}