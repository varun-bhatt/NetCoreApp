using Microsoft.EntityFrameworkCore;
using NetCoreApp.Application.Interfaces.Repositories;
using NetCoreApp.Domain.Entities;
using NetCoreApp.Infrastructure.Persistence;

namespace NetCoreApp.Infrastructure.Repositories
{
    public class GenericRepositoryAsync : IGenericRepositoryAsync
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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