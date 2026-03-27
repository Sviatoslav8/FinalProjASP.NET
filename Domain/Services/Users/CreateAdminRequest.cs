namespace Domain.Services.Users;

public class CreateAdminRequest
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}