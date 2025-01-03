using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Application.UseCases.ExpenseCategory.CreateExpense;
using NetCoreApp.Application.UseCases.ExpenseCategory.DeleteExpenseCategory;
using NetCoreApp.Application.UseCases.ExpenseCategory.GetAllExpenseCategories;
using NetCoreApp.Application.UseCases.ExpenseCategory.GetExpenseCategory;
using NetCoreApp.Application.UseCases.ExpenseCategory.UpdateExpenseCategory;

namespace NetCoreApp.Controllers;

[ApiController]
[Route("v1/category")]
public class ExpenseCategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpenseCategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExpenseCategoryCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateExpenseCategoryRequest request)
    {
        var command = new UpdateExpenseCategoryCommand()
        {
            Id = id,
            Name = request.Name
        };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteExpenseCategoryCommand { Id = id });
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _mediator.Send(new GetExpenseCategoryByIdQuery { Id = id });
        return Ok(category);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _mediator.Send(new GetAllExpenseCategoriesQuery());
        return Ok(categories);
    }
}