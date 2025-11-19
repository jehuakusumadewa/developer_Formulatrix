using System;
using System.Collections.Generic;
using System.Linq;

// Enumerations
public enum PieceType { Pawn, King }
public enum PieceColor { Red, Black }
public enum StatusType { NotStart, Play, Win, Draw }

// Structs
public struct Position
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public override bool Equals(object obj)
    {
        if (obj is Position other)
            return X == other.X && Y == other.Y;
        return false;
    }
    
    public override int GetHashCode() => HashCode.Combine(X, Y);
    
    public static bool operator ==(Position left, Position right) => left.Equals(right);
    public static bool operator !=(Position left, Position right) => !left.Equals(right);
    
    public override string ToString() => $"({X},{Y})";
}

// Interfaces
public interface IBoard
{
    ICell[,] Squares { get; }
}

public interface IPiece
{
    PieceColor Color { get; }
    Position Position { get; set; }
    PieceType TypePiece { get; set; }
}

public interface ICell
{
    Position Position { get; }
    IPiece Piece { get; set; }
}

public interface IPlayer
{
    string Name { get; }
    PieceColor Color { get; }
}

// Implementations
public class Cell : ICell
{
    public Position Position { get; }
    public IPiece Piece { get; set; }
    
    public Cell(int x, int y, IPiece piece = null)
    {
        Position = new Position(x, y);
        Piece = piece;
    }
}

public class Player : IPlayer
{
    public string Name { get; }
    public PieceColor Color { get; }
    
    public Player(string name, PieceColor color)
    {
        Name = name;
        Color = color;
    }
    
    public override string ToString() => $"{Name} ({Color})";
}

public class CheckersPiece : IPiece
{
    public PieceColor Color { get; }
    public Position Position { get; set; }
    public PieceType TypePiece { get; set; }
    
    public CheckersPiece(PieceColor color, Position position, PieceType type = PieceType.Pawn)
    {
        Color = color;
        Position = position;
        TypePiece = type;
    }
    
    public override string ToString() => $"{Color} {TypePiece} at {Position}";
}

public class CheckersBoard : IBoard
{
    public ICell[,] Squares { get; private set; }
    private const int BoardSize = 8;
    
    public CheckersBoard()
    {
        Squares = new ICell[BoardSize, BoardSize];
        InitializeBoard();
    }
    
    private void InitializeBoard()
    {
        for (int x = 0; x < BoardSize; x++)
        {
            for (int y = 0; y < BoardSize; y++)
            {
                Squares[x, y] = new Cell(x, y);
            }
        }
    }
    
    public IPiece GetPieceAt(Position position)
    {
        if (!IsValidPosition(position))
            return null;
            
        return Squares[position.X, position.Y].Piece;
    }
    
    public void SetPieceAt(Position position, IPiece piece)
    {
        if (IsValidPosition(position))
        {
            Squares[position.X, position.Y].Piece = piece;
            if (piece != null)
                piece.Position = position;
        }
    }
    
    public void RemovePieceAt(Position position)
    {
        if (IsValidPosition(position))
        {
            Squares[position.X, position.Y].Piece = null;
        }
    }
    
    public bool IsValidPosition(Position position)
    {
        return position.X >= 0 && position.X < BoardSize && 
               position.Y >= 0 && position.Y < BoardSize;
    }
    
    public bool IsDarkSquare(Position position)
    {
        return (position.X + position.Y) % 2 == 1;
    }
}

public class CheckersGame
{
    private CheckersBoard _board;
    private List<IPiece> _listPiece;
    private Dictionary<IPlayer, List<IPiece>> _playerData;
    private List<IPlayer> _players;
    private IPlayer _currentPlayer;
    private Position _activePieceInJump;
    private bool _isInMultipleJump;
    private StatusType _status;
    private int _movesWithoutCapture;
    private int _movesWithoutKing;
    private const int _maxMovesWithoutProgress = 50;
    
    public CheckersGame(List<IPlayer> _players, IBoard board, StatusType status, int movesWithoutCapture, int movesWithoutKing, int movesWithoutProgress, bool isInMultipleJump, Position _activePieceInJump, Dictionary<IPlayer, List<IPiece>> _playerData)
    {
        _board = new CheckersBoard();
        _listPiece = new List<IPiece>();
        _playerData = new Dictionary<IPlayer, List<IPiece>>();
        _players = new List<IPlayer> { player1, player2 };
        _status = StatusType.NotStart;
        _movesWithoutCapture = 0;
        _movesWithoutKing = 0;
        _isInMultipleJump = false;
        _activePieceInJump = new Position(-1, -1);
        
        _playerData[player1] = new List<IPiece>();
        _playerData[player2] = new List<IPiece>();
    }
    
    public void StartGame()
    {
        if (_status == StatusType.NotStart)
        {
            InitializeBoardCells();
            _currentPlayer = _players[0]; // Black starts first
            _status = StatusType.Play;
            Console.WriteLine("Game started! Black goes first.");
        }
    }
    
    private void InitializeBoardCells()
    {
        // Clear existing pieces
        _listPiece.Clear();
        foreach (var player in _players)
        {
            _playerData[player].Clear();
        }
        
        // Initialize black pieces (top three rows)
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                if (((row + col) % 2) == 1)
                {
                    var position = new Position(row, col);
                    var piece = new CheckersPiece(PieceColor.Black, position);
                    _board.SetPieceAt(position, piece);
                    _listPiece.Add(piece);
                    AddPieceToPlayerData(_players.First(p => p.Color == PieceColor.Black), piece);
                }
            }
        }
        
        // Initialize red pieces (bottom three rows)
        for (int row = 5; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                if (((row + col) % 2) == 1)
                {
                    var position = new Position(row, col);
                    var piece = new CheckersPiece(PieceColor.Red, position);
                    _board.SetPieceAt(position, piece);
                    _listPiece.Add(piece);
                    AddPieceToPlayerData(_players.First(p => p.Color == PieceColor.Red), piece);
                }
            }
        }
    }
    
    private void AddPieceToPlayerData(IPlayer player, IPiece piece)
    {
        if (_playerData.ContainsKey(player))
        {
            _playerData[player].Add(piece);
        }
    }
    
    public IPlayer GetCurrentPlayer()
    {
        return _currentPlayer;
    }
    
    private List<Tuple<Position, Position>> GetPossibleMovesForPiece(IPiece piece, bool requireCaptureOnly = false)
    {
        var moves = new List<Tuple<Position, Position>>();
        var captures = new List<Tuple<Position, Position>>();
        
        if (piece == null) return moves;
        
        int direction = (piece.Color == PieceColor.Black) ? 1 : -1;
        
        // Check normal moves and captures
        CheckDirections(piece, direction, moves, captures);
        
        // For kings, also check opposite direction
        if (piece.TypePiece == PieceType.King)
        {
            CheckDirections(piece, -direction, moves, captures);
        }
        
        // If we're in multiple jump mode, only allow captures with the active piece
        if (_isInMultipleJump)
        {
            if (!piece.Position.Equals(_activePieceInJump))
                return new List<Tuple<Position, Position>>();
                
            return captures.Where(move => move.Item1.Equals(_activePieceInJump)).ToList();
        }
        
        // If captures exist and requireCaptureOnly is true, return only captures
        if (requireCaptureOnly && captures.Count > 0)
            return captures;
        
        // If no captures exist, return normal moves
        if (captures.Count == 0)
            return moves;
        
        return captures;
    }
    
    private void CheckDirections(IPiece piece, int direction, List<Tuple<Position, Position>> moves, List<Tuple<Position, Position>> captures)
    {
        int[] colOffsets = { -1, 1 };
        
        foreach (int colOffset in colOffsets)
        {
            Position normalMove = new Position(piece.Position.X + direction, piece.Position.Y + colOffset);
            Position captureMove = new Position(piece.Position.X + 2 * direction, piece.Position.Y + 2 * colOffset);
            Position capturedPos = new Position(piece.Position.X + direction, piece.Position.Y + colOffset);
            
            // Check normal move
            if (_board.IsValidPosition(normalMove) && 
                _board.GetPieceAt(normalMove) == null &&
                _board.IsDarkSquare(normalMove))
            {
                moves.Add(Tuple.Create(piece.Position, normalMove));
            }
            
            // Check capture move
            if (_board.IsValidPosition(captureMove) && 
                _board.GetPieceAt(captureMove) == null &&
                _board.IsValidPosition(capturedPos))
            {
                IPiece capturedPiece = _board.GetPieceAt(capturedPos);
                if (capturedPiece != null && capturedPiece.Color != piece.Color &&
                    _board.IsDarkSquare(captureMove))
                {
                    captures.Add(Tuple.Create(piece.Position, captureMove));
                }
            }
        }
    }
    
    private bool CanMakeaMove(IPlayer player)
    {
        bool requireCaptureOnly = AnyCaptureExists(player);
        
        foreach (var piece in _playerData[player])
        {
            var moves = GetPossibleMovesForPiece(piece, requireCaptureOnly);
            if (moves.Count > 0)
                return true;
        }
        return false;
    }
    
    private bool IsValidMove(Position from, Position to, out bool isCapture, out Position capturedPos)
    {
        isCapture = false;
        capturedPos = new Position(-1, -1);
        
        IPiece piece = _board.GetPieceAt(from);
        if (piece == null || piece.Color != _currentPlayer.Color)
            return false;
        
        // If we're in multiple jump, only the active piece can move
        if (_isInMultipleJump && !from.Equals(_activePieceInJump))
            return false;
        
        bool requireCaptureOnly = AnyCaptureExists(_currentPlayer);
        var possibleMoves = GetPossibleMovesForPiece(piece, requireCaptureOnly);
        
        foreach (var move in possibleMoves)
        {
            if (move.Item1.Equals(from) && move.Item2.Equals(to))
            {
                // Check if this is a capture move
                int rowDiff = Math.Abs(to.X - from.X);
                if (rowDiff == 2)
                {
                    isCapture = true;
                    capturedPos = new Position((from.X + to.X) / 2, (from.Y + to.Y) / 2);
                }
                return true;
            }
        }
        
        return false;
    }
    
    public bool MovePiece(Position from, Position to)
    {
        if (_status != StatusType.Play) 
        {
            Console.WriteLine("Game is not in play state!");
            return false;
        }
        
        bool isCapture;
        Position capturedPos;
        
        if (!IsValidMove(from, to, out isCapture, out capturedPos))
        {
            Console.WriteLine("Invalid move!");
            return false;
        }
        
        IPiece piece = _board.GetPieceAt(from);
        
        // Move the piece
        _board.SetPieceAt(to, piece);
        _board.RemovePieceAt(from);
        
        // Handle capture
        if (isCapture)
        {
            HandleCapturedPiece(capturedPos);
            Console.WriteLine($"Captured piece at {capturedPos}!");
        }
        
        // Check for promotion
        bool wasKingPromoted = piece.TypePiece == PieceType.Pawn;
        CheckAndPromote(piece);
        wasKingPromoted = wasKingPromoted && piece.TypePiece == PieceType.King;
        
        // Update draw counters
        UpdateDrawCounters(isCapture, wasKingPromoted);
        
        // Check for multiple jump
        if (isCapture && CheckMultipleJump(to))
        {
            _isInMultipleJump = true;
            _activePieceInJump = to;
            Console.WriteLine("Multiple jump available! Continue jumping with the same piece.");
        }
        else
        {
            _isInMultipleJump = false;
            _activePieceInJump = new Position(-1, -1);
            SwitchPlayer();
        }
        
        // Check game end conditions
        CheckGameEnd();
        
        return true;
    }
    
    private bool AnyCaptureExists(IPlayer player)
    {
        foreach (var piece in _playerData[player])
        {
            var moves = GetPossibleMovesForPiece(piece, false);
            foreach (var move in moves)
            {
                int rowDiff = Math.Abs(move.Item2.X - move.Item1.X);
                if (rowDiff == 2) // This is a capture move
                    return true;
            }
        }
        return false;
    }
    
    private void HandleCapturedPiece(Position capturedPos)
    {
        IPiece capturedPiece = _board.GetPieceAt(capturedPos);
        if (capturedPiece != null)
        {
            _board.RemovePieceAt(capturedPos);
            _listPiece.Remove(capturedPiece);
            
            var owner = _players.First(p => p.Color == capturedPiece.Color);
            _playerData[owner].Remove(capturedPiece);
        }
    }
    
    private bool CheckMultipleJump(Position currentPosition)
    {
        IPiece piece = _board.GetPieceAt(currentPosition);
        if (piece != null)
        {
            var moves = GetPossibleMovesForPiece(piece, true);
            return moves.Any(move => Math.Abs(move.Item2.X - move.Item1.X) == 2); // Capture moves only
        }
        return false;
    }
    
    private void CheckAndPromote(IPiece piece)
    {
        if (piece.TypePiece == PieceType.Pawn)
        {
            if ((piece.Color == PieceColor.Black && piece.Position.X == 7) ||
                (piece.Color == PieceColor.Red && piece.Position.X == 0))
            {
                piece.TypePiece = PieceType.King;
                Console.WriteLine($"Piece at ({piece.Position.X}, {piece.Position.Y}) promoted to King!");
            }
        }
    }
    
    public bool IsDraw()
    {
        return _movesWithoutCapture >= _maxMovesWithoutProgress || 
               _movesWithoutKing >= _maxMovesWithoutProgress;
    }
    
    private void UpdateDrawCounters(bool wasCapture, bool wasKingPromoted)
    {
        if (wasCapture)
        {
            _movesWithoutCapture = 0;
        }
        else
        {
            _movesWithoutCapture++;
        }
        
        if (wasKingPromoted)
        {
            _movesWithoutKing = 0;
        }
        else if (!wasCapture)
        {
            _movesWithoutKing++;
        }
    }
    
    private void SwitchPlayer()
    {
        // Find the next player
        int currentIndex = _players.IndexOf(_currentPlayer);
        int nextIndex = (currentIndex + 1) % _players.Count;
        _currentPlayer = _players[nextIndex];
        
        Console.WriteLine($"Now it's {_currentPlayer.Name}'s turn ({_currentPlayer.Color})");
    }
    
    public bool GetPlayerHasPiecesLeft(IPlayer player)
    {
        return _playerData.ContainsKey(player) && _playerData[player].Count > 0;
    }
    
    public IPlayer GetWinner()
    {
        if (_status != StatusType.Win) 
            return null;
        
        // Find the player who still has pieces and can move
        foreach (var player in _players)
        {
            if (GetPlayerHasPiecesLeft(player) && CanMakeaMove(player))
                return player;
        }
        return null;
    }
    
    private void CheckGameEnd()
    {
        // Check if current player has no moves
        if (!CanMakeaMove(_currentPlayer))
        {
            _status = StatusType.Win;
            var winner = _players.First(p => !p.Equals(_currentPlayer));
            FinishGame(winner => Console.WriteLine($"Game Over! Winner: {winner.Name} ({winner.Color}) - opponent has no valid moves"));
            return;
        }
        
        // Check if any player has no pieces left
        foreach (var player in _players)
        {
            if (!GetPlayerHasPiecesLeft(player))
            {
                _status = StatusType.Win;
                var winner = _players.First(p => !p.Equals(player));
                FinishGame(winner => Console.WriteLine($"Game Over! Winner: {winner.Name} ({winner.Color}) - opponent has no pieces left"));
                return;
            }
        }
        
        // Check for draw
        if (IsDraw())
        {
            _status = StatusType.Draw;
            FinishGame(winner => Console.WriteLine("Game Over! It's a draw!"));
        }
    }
    
    public void FinishGame(Action<IPlayer> showGameStatusCallback)
    {
        IPlayer winner = GetWinner();
        showGameStatusCallback(winner);
    }
    
    // Utility method for displaying the board
    public void DisplayBoard()
    {
        Console.WriteLine("\n  Current Board:");
        Console.WriteLine("   0 1 2 3 4 5 6 7");
        
        for (int x = 0; x < 8; x++)
        {
            Console.Write($"{x} ");
            for (int y = 0; y < 8; y++)
            {
                var piece = _board.GetPieceAt(new Position(x, y));
                if (piece == null)
                {
                    // Show dark squares with . and light squares with space
                    if (_board.IsDarkSquare(new Position(x, y)))
                        Console.Write(". ");
                    else
                        Console.Write("  ");
                }
                else
                {
                    char symbol = piece.Color == PieceColor.Black ? 
                        (piece.TypePiece == PieceType.Pawn ? 'b' : 'B') : 
                        (piece.TypePiece == PieceType.Pawn ? 'r' : 'R');
                    Console.Write($"{symbol} ");
                }
            }
            Console.WriteLine();
        }
    }
    
    public void DisplayGameStatus()
    {
        Console.WriteLine($"\nGame Status: {_status}");
        Console.WriteLine($"Current Player: {_currentPlayer?.Name} ({_currentPlayer?.Color})");
        Console.WriteLine($"Black pieces: {_playerData[_players[0]].Count}");
        Console.WriteLine($"Red pieces: {_playerData[_players[1]].Count}");
        Console.WriteLine($"Moves without capture: {_movesWithoutCapture}/{_maxMovesWithoutProgress}");
        if (_isInMultipleJump)
        {
            Console.WriteLine($"Multiple jump in progress with piece at {_activePieceInJump}");
        }
    }
    
    public void DisplayPossibleMoves(Position position)
    {
        var piece = _board.GetPieceAt(position);
        if (piece == null)
        {
            Console.WriteLine($"No piece at position {position}");
            return;
        }
        
        if (piece.Color != _currentPlayer.Color)
        {
            Console.WriteLine($"Piece at {position} belongs to opponent");
            return;
        }
        
        var moves = GetPossibleMovesForPiece(piece, AnyCaptureExists(_currentPlayer));
        if (moves.Count > 0)
        {
            Console.WriteLine($"Possible moves for piece at {position}:");
            foreach (var move in moves)
            {
                Console.WriteLine($"  -> {move.Item2}");
            }
        }
        else
        {
            Console.WriteLine($"No possible moves for piece at {position}");
        }
    }
}

// Main Program
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Checkers Game!");
        Console.WriteLine("Board symbols: b = black pawn, B = black king, r = red pawn, R = red king");
        Console.WriteLine("Black starts at top (rows 0-2), Red starts at bottom (rows 5-7)");
        Console.WriteLine("Note: Only dark squares (marked with '.') can contain pieces");
        
        // Create players
        IPlayer player1 = new Player("Player 1 (Black)", PieceColor.Black);
        IPlayer player2 = new Player("Player 2 (Red)", PieceColor.Red);
        
        // Create game
        CheckersGame game = new CheckersGame(player1, player2);
        
        // Start game
        game.StartGame();
        
        // Simple game loop for demonstration
        bool gameRunning = true;
        while (gameRunning)
        {
            game.DisplayBoard();
            game.DisplayGameStatus();
            
            Console.WriteLine("\nCommands:");
            Console.WriteLine("  move: fromX fromY toX toY (e.g., '2 1 3 2')");
            Console.WriteLine("  moves: show moves for position (e.g., 'moves 2 1')");
            Console.WriteLine("  quit: exit game");
            Console.Write("\nEnter command: ");
            
            string input = Console.ReadLine()?.Trim();
            
            if (input?.ToLower() == "quit" || input?.ToLower() == "q")
                break;
                
            if (string.IsNullOrWhiteSpace(input))
                continue;
                
            try
            {
                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                
                if (parts[0].ToLower() == "moves" && parts.Length == 3)
                {
                    Position pos = new Position(int.Parse(parts[1]), int.Parse(parts[2]));
                    game.DisplayPossibleMoves(pos);
                }
                else if (parts.Length == 4)
                {
                    Position from = new Position(int.Parse(parts[0]), int.Parse(parts[1]));
                    Position to = new Position(int.Parse(parts[2]), int.Parse(parts[3]));
                    
                    if (game.MovePiece(from, to))
                    {
                        Console.WriteLine("Move successful!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid move! Try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid command!");
                    Console.WriteLine("For moves: 'moves x y'");
                    Console.WriteLine("For move: 'fromX fromY toX toY'");
                    Console.WriteLine("Example: '2 1 3 2' or 'moves 2 1'");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Please enter valid numbers!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
        
        Console.WriteLine("Thanks for playing!");
    }
}