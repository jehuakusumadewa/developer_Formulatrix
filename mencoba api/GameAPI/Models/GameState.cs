namespace GameAPI.Models;

public class GameState
{
    public string GameId { get; set; } = string.Empty;
    public string Status { get; set; } = "Playing";
    public int Score { get; set; }
    public Player Player { get; set; } = new Player();
}

public class Player
{
    public string Name { get; set; } = "Player";
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public int Health { get; set; } = 100;
}

public class MoveRequest
{
    public int DeltaX { get; set; }
    public int DeltaY { get; set; }
}