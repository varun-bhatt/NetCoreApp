using Microsoft.EntityFrameworkCore;
using NetCoreApp.Application.Interfaces.Repositories;
using NetCoreApp.Application.UseCases.ListAllExpenses;
using NetCoreApp.Domain.Entities;
using NetCoreApp.Infrastructure.Persistence;
using Peddle.Foundation.Common.Pagination;

namespace NetCoreApp.Infrastructure.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ExpenseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Expense>> SearchExpensesAsync(string searchText)
        {
            double number;
            bool isNumber = double.TryParse(searchText, out number);

            if (isNumber)
            {
                return await _dbContext.Expenses.Where(e =>
                    (double?)e.Amount == number ||
                    e.Id == (int)number
                ).Take(500).ToListAsync();
            }

            var result = await _dbContext.Expenses.Include(x => x.ExpenseCategory).Where(e =>
                e.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                e.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                (e.ExpenseCategory != null &&
                 e.ExpenseCategory.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                e.Id.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                e.CreatedAt.ToString().Contains(searchText)
            ).Take(500).ToListAsync();

            return result;
        }

        public async Task<Expense> GetByIdAsync(int id)
        {
            return await _dbContext.Expenses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Expense> AddAsync(Expense entity)
        {
            await _dbContext.Set<Expense>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Expense entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expense entity)
        {
            _dbContext.Set<Expense>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public PagedList<GetExpensesResponseModel> GetAll(GetExpensesFilterModel getExpensesFilterModel)
        {
            IQueryable<GetExpensesResponseModel> expensesQuery =
                (from expense in _dbContext.Expenses
                    join category in _dbContext.ExpenseCategories on expense.ExpenseCategoryId equals category.Id
                    join user in _dbContext.Users on expense.UserId equals user.Id
                    select new GetExpensesResponseModel
                    {
                        Name = expense.Name,
                        Description = expense.Description,
                        Amount = expense.Amount ?? 0,
                        Category = category.Name,
                        Date = expense.CreatedAt
                    }).AsQueryable();

            // Apply filters
            if (getExpensesFilterModel.StartDate.HasValue)
            {
                expensesQuery = expensesQuery.Where(e => e.Date >= getExpensesFilterModel.StartDate.Value);
            }

            if (getExpensesFilterModel.EndDate.HasValue)
            {
                expensesQuery = expensesQuery.Where(e => e.Date <= getExpensesFilterModel.EndDate.Value);
            }

            if (!string.IsNullOrEmpty(getExpensesFilterModel.Category))
            {
                expensesQuery = expensesQuery.Where(e => e.Category == getExpensesFilterModel.Category);
            }

            if (getExpensesFilterModel.MinAmount.HasValue)
            {
                expensesQuery = expensesQuery.Where(e => e.Amount >= getExpensesFilterModel.MinAmount.Value);
            }

            if (getExpensesFilterModel.MaxAmount.HasValue)
            {
                expensesQuery = expensesQuery.Where(e => e.Amount <= getExpensesFilterModel.MaxAmount.Value);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(getExpensesFilterModel.SortBy))
            {
                expensesQuery = getExpensesFilterModel.SortOrder.ToLower() == "desc"
                    ? expensesQuery.OrderByDescending(e => EF.Property<object>(e, getExpensesFilterModel.SortBy))
                    : expensesQuery.OrderBy(e => EF.Property<object>(e, getExpensesFilterModel.SortBy));
            }

            // Apply pagination
            var pagedExpenses =
                PagedList<GetExpensesResponseModel>.Create(expensesQuery, getExpensesFilterModel.PageNumber, getExpensesFilterModel.PageSize);

            return pagedExpenses;
        }
    }
}