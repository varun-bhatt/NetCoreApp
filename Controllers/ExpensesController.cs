
using System.Dynamic;
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
 using NetCoreApp.Application.UseCases.Expense.CreateExpense;
 using NetCoreApp.Application.UseCases.Expense.SearchExpense;
using NetCoreApp.Application.UseCases.ListAllExpenses;
using NetCoreApp.Domain.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Peddle.Foundation.Common.Dtos;
using Peddle.Foundation.Common.Extensions;
using Peddle.Foundation.Common.JsonCasing;
using Peddle.Foundation.MediatR;

namespace NetCoreApp.Controllers;

[ApiController]
[Route("api/v1")]
public class ExpensesController : PeddleApiControllerBase
{
        private readonly ILogger<ExpensesController> _logger;

        public ExpensesController(ILogger<ExpensesController> logger, IMapper mapper, IMediator mediator) : base(mediator, logger)
        {
            _logger = logger;
        }
        
        [HttpPost]
        [Route("expenses")]
        public async Task<IActionResult> Create([FromBody] CreateExpenseRequestDto createExpenseRequest)
        {
            var response = await CommandAsync(createExpenseRequest);
            return Created();
        }

        [Route("expenses/search")]
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchExpenseQuery query)
        {
            var response = await QueryAsync(query);
            return Ok(response.SuccessValue);
        }
        
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet("expenses",Name = CommonConstants.ListAllExpensesRouteName)]
        public async Task<IActionResult> GetAll([FromQuery] GetExpensesRequestDto request)
        {
            var getExpensesQuery = new GetExpensesQuery { GetExpensesRequest = request };

            Result<IEnumerable<ExpandoObject>, Exception> expenses = await QueryAsync(getExpensesQuery);
            if (!(expenses.IsSuccess && expenses.SuccessValue.Any())) throw expenses.FailureValue;

           // IDictionary<string, object> paginationData = expenses.SuccessValue?.Last();

            // Response.Headers.Append("Pagination", JsonConvert.SerializeObject(paginationData["Pagination"],
            //     new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } }));

            var result = new
            {
                expenses = expenses.SuccessValue.SkipLast(1)
            };
            return Ok(result);
        }

        private IEnumerable<LinkDto> GetPaginationLinks(IDictionary<string, object> paginationData, GetExpensesRequestDto request)
        {
            var paginatedDictionary = paginationData?["Pagination"].ShapeData()
                .ToDictionary(p => p.Key, x => x.Value);

            return CreatePaginationLinks(request,
                (bool)(paginatedDictionary?["HasNext"] ?? false),
                (bool)(paginatedDictionary?["HasPrevious"] ?? false));
        }

        private IEnumerable<LinkDto> CreatePaginationLinks(GetExpensesRequestDto requestDto, bool hasNext, bool hasPrevious)
        {
            var linkList = new List<LinkDto>();
            if (hasPrevious)
            {
                linkList.Add(GetLinkDto(requestDto with { PageNumber = requestDto.PageNumber - 1 }, "previous-page"));
            }
            if (hasNext)
            {
                var nextPage = (requestDto.PageNumber < CommonConstants.MinPageNumber ? CommonConstants.MinPageNumber : requestDto.PageNumber) + 1;
                var pageSize = requestDto.PageSize < CommonConstants.MinPageSize ? CommonConstants.DefaultPageSize : requestDto.PageSize;
                linkList.Add(GetLinkDto(requestDto with { PageNumber = nextPage, PageSize = pageSize }, "next-page"));
            }

            return linkList.Count > 0 ? linkList : null;
        }

        private LinkDto GetLinkDto(GetExpensesRequestDto expensesRequestDto, string page)
        {
            var jsonSnakeCaseNamingPolicy = new JsonSnakeCaseNamingPolicy();
            var queriesDictionary = expensesRequestDto?.GetType().GetProperties(BindingFlags.IgnoreCase |
                                                                              BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary
                (
                    propInfo => jsonSnakeCaseNamingPolicy.ConvertName(propInfo.Name),
                    propInfo => propInfo.GetValue(expensesRequestDto, null)?.ToString()
                );
            return new LinkDto(
                Url.Link(CommonConstants.ListAllExpensesRouteName, queriesDictionary)
                    ?.Replace("", ""),
                page, "GET");
        }
}