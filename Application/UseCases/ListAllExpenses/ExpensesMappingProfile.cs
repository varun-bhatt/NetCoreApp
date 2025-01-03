using AutoMapper;
using Peddle.Foundation.Common.Pagination;

namespace NetCoreApp.Application.UseCases.ListAllExpenses;

public class ExpensesMappingProfile : Profile
{
    public ExpensesMappingProfile()
    {
        //CreateMap<PagedList<GetExpensesResponseDto>, PaginationMetadata>();
    }
}