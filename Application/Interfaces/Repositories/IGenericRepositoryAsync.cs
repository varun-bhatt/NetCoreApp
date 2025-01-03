using NetCoreApp.Domain.Entities;

namespace NetCoreApp.Application.Interfaces.Repositories
{
    public interface IGenericRepositoryAsync
    {
        Task<Expense> GetByIdAsync(int id);
        Task<IReadOnlyList<Expense>> GetAllAsync();
        Task<Expense> AddAsync(Expense entity);
        Task UpdateAsync(Expense entity);
        Task DeleteAsync(Expense entity);
    }
}