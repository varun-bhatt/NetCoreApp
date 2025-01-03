using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetCoreApp.Application.Interfaces.Repositories;
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
            var user = await _context.Users.Where(x => x.Id == request.UserId).FirstOrDefaultAsync();
            
            var expense = new Domain.Entities.Expense
            {
                //Id = ,
                Amount = request.Amount,
                Description = request.Description,
                Name = request.Name,
                User = user,
                CreatedAt = DateTime.UtcNow,
                ExpenseCategory = category,
                IsDeleted = false,
                //UserId = request.UserId,
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