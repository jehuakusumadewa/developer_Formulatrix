using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs.Requests;
using TodoApi.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                _logger.LogInformation("Registration attempt for email: {Email}", request.Email);
                
                var result = await _authService.RegisterAsync(request);
                
                if (!result.Success)
                {
                    _logger.LogWarning("Registration failed for email {Email}: {Error}", 
                        request.Email, result.Error);
                    return BadRequest(new { error = result.Error });
                }
                
                _logger.LogInformation("User registered successfully with ID: {UserId}", 
                    result.Value.Id);
                
                return Ok(new
                {
                    message = "Registration successful",
                    user = result.Value
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration for email: {Email}", 
                    request.Email);
                return StatusCode(500, new { 
                    error = "An unexpected error occurred during registration",
                    details = ex.Message 
                });
            }
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Login attempt for email: {Email}", request.Email);
                
                var result = await _authService.LoginAsync(request);
                
                if (!result.Success)
                {
                    _logger.LogWarning("Login failed for email {Email}: {Error}", 
                        request.Email, result.Error);
                    return Unauthorized(new { error = result.Error });
                }
                
                _logger.LogInformation("User logged in successfully: {Email}", request.Email);
                
                return Ok(new 
                { 
                    message = "Login successful",
                    token = result.Value 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login for email: {Email}", 
                    request.Email);
                return StatusCode(500, new { 
                    error = "An unexpected error occurred during login",
                    details = ex.Message 
                });
            }
        }
    }
}