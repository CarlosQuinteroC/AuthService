using AuthService.Api.Data;
using AuthService.Api.Entities;
using AuthService.Api.Models.Request;
using AuthService.Api.Models.Response;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Api.Services;

public class AuthServiceManager
{
    private readonly AuthDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly ILogger<AuthServiceManager> _logger;
    
    
    public AuthServiceManager
        (
            AuthDbContext context, 
            PasswordHasher<User> passwordHasher, 
            ILogger<AuthServiceManager> logger
            )
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }
    
    public async Task<RegisterResponse> RegisterAsync  (RegisterRequest)
    {
        return new RegisterResponse
        {
            Id = Guid.NewGuid(),
            FirstName = RegisterRequest.FirstName,
            LastName = RegisterRequest.LastName,
            Email = RegisterRequest.Email,
            PhoneNumber = RegisterRequest.PhoneNumber,
            CreatedAt = DateTime.UtcNow
        }
    }
}