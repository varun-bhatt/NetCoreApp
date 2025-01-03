﻿using MediatR;
using NetCoreApp.Application.Interfaces.Repositories;
using NetCoreApp.Domain.Entities;

namespace NetCoreApp.Application.UseCases.GetPerson
{
    public class GetPersonByIdHandler : IRequestHandler<GetPersonByIdQuery, Expense>
    {
        private readonly IGenericRepositoryAsync _repository;

        public GetPersonByIdHandler(IGenericRepositoryAsync repository)
        {
            _repository = repository;
        }

        public async Task<Expense> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id);
        }
    }
}