using System.Dynamic;
using MediatR;
using Peddle.Foundation.Common.Pagination;

namespace NetCoreApp.Application.UseCases.ListAllExpenses;

public record GetExpensesRequestDto : PaginationQueryStringParameters
{
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Category { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public string SortBy { get; set; }
    public string SortOrder { get; set; } = "asc";
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int? UserId { get; set; }
}