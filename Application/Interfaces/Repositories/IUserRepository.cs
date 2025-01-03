using NetCoreApp.Domain.Entities;

namespace NetCoreApp.Application.Interfaces.Repositories;

// IUserRepository interface
public interface IUserRepository
{
    Task AddUser(User user);
    Task<User> GetUserByEmail(string email);
}