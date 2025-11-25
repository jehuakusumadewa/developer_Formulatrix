using GameAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private static Dictionary<string, GameState> _games = new();
    
    [HttpPost("start")]
    public IActionResult StartGame([FromBody] string playerName)
    {
        var gameId = Guid.NewGuid().ToString();
        
        var gameState = new GameState
        {
            GameId = gameId,
            Player = new Player { Name = playerName }
        };
        
        _games[gameId] = gameState;
        return Ok(gameState);
    }
    
    [HttpGet("{gameId}")]
    public IActionResult GetGame(string gameId)
    {
        if (_games.ContainsKey(gameId))
            return Ok(_games[gameId]);
        return NotFound("Game not found");
    }
    
    [HttpPost("{gameId}/move")]
    public IActionResult MovePlayer(string gameId, [FromBody] MoveRequest move)
    {
        if (!_games.ContainsKey(gameId))
            return NotFound("Game not found");
            
        var game = _games[gameId];
        
        // Update player position
        game.Player.PositionX += move.DeltaX;
        game.Player.PositionY += move.DeltaY;
        
        // Simple game logic: increase score when moving
        game.Score += 5;
        
        // Simple bounds checking
        if (game.Player.PositionX < 0) game.Player.PositionX = 0;
        if (game.Player.PositionY < 0) game.Player.PositionY = 0;
        if (game.Player.PositionX > 10) game.Player.PositionX = 10;
        if (game.Player.PositionY > 10) game.Player.PositionY = 10;
        
        return Ok(game);
    }
    
    [HttpPost("{gameId}/attack")]
    public IActionResult Attack(string gameId)
    {
        if (!_games.ContainsKey(gameId))
            return NotFound("Game not found");
            
        var game = _games[gameId];
        game.Score += 20;
        
        return Ok(new { 
            message = "Player attacked!", 
            score = game.Score 
        });
    }
}