using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace NetCoreApp.Controllers;

[ApiController]
[Route("expenses")]
public class ExpensesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpensesController(IMediator mediator)
    {
        _mediator = mediator;
    }
}