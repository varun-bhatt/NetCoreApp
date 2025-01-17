﻿using Microsoft.EntityFrameworkCore;
using NetCoreApp.Application.Interfaces.Repositories;
using NetCoreApp.Domain.Entities;
using NetCoreApp.Infrastructure.Persistence;

namespace NetCoreApp.Infrastructure.Repositories;

// UserRepository implementation
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddUser(User user)
    {
        _context.Set<User>().Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
    }
}