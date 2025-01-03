using AutoMapper;
using MediatR;
using NetCoreApp.Application.Interfaces.Repositories;
using Peddle.Foundation.Common.Dtos;

namespace NetCoreApp.Application.UseCases.Expense.SearchExpense;

public class SearchExpenseEventHandler : IRequestHandler<SearchExpenseQuery, Result<List<SearchExpenseResponse>, Exception>>
{
    private readonly ILogger<SearchExpenseEventHandler> _logger;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public SearchExpenseEventHandler(
        ILogger<SearchExpenseEventHandler> logger,
        IExpenseRepository expenseRepository,
        IMapper mapper)
    {
        _logger = logger;
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<SearchExpenseResponse>, Exception>> Handle(SearchExpenseQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var expenses = await _expenseRepository.SearchExpensesAsync(request.SearchText);

            var result = new List<SearchExpenseResponse>();

            foreach (var expense in expenses)
            {
                result.Add( new SearchExpenseResponse
                    {
                        Amount = expense.Amount,
                        Description = expense.Description,
                        CreatedAt = expense.CreatedAt,
                        Category = expense.ExpenseCategory.Name,
                        Id = expense.Id,
                        Name = expense.Name,
                        UserId = expense.UserId
                    }
                );
            }

            return Result<List<SearchExpenseResponse>, Exception>.SucceedWith(result);
        }
        catch (Exception exception)
        {
            _logger.LogError($"SearchExpense failed - {{exceptionMessage}}", exception.Message);
            return Result<List<SearchExpenseResponse>, Exception>.FailWith(exception);
        }
    }
}