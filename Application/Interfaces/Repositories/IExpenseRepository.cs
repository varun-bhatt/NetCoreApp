using NetCoreApp.Application.UseCases.ListAllExpenses;
using NetCoreApp.Domain.Entities;
using Peddle.Foundation.Common.Pagination;

namespace NetCoreApp.Application.Interfaces.Repositories
{
    public interface IExpenseRepository
    {
        Task<Expense> GetByIdAsync(int id);
        PagedList<GetExpensesResponseModel> GetAll(GetExpensesFilterModel getExpensesFilterModel);
        Task<Expense> AddAsync(Expense entity);
        Task UpdateAsync(Expense entity);
        Task DeleteAsync(Expense entity);
        Task<List<Expense>> SearchExpensesAsync(string searchText);
    }
}