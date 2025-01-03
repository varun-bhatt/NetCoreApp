using MediatR;
using Peddle.Foundation.Common.Dtos;

namespace NetCoreApp.Application.UseCases.Expense.CreateExpense;

public class CreateExpenseRequestDto : IRequest<Result<CreateExpenseResponseDto, Exception>>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Category { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public long UserId { get; set; }
}