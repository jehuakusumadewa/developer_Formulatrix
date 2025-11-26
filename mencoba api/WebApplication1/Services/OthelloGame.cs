using WebApplication1.Models;

namespace WebApplication1.Services
{
    // INTERFACE BOARD (Dari console)
    public interface IBoard
    {
        ICell[,] Squares { get; set; }
    }

    public interface ICell
    {
        Position Position { get; set; }
        IDisk Disk { get; set; }
    }

    public interface IDisk
    {
        Position Position { get; set; }
        DiskColor Color { get; set; }
    }

    // IMPLEMENTASI
    public class OthelloBoard : IBoard
    {
        public ICell[,] Squares { get; set; } = new ICell[8, 8];
    }

    public class Cell : ICell
    {
        public Position Position { get; set; } = new Position(0, 0);
        public IDisk Disk { get; set; }
    }

    public class OthelloDisk : IDisk
    {
        public Position Position { get; set; } = new Position(0, 0);
        public DiskColor Color { get; set; }
    }

    // GAME LOGIC UTAMA
    public class OthelloGame
    {
        private IBoard _board;
        private List<IPlayer> _players;
        private IPlayer _currentPlayer;
        private StatusType _status;
        private int _size;
        private readonly List<Position> _directions;

        // DELEGATES
        public Action<IPlayer> OnTurnChanged { get; set; }
        public Action<string> OnMessage { get; set; }

        public OthelloGame(List<IPlayer> players, IBoard board, int size, StatusType status, IPlayer currentPlayer, List<Position> directions)
        {
            _board = board;
            _size = size;
            _status = status;
            _currentPlayer = currentPlayer;
            _players = players;
            _directions = directions;

            // Default actions
            OnTurnChanged = (player) => Console.WriteLine($"🎮 Giliran {player.Name} ({player.Color})");
            OnMessage = (msg) => Console.WriteLine($"💡 {msg}");
        }

        private void InitializeBoard()
        {
            _board.Squares = new ICell[_size, _size];
            
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _board.Squares[i, j] = new Cell
                    {
                        Position = new Position(i, j)
                    };
                }
            }
        }

        public void StartGame()
        {
            _status = StatusType.Play;
            InitializeBoard();
            InitializeBoardDisks();
            
            Console.WriteLine("🎯 Othello Game Started!");
            OnTurnChanged?.Invoke(_currentPlayer);
        }

        public bool IsGameActive()
        {
            return _status == StatusType.Play;
        }

        public IPlayer GetCurrentPlayer()
        {
            return _currentPlayer;
        }

        public IPlayer GetPlayerByColor(DiskColor color)
        {
            return _players.Find(p => p.Color == color);
        }

        public int GetPlayerScore(IPlayer player)
        {
            int count = 0;
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    var disk = _board.Squares[i, j].Disk;
                    if (disk != null && disk.Color == player.Color)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public IPlayer GetWinner()
        {
            if (_status != StatusType.Win) return null;

            int blackScore = GetPlayerScore(GetPlayerByColor(DiskColor.Black));
            int whiteScore = GetPlayerScore(GetPlayerByColor(DiskColor.White));

            if (blackScore > whiteScore)
                return GetPlayerByColor(DiskColor.Black);
            else if (whiteScore > blackScore)
                return GetPlayerByColor(DiskColor.White);
            else
                return null;
        }

        private void InitializeBoardDisks()
        {
            PlaceDisk(new Position(3, 3), new OthelloDisk { Color = DiskColor.White });
            PlaceDisk(new Position(3, 4), new OthelloDisk { Color = DiskColor.Black });
            PlaceDisk(new Position(4, 3), new OthelloDisk { Color = DiskColor.Black });
            PlaceDisk(new Position(4, 4), new OthelloDisk { Color = DiskColor.White });
        }

        private void PlaceDisk(Position position, IDisk disk)
        {
            disk.Position = position;
            _board.Squares[position.X, position.Y].Disk = disk;
        }

        // ... (TAMBAHKAN METHOD LAINNYA YANG DIPERLUKAN)
        // Untuk demo, kita skip dulu method-method game logic yang lengkap
    }
}