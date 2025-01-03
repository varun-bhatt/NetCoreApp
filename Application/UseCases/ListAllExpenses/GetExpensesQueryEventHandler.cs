using System.Dynamic;
using AutoMapper;
using MediatR;
using NetCoreApp.Application.Interfaces.Repositories;
using Peddle.Foundation.Common.Dtos;
using Peddle.Foundation.Common.Extensions;
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
            var getExpensesFilterModel = new GetExpensesFilterModel
            {
                SortBy = request.GetExpensesRequest.SortBy,
                StartDate = !request.GetExpensesRequest.StartDate.IsEmpty() ? Convert.ToDateTime(request.GetExpensesRequest.StartDate) : null,
                SortOrder = request.GetExpensesRequest.SortOrder,
                PageNumber = request.GetExpensesRequest.PageNumber,
                EndDate = !request.GetExpensesRequest.EndDate.IsEmpty() ? Convert.ToDateTime(request.GetExpensesRequest.EndDate) : null,
                Category = request.GetExpensesRequest.Category,
                PageSize = request.GetExpensesRequest.PageSize,
                UserId = request.GetExpensesRequest.UserId,
                MinAmount = request.GetExpensesRequest.MinAmount,
                MaxAmount = request.GetExpensesRequest.MaxAmount,
            };
            var expenses = _expenseRepository.GetAll(getExpensesFilterModel);
            return await ShapeTheResponseData(expenses);
        }

        private async Task<Result<IEnumerable<ExpandoObject>, Exception>> ShapeTheResponseData(PagedList<GetExpensesResponseModel> getExpenses)
        {
            var response = new List<GetExpensesResponseDto>();

            foreach (var expense in getExpenses)
            {
                var expenseDto = new GetExpensesResponseDto
                {
                    Name = expense.Name,
                    Description = expense.Description,
                    Amount = expense.Amount,
                    Category = expense.Category,
                    Date = expense.Date
                };
                response.Add(expenseDto);
            }
            var expensesShapedData = response.ShapeData(null);
            // var obj = new ExpandoObject();
            // ((IDictionary<string, object>)obj!).Add("Pagination", _mapper.Map<PaginationMetadata>(getExpenses));
            // ((IDictionary<string, object>)obj!).Add("Pagination", new PaginationMetadata
            // {
            //     TotalCount = getExpenses.TotalCount,
            //     PageSize = getExpenses.PageSize,
            //     CurrentPage = getExpenses.CurrentPage,
            //     TotalPages = getExpenses.TotalPages,
            //     HasNext = getExpenses.HasNext,
            //     HasPrevious = getExpenses.HasPrevious
            // });
            var shapedData = expensesShapedData as ExpandoObject[] ?? expensesShapedData.ToArray();
            return await Task.FromResult(Result<IEnumerable<ExpandoObject>, Exception>.SucceedWith(shapedData));
        }
    }
}