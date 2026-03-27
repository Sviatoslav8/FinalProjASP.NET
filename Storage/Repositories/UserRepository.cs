using Domain.Abstraction;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Storage.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(DataContext context) : base(context)
    {
    }

    public async Task<User> Register(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public Task<User?> GetByEmail(string email)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public Task<bool> IsEmailTaken(string email)
    {
        return _context.Users.AnyAsync(u => u.Email == email);
    }
}