using Domain.Abstraction;
using Domain.Models;
using Domain.Services.Auth;
using Microsoft.Extensions.Logging;

namespace Domain.Services.Users;

public class UsersService
{
    private readonly IUserRepository _repository;
    private readonly JwtTokenGenerator _jwt;
    private readonly ILogger _logger;

    public UsersService(
        IUserRepository repository,
        JwtTokenGenerator jwt,
        ILogger<UsersService> logger)
    {
        _repository = repository;
        _jwt = jwt;
        _logger = logger;
    }

    public async Task<UserResponse> Register(RegisterUserRequest request)
    {
        if (await _repository.IsEmailTaken(request.Email))
        {
            throw new Exception("Email already taken");
        }

        var user = new User()
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        user = await _repository.Register(user);

        var token = _jwt.GenerateToken(user);

        _logger.LogInformation($"User {user.Username} registered successfully");

        return MapToResponse(user, token);
    }

    public async Task<UserResponse> Login(LoginUserRequest request)
    {
        var user = await _repository.GetByEmail(request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new Exception("Invalid credentials");
        }

        var token = _jwt.GenerateToken(user);

        _logger.LogInformation($"User {user.Username} logged in successfully");

        return MapToResponse(user, token);
    }
    public async Task<UserResponse> CreateAdmin(CreateAdminRequest request)
    {
        if (await _repository.IsEmailTaken(request.Email))
            throw new Exception("Email already taken");

        var admin = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash),
            Role = "Admin",
            CreatedAt = DateTime.UtcNow
        };

        admin = await _repository.Register(admin);

        var token = _jwt.GenerateToken(admin);

        _logger.LogInformation($"Admin {admin.Username} created");

        return MapToResponse(admin, token);
    }

    private UserResponse MapToResponse(User user, string token)
    {
        return new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Token = token,
        };
    }
}