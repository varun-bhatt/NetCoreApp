using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Application.UseCases.GetPerson;

namespace NetCoreApp.Controllers
{
    [ApiController]
    [Route("Person")]
    public class Controller : ControllerBase
    {
        private readonly IMediator _mediator;

        public Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonById(int id)
        {
            var person = await _mediator.Send(new GetPersonByIdQuery(id));
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }
    }
}