using System.Dynamic;
using AutoMapper;
using MediatR;
using NetCoreApp.Application.Interfaces.Repositories;
using Peddle.Foundation.Common.Dtos;
using Peddle.Foundation.Common.Pagination;
using Peddle.Offer.Extensions;

namespace NetCoreApp.Application.UseCases.ListAllExpenses
{
    public class GetExpensesQueryEventHandler : IRequestHandler<GetExpensesQuery, Result<IEnumerable<ExpandoObject>, Exception>>
    {
        private readonly ILogger<GetExpensesQueryEventHandler> _logger;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;

        public GetExpensesQueryEventHandler(IExpenseRepository expenseRepository, IMapper mapper, ILogger<GetExpensesQueryEventHandler> logger)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<ExpandoObject>, Exception>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            var getExpensesFilterModel = _mapper.Map<GetExpensesFilterModel>(request.GetExpensesRequest);
            var expenses = _expenseRepository.GetAll(getExpensesFilterModel);
            return await ShapeTheResponseData(expenses);
        }

        private async Task<Result<IEnumerable<ExpandoObject>, Exception>> ShapeTheResponseData(PagedList<GetExpensesResponseModel> getExpenses)
        {
            var response = _mapper.Map<List<GetExpensesResponseDto>>(getExpenses);
            var expensesShapedData = response.ShapeData(null);
            var obj = new ExpandoObject();
            ((IDictionary<string, object>)obj!).Add("Pagination", _mapper.Map<PaginationMetadata>(getExpenses));
            var shapedData = expensesShapedData as ExpandoObject[] ?? expensesShapedData.ToArray();
            return await Task.FromResult(Result<IEnumerable<ExpandoObject>, Exception>.SucceedWith(shapedData.Append(obj)));
        }
    }
}