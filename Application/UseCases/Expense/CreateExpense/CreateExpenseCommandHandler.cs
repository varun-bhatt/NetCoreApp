using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetCoreApp.Application.Interfaces.Repositories;
using NetCoreApp.Domain.ErrorResponseProvider;
using NetCoreApp.Infrastructure.Persistence;
using Peddle.Foundation.Common.Dtos;

namespace NetCoreApp.Application.UseCases.Expense.CreateExpense;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseRequestDto, Result<CreateExpenseResponseDto, Exception>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _context;

    public CreateExpenseCommandHandler(IExpenseRepository expenseRepository, IMapper mapper, IMediator mediator, ApplicationDbContext context)
    {
        _expenseRepository = expenseRepository;
        _mediator = mediator;
        _context = context;
    }

    public async Task<Result<CreateExpenseResponseDto, Exception>> Handle(CreateExpenseRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _context.ExpenseCategories.Where(x => x.Name.ToLower() == request.Category.ToLower()).FirstOrDefaultAsync();

            if (category == null) throw new BusinessException(ErrorResponsesProvider.InvalidExpenseCategoryName.Code);
            
            var user = await _context.Users.Where(x => x.Id == request.UserId).FirstOrDefaultAsync();
            
            if(user == null) throw new BusinessException(ErrorResponsesProvider.InvalidUserId.Code);

            var expense = new Domain.Entities.Expense
            {
                Amount = request.Amount,
                Description = request.Description,
                Name = request.Name,
                User = user,
                CreatedAt = DateTime.UtcNow,
                ExpenseCategory = category,
                IsDeleted = false,
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