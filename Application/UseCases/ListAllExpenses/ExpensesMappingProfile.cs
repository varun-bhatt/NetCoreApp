using AutoMapper;
using Peddle.Foundation.Common.Pagination;

namespace NetCoreApp.Application.UseCases.ListAllExpenses;

public class ExpensesMappingProfile : Profile
{
    public ExpensesMappingProfile()
    {
        // CreateMap<GetExpensesQuery, GetExpensesFilterModel>()
        //     .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.GetExpensesRequest.StartDate))
        //     .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.GetExpensesRequest.EndDate))
        //     .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.GetExpensesRequest.Category))
        //     .ForMember(dest => dest.MinAmount, opt => opt.MapFrom(src => src.GetExpensesRequest.MinAmount))
        //     .ForMember(dest => dest.MaxAmount, opt => opt.MapFrom(src => src.GetExpensesRequest.MaxAmount))
        //     .ForMember(dest => dest.SortBy, opt => opt.MapFrom(src => src.GetExpensesRequest.SortBy))
        //     .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.GetExpensesRequest.SortOrder))
        //     .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.GetExpensesRequest.PageNumber))
        //     .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.GetExpensesRequest.PageSize));
        //
        // CreateMap<GetExpensesResponseModel, GetExpensesResponseDto>()
        //     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
        //     .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
        //     .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
        //     .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
        //     .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));
        //
        //
        // CreateMap<PagedList<GetExpensesResponseModel>, PaginationMetadata>();
    }
}