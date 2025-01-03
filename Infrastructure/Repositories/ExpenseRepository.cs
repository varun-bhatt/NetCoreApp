using Microsoft.EntityFrameworkCore;
using NetCoreApp.Application.Interfaces.Repositories;
using NetCoreApp.Domain.Entities;
using NetCoreApp.Infrastructure.Persistence;

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
            
            if(isNumber)
            {
                return await _dbContext.Expenses.Where(e =>
                    (double?)e.Amount == number ||
                    e.Id == (int)number
                ).Take(500).ToListAsync();
            }

            var result = await _dbContext.Expenses.Include(x => x.ExpenseCategory).Where(e =>
                e.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                e.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                (e.ExpenseCategory != null && e.ExpenseCategory.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                e.Id.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                e.CreatedAt.ToString().Contains(searchText)
            ).Take(500).ToListAsync();

            return result;
        }

        public  async Task<Expense> GetByIdAsync(int id)
        {
            return await _dbContext.Expenses.FirstOrDefaultAsync(x =>x.Id == id);
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

        public async Task<IReadOnlyList<Expense>> GetAllAsync()
        {
            return await _dbContext
                .Set<Expense>()
                .ToListAsync();
        }
    }
}