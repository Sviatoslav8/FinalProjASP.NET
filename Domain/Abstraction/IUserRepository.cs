using Domain.Models;

namespace Domain.Abstraction;

public interface IUserRepository
{
    Task<User> Register(User user);
    Task<User?> GetByEmail(string email);
    Task<bool> IsEmailTaken(string email);
}