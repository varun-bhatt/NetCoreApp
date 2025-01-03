using NetCoreApp.Domain.Entities;

namespace NetCoreApp.Application.Interfaces.Repositories
{
    public interface IGenericRepositoryAsync
    {
        Task<Person> GetByIdAsync(int id);
        Task<IReadOnlyList<Person>> GetAllAsync();
        Task<Person> AddAsync(Person entity);
        Task UpdateAsync(Person entity);
        Task DeleteAsync(Person entity);
    }
}