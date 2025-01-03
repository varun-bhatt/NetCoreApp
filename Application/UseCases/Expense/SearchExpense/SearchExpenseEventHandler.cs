using AutoMapper;
using MediatR;
using NetCoreApp.Application.Interfaces.Repositories;
using Peddle.Foundation.Common.Dtos;

namespace NetCoreApp.Application.UseCases.Expense.SearchExpense;

public class SearchExpenseEventHandler : IRequestHandler<SearchExpenseQuery, Result<IEnumerable<Domain.Entities.Expense>, Exception>>
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

    public async Task<Result<IEnumerable<Domain.Entities.Expense>, Exception>> Handle(SearchExpenseQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var expenses = await _expenseRepository.SearchExpensesAsync(request.SearchText);
            var response = _mapper.Map<IEnumerable<Domain.Entities.Expense>>(expenses);
            return Result<IEnumerable<Domain.Entities.Expense>, Exception>.SucceedWith(response);
        }
        catch (Exception exception)
        {
            _logger.LogError($"SearchExpense failed - {{exceptionMessage}}", exception.Message);
            return Result<IEnumerable<Domain.Entities.Expense>, Exception>.FailWith(exception);
        }
    }
}