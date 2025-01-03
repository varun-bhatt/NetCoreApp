using MediatR;
using NetCoreApp.Domain.Entities;

namespace NetCoreApp.Application.UseCases.GetPerson
{
    public class GetPersonByIdQuery : IRequest<Expense>
    {
        public int Id { get; set; }

        public GetPersonByIdQuery(int id)
        {
            Id = id;
        }
    }
}