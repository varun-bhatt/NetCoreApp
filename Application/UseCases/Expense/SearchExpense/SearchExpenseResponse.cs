namespace NetCoreApp.Application.UseCases.Expense.SearchExpense;

public class SearchExpenseResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal? Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public long UserId { get; set; }
}