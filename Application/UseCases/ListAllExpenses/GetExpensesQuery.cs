using System.Dynamic;
using MediatR;
using Peddle.Foundation.Common.Dtos;

namespace NetCoreApp.Application.UseCases.ListAllExpenses;

public class GetExpensesQuery : IRequest<Result<IEnumerable<ExpandoObject>, Exception>>
{
    public GetExpensesRequestDto GetExpensesRequest { get; set; }
    public int? UserId { get; set; }
}