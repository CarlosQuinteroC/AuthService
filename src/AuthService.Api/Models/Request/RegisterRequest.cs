namespace AuthService.Api.Models.Request;
/// <summary>
///  Defines the request model for registering a new user
/// </summary>
public class RegisterRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? PhoneNumber { get; set; }
}