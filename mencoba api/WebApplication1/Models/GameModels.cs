namespace WebApplication1.Models
{
    // POSITION (Untuk arah dan koordinat)
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    // WARNA DISK
    public enum DiskColor
    {
        Black,
        White
    }

    // STATUS GAME
    public enum StatusType
    {
        NotStart,
        Play,
        Win,
        Draw
    }

    // PLAYER
    public class Player : IPlayer
    {
        public string Name { get; set; } = string.Empty;
        public DiskColor Color { get; set; }
    }

    // INTERFACE UNTUK PLAYER (Dari console project)
    public interface IPlayer
    {
        string Name { get; set; }
        DiskColor Color { get; set; }
    }
}