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
    
    public async Task<RegisterResponse> RegisterAsync  (RegisterRequest request)
    {
        _logger.LogInformation("---------------------------------------------------------------------------------------------------");
        _logger.LogInformation("Register user Flow Triggered");
        // Validate Unique and valid email
        if (request.Email == "")
        {
            _logger.LogInformation("Email Property cannot be empty");
            throw new Exception("Email Property cannot be empty");
        }
        // trim email
        request.Email = request.Email.Trim();
        var users = _context.Users.Where(u => u.Email == request.Email).FirstOrDefault();
        if (users == null)
        {
            _logger.LogInformation("Users not found");
            throw new Exception("Users not found");
        }

        return new RegisterResponse
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            CreatedAt = DateTime.UtcNow
        };
    }
}