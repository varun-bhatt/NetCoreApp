using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Application.UseCases.Expense.SearchExpense;
using Peddle.Foundation.MediatR;

namespace NetCoreApp.Controllers;

[ApiController]
[Route("expenses")]
public class ExpensesController : PeddleApiControllerBase
{
    private readonly ILogger<ExpensesController> _logger;

    [Route("v1/search")]
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] SearchExpenseQuery query)
    {
        _logger.LogInformation($"{nameof(Search)} Request Message: {{Request}}", JsonSerializer.Serialize(query));

        var response = await QueryAsync(query);
        return Ok(response.SuccessValue);
    }
    
    public ExpensesController(IMediator mediator, ILogger<ExpensesController> logger) : base(mediator, logger)
    {
        _logger = logger;
    }
}