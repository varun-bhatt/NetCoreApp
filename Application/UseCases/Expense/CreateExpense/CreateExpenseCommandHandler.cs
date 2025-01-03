using AutoMapper;
using MediatR;
using NetCoreApp.Application.Interfaces.Repositories;
using Peddle.Foundation.Common.Dtos;

namespace NetCoreApp.Application.UseCases.Expense.CreateExpense;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseRequestDto, Result<CreateExpenseResponseDto, Exception>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public CreateExpenseCommandHandler(IExpenseRepository expenseRepository, IMapper mapper)
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<Result<CreateExpenseResponseDto, Exception>> Handle(CreateExpenseRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var expense = new Domain.Entities.Expense
            {
                //Id = ,
                Amount = request.Amount,
                Description = request.Description,
                Name = request.Name,
                //User = ,
                CreatedAt = DateTime.UtcNow,
                ExpenseCategory = new Domain.Entities.ExpenseCategory
                {
                    Name = request.Category
                },
                IsDeleted = false,
                UserId = request.UserId,
                //ExpenseCategoryId = ,
                LastModifiedAt = DateTime.UtcNow
            };
            
            await _expenseRepository.AddAsync(expense);
            
            var result = new CreateExpenseResponseDto{Id = expense.Id};
            
            return Result<CreateExpenseResponseDto, Exception>.SucceedWith(result);
        }
        catch (Exception ex)
        {
            return Result<CreateExpenseResponseDto, Exception>.FailWith(ex);
        }
    }
}