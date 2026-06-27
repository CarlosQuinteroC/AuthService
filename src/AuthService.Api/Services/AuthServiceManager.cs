using AuthService.Api.Data;
using AuthService.Api.Entities;
using AuthService.Api.Models.Request;
using AuthService.Api.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        // Validate request properties
        if (string.IsNullOrWhiteSpace(request.FirstName)
            || string.IsNullOrWhiteSpace(request.LastName) 
            || string.IsNullOrWhiteSpace(request.Email)
            || string.IsNullOrWhiteSpace(request.Password)
            )
        {
            _logger.LogInformation("One of the required properties is missing");
            throw new Exception("One of the required properties is missing");
        }
        // trim email
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var userEmailExists =  await _context.Users.Where(u => u.Email == normalizedEmail).AnyAsync();
        if (userEmailExists)
        {
            _logger.LogInformation("User with email {email} already exist", normalizedEmail);
            throw new Exception("User with email already exist");
        }
        
        // Create user in memory
        var user = new User
        {
            Id = Guid.CreateVersion7(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = normalizedEmail,
            PasswordHash = "HashedPassword",
            PhoneNumber = !string.IsNullOrWhiteSpace(request.PhoneNumber) ? request.PhoneNumber : null,
        };
        
        // Hash the password
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
        
        _context.Users.Add(user);
        
        // Get roles and find user role
        var userRole  = await _context.Roles.Where(r => r.Name == "user").FirstOrDefaultAsync();
        if (userRole  == null)
        {
            _logger.LogInformation("Role with Name User does not exist, creating role");
            userRole = new Role
            {
                Id = Guid.CreateVersion7(),
                Name = "user"
            };
            _context.Roles.Add(userRole);
            _logger.LogInformation("Role {RoleName} created",  userRole.Name);
        }
        
        // assign role to user
        var userRoleAssignment = new UserRole
        {
            UserId = user.Id,
            RoleId = userRole.Id
        };
        
        _context.UserRoles.Add(userRoleAssignment);
        
        _logger.LogInformation("Creating user with name {FirstName} {LastName} and email {Email}", user.FirstName, user.LastName, user.Email);
        
        // Save in database
        await _context.SaveChangesAsync();

        return new RegisterResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt
        };

    }
}