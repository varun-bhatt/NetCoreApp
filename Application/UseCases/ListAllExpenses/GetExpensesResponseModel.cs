namespace NetCoreApp.Application.UseCases.ListAllExpenses;

public class GetExpensesResponseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; }
    public DateTime Date { get; set; }
}