using AuthService.Api.Models.Request;
using AuthService.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    
    private readonly ILogger<AuthController> _logger;
    private readonly AuthServiceManager _authServiceManager;
    
    public AuthController(
        ILogger<AuthController> logger,
        AuthServiceManager authServiceManager
        )
    {
        _logger = logger;
        _authServiceManager = authServiceManager;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        _logger.LogInformation("Register endpoint called");
            var response = await _authServiceManager.RegisterAsync(request);
            return StatusCode(StatusCodes.Status201Created, response);
    }
}