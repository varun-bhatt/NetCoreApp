namespace NetCoreApp.Application.UseCases.ListAllExpenses
{
    public class GetExpensesFilterModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Category { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string Status { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; } = "asc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? UserId { get; set; }
    }
}