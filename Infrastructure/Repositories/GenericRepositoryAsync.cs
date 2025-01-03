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

        public  async Task<Person> GetByIdAsync(int id)
        {
            return await _dbContext.Persons.FirstOrDefaultAsync(x =>x.PersonId == id);
        }
        public async Task<Person> AddAsync(Person entity)
        {
            await _dbContext.Set<Person>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Person entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Person entity)
        {
            _dbContext.Set<Person>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Person>> GetAllAsync()
        {
            return await _dbContext
                .Set<Person>()
                .ToListAsync();
        }
    }
}