using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs.Requests;
using TodoApi.Services.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TodoApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly ILogger<TodosController> _logger;
        
        public TodosController(
            ITodoService todoService,
            ILogger<TodosController> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "User ID not found in token" });
                }
                
                _logger.LogInformation("Getting all todos for user {UserId}", userId);
                
                var result = await _todoService.GetUserTodosAsync(userId);
                
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to get todos for user {UserId}: {Error}", 
                        userId, result.Error);
                    return BadRequest(new { error = result.Error });
                }
                
                return Ok(new
                {
                    message = "Todos retrieved successfully", 
                    todos = result.Value
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting todos");
                return StatusCode(500, new { 
                    error = "An unexpected error occurred while fetching todos",
                    details = ex.Message 
                });
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "User ID not found in token" });
                }
                
                _logger.LogInformation("Getting todo {TodoId} for user {UserId}", id, userId);
                
                var result = await _todoService.GetTodoByIdAsync(id, userId);
                
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to get todo {TodoId} for user {UserId}: {Error}", 
                        id, userId, result.Error);
                    return NotFound(new { error = result.Error });
                }
                
                _logger.LogInformation("Retrieved todo {TodoId} for user {UserId}", id, userId);
                
                return Ok(new
                {
                    message = "Todo retrieved successfully",
                    todo = result.Value
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting todo {TodoId}", id);
                return StatusCode(500, new { 
                    error = "An unexpected error occurred while fetching todo",
                    details = ex.Message 
                });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TodoRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "User ID not found in token" });
                }
                
                _logger.LogInformation("Creating todo for user {UserId}: {Title}", 
                    userId, request.Title);
                
                var result = await _todoService.CreateTodoAsync(request, userId);
                
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to create todo for user {UserId}: {Error}", 
                        userId, result.Error);
                    return BadRequest(new { error = result.Error });
                }
                
                _logger.LogInformation("Todo created successfully with ID {TodoId}", result.Value.Id);
                
                return CreatedAtAction(nameof(GetById), 
                    new { id = result.Value.Id }, 
                    new 
                    { 
                        message = "Todo created successfully",
                        todo = result.Value 
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating todo");
                return StatusCode(500, new { 
                    error = "An unexpected error occurred while creating todo",
                    details = ex.Message 
                });
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TodoRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "User ID not found in token" });
                }
                
                _logger.LogInformation("Updating todo {TodoId} for user {UserId}", id, userId);
                
                var result = await _todoService.UpdateTodoAsync(id, request, userId);
                
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to update todo {TodoId} for user {UserId}: {Error}", 
                        id, userId, result.Error);
                    return BadRequest(new { error = result.Error });
                }
                
                _logger.LogInformation("Todo {TodoId} updated successfully for user {UserId}", 
                    id, userId);
                
                return Ok(new
                {
                    message = "Todo updated successfully",
                    todo = result.Value
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating todo {TodoId}", id);
                return StatusCode(500, new { 
                    error = "An unexpected error occurred while updating todo",
                    details = ex.Message 
                });
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "User ID not found in token" });
                }
                
                _logger.LogInformation("Deleting todo {TodoId} for user {UserId}", id, userId);
                
                var result = await _todoService.DeleteTodoAsync(id, userId);
                
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to delete todo {TodoId} for user {UserId}: {Error}", 
                        id, userId, result.Error);
                    return BadRequest(new { error = result.Error });
                }
                
                _logger.LogInformation("Todo {TodoId} deleted successfully for user {UserId}", 
                    id, userId);
                
                return Ok(new
                {
                    message = "Todo deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting todo {TodoId}", id);
                return StatusCode(500, new { 
                    error = "An unexpected error occurred while deleting todo",
                    details = ex.Message 
                });
            }
        }
        
        [HttpPatch("{id}/mark-complete")]
        public async Task<IActionResult> MarkCompletion(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "User ID not found in token" });
                }
                
                _logger.LogInformation("Marking todo {TodoId} as complete for user {UserId}", 
                    id, userId);
                
                var result = await _todoService.MarkCompletionAsync(id, userId);
                
                if (!result.Success)
                {
                    _logger.LogWarning(
                        "Failed to mark todo {TodoId} as complete for user {UserId}: {Error}", 
                        id, userId, result.Error);
                    return BadRequest(new { error = result.Error });
                }
                
                _logger.LogInformation(
                    "Todo {TodoId} marked as complete successfully for user {UserId}", 
                    id, userId);
                
                return Ok(new
                {
                    message = "Todo marked as complete successfully",
                    todo = result.Value
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error occurred while marking todo {TodoId} as complete", 
                    id);
                return StatusCode(500, new { 
                    error = "An unexpected error occurred while marking todo as complete",
                    details = ex.Message 
                });
            }
        }
    }
}