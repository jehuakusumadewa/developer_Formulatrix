using System;
using System.Collections.Generic;
using System.Linq;

// Enums
public enum PieceType
{
    Pawn,
    King
}

public enum PieceColor
{
    Red,
    Black
}

// Interfaces
public interface IBoard
{
    ICell[,] _squares { get; }
}

public interface IPiece
{
    PieceColor Color { get; }
    Position Position { get; set; }
    PieceType TypePiece { get; }
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

// Structs
public struct Position
{
    public int x;
    public int y;
    
    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    
    public override string ToString()
    {
        return $"({x},{y})";
    }
    
    public override bool Equals(object obj)
    {
        if (obj is Position other)
        {
            return x == other.x && y == other.y;
        }
        return false;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }
    
    public static bool operator ==(Position left, Position right)
    {
        return left.Equals(right);
    }
    
    public static bool operator !=(Position left, Position right)
    {
        return !left.Equals(right);
    }
}

public struct Cell : ICell
{
    public Position Position { get; }
    public IPiece Piece { get; set; }
    
    public Cell(Position position, IPiece piece = null)
    {
        Position = position;
        Piece = piece;
    }
}

// Classes
public class CheckerPiece : IPiece
{
    public PieceColor Color { get; private set; }
    public Position Position { get; set; }
    public PieceType TypePiece { get; private set; }

    public CheckerPiece(PieceColor color, Position position)
    {
        Color = color;
        Position = position;
        TypePiece = PieceType.Pawn;
    }

    public void PromoteToKing()
    {
        TypePiece = PieceType.King;
    }

    public void CheckAndPromote()
    {
        if ((Color == PieceColor.Red && Position.y == 7) || 
            (Color == PieceColor.Black && Position.y == 0))
        {
            PromoteToKing();
        }
    }
    
    public override string ToString()
    {
        return $"{Color} {TypePiece} at {Position}";
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
    
    public override string ToString()
    {
        return $"{Name} ({Color})";
    }
}

public class CheckersBoard : IBoard
{
    public ICell[,] _squares { get; private set; }

    public CheckersBoard()
    {
        _squares = new ICell[8, 8];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                _squares[x, y] = new Cell(new Position(x, y));
            }
        }
    }
    
    public void DisplayBoard()
    {
        Console.WriteLine("\n  +---+---+---+---+---+---+---+---+");
        for (int y = 7; y >= 0; y--)
        {
            Console.Write($"{y} |");
            for (int x = 0; x < 8; x++)
            {
                var piece = _squares[x, y].Piece;
                if (piece == null)
                {
                    Console.Write("   |");
                }
                else
                {
                    char symbol = piece.TypePiece == PieceType.Pawn ? 'p' : 'K';
                    char color = piece.Color == PieceColor.Red ? 'R' : 'B';
                    Console.Write($" {color}{symbol}|");
                }
            }
            Console.WriteLine();
            Console.WriteLine("  +---+---+---+---+---+---+---+---+");
        }
        Console.WriteLine("    0   1   2   3   4   5   6   7");
    }
}

// Player Data class to store player information and pieces
public class PlayerData
{
    public IPlayer Player { get; }
    public List<IPiece> Pieces { get; }
    
    public PlayerData(IPlayer player)
    {
        Player = player;
        Pieces = new List<IPiece>();
    }
    
    public void AddPiece(IPiece piece)
    {
        Pieces.Add(piece);
    }
    
    public void RemovePiece(IPiece piece)
    {
        Pieces.Remove(piece);
    }
    
    public bool HasPiecesLeft => Pieces.Count > 0;
}

// Main Game Class
public class CheckersGame
{
    private IBoard _board;
    private List<IPiece> _allPieces;
    private List<PlayerData> _playersData;
    private IPlayer _currentPlayer;
    private Position _activePieceInJump;
    private bool _isInMultipleJump;
    private string _status;

    public CheckersGame(IPlayer player1, IPlayer player2)
    {
        _board = new CheckersBoard();
        _allPieces = new List<IPiece>();
        _playersData = new List<PlayerData>
        {
            new PlayerData(player1),
            new PlayerData(player2)
        };
        _currentPlayer = player1;
        _isInMultipleJump = false;
        _status = "Not Started";
    }

    // Game Flow Methods
    public void StartGame()
    {
        InitializeBoard();
        foreach (var playerData in _playersData)
        {
            InitializePieces(playerData.Player);
        }
        _status = "In Progress";
        Console.WriteLine("Game Started!");
    }

    public bool MakeMove(Position from, Position to)
    {
        if (!IsValidPosition(from) || !IsValidPosition(to))
        {
            Console.WriteLine("Invalid position! Positions must be between 0-7.");
            return false;
        }

        if (IsPositionEmpty(from))
        {
            Console.WriteLine("No piece at starting position!");
            return false;
        }

        var piece = GetPieceAt(from);
        if (piece.Color != _currentPlayer.Color)
        {
            Console.WriteLine("Not your piece!");
            return false;
        }

        // If in multiple jump mode, must continue with the same piece
        if (_isInMultipleJump && !from.Equals(_activePieceInJump))
        {
            Console.WriteLine("Must continue multiple jump with the same piece!");
            return false;
        }

        bool isCapture;
        Position capturedPos;
        if (!IsValidMoveInternal(from, to, out isCapture, out capturedPos))
        {
            Console.WriteLine("Invalid move!");
            return false;
        }

        Console.WriteLine($"{_currentPlayer.Name} moves from {from} to {to}" + 
                         (isCapture ? $" capturing piece at {capturedPos}" : ""));

        MovePiece(from, to);
        
        if (isCapture)
        {
            HandleCapturedPiece(capturedPos);
            _isInMultipleJump = CheckMultipleJump(to);
            if (_isInMultipleJump)
            {
                _activePieceInJump = to;
                Console.WriteLine("Multiple jump available! Continue with the same piece.");
                DisplayBoard();
                return true;
            }
        }

        CheckAndPromotePiece(piece);
        SwitchPlayer();
        _isInMultipleJump = false;
        CheckGameOver();
        return true;
    }

    public void SwitchPlayer()
    {
        _currentPlayer = _currentPlayer == _playersData[0].Player ? 
                        _playersData[1].Player : _playersData[0].Player;
        Console.WriteLine($"\nNow it's {_currentPlayer.Name}'s turn ({_currentPlayer.Color})");
    }

    public IPlayer GetWinner()
    {
        foreach (var playerData in _playersData)
        {
            if (!HasPiecesLeft(playerData.Player) || !CanMakeaMove(playerData.Player))
            {
                return _playersData.Find(p => p.Player != playerData.Player)?.Player;
            }
        }
        return null;
    }

    public void DisplayWinner(Action<IPlayer> showWinnerCallback)
    {
        var winner = GetWinner();
        showWinnerCallback?.Invoke(winner);
    }

    public IPlayer GetCurrentPlayer()
    {
        return _currentPlayer;
    }

    public void CheckGameOver()
    {
        var winner = GetWinner();
        if (winner != null)
        {
            _status = "Game Over";
            Console.WriteLine($"\n*** GAME OVER ***");
            Console.WriteLine($"*** {winner.Name} WINS! ***");
        }
    }

    // Board Methods
    public void PlacePiece(IPiece piece, Position position)
    {
        if (IsValidPosition(position))
        {
            piece.Position = position;
            _board._squares[position.x, position.y].Piece = piece;
        }
    }

    public IPiece GetPieceAt(Position position)
    {
        return IsValidPosition(position) ? _board._squares[position.x, position.y].Piece : null;
    }

    public bool IsValidPosition(Position position)
    {
        return position.x >= 0 && position.x < 8 && position.y >= 0 && position.y < 8;
    }

    public bool IsPositionEmpty(Position position)
    {
        return GetPieceAt(position) == null;
    }

    public void MovePiece(Position from, Position to)
    {
        var piece = GetPieceAt(from);
        if (piece != null)
        {
            _board._squares[from.x, from.y].Piece = null;
            _board._squares[to.x, to.y].Piece = piece;
            piece.Position = to;
        }
    }

    public void InitializeBoard()
    {
        // Board initialization is handled in CheckersBoard constructor
    }

    // Piece Methods
    public void CheckAndPromotePiece(IPiece piece)
    {
        if (piece is CheckerPiece checkerPiece)
        {
            checkerPiece.CheckAndPromote();
            if (checkerPiece.TypePiece == PieceType.King)
            {
                Console.WriteLine($"{piece.Color} piece at {piece.Position} promoted to King!");
            }
        }
    }

    // Player Methods
    public bool HasPiecesLeft(IPlayer player)
    {
        var playerData = _playersData.Find(p => p.Player == player);
        return playerData?.HasPiecesLeft ?? false;
    }

    public bool CanMakeaMove(IPlayer player)
    {
        var playerData = _playersData.Find(p => p.Player == player);
        if (playerData == null || !playerData.HasPiecesLeft) return false;

        // Check if any piece can make a move
        foreach (var piece in playerData.Pieces)
        {
            var moves = GetPossibleMovesForPiece(piece, AnyCaptureExists(player));
            if (moves.Count > 0) return true;
        }
        return false;
    }

    public void InitializePieces(IPlayer player)
    {
        int startY = player.Color == PieceColor.Red ? 0 : 5;
        int endY = player.Color == PieceColor.Red ? 3 : 8;
        var playerData = _playersData.Find(p => p.Player == player);

        for (int y = startY; y < endY; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if ((x + y) % 2 == 1)
                {
                    var position = new Position(x, y);
                    var piece = new CheckerPiece(player.Color, position);
                    PlacePiece(piece, position);
                    _allPieces.Add(piece);
                    playerData.AddPiece(piece);
                }
            }
        }
    }

    // Move Calculation & Validation - INTERNAL VERSION without capture check
    private bool IsValidMoveInternal(Position from, Position to, out bool isCapture, out Position capturedPos)
    {
        isCapture = false;
        capturedPos = new Position(-1, -1);
        
        var piece = GetPieceAt(from);
        if (piece == null) return false;

        // Check if destination is empty
        if (!IsPositionEmpty(to)) return false;

        int dx = to.x - from.x;
        int dy = to.y - from.y;
        
        // Check if move is diagonal
        if (Math.Abs(dx) != Math.Abs(dy)) return false;
        
        // Check distance for pawns and kings
        if (piece.TypePiece == PieceType.Pawn)
        {
            // Pawns can only move 1 or 2 squares
            if (Math.Abs(dx) != 1 && Math.Abs(dx) != 2) return false;
            
            // Pawns can only move forward
            if ((piece.Color == PieceColor.Red && dy <= 0) || 
                (piece.Color == PieceColor.Black && dy >= 0))
                return false;
        }
        else
        {
            // Kings can move any diagonal distance but only 1 or 2 squares in this implementation
            if (Math.Abs(dx) != 1 && Math.Abs(dx) != 2) return false;
        }
        
        if (Math.Abs(dx) == 1) // Regular move
        {
            isCapture = false;
            return true;
        }
        else if (Math.Abs(dx) == 2) // Capture move
        {
            capturedPos = new Position(from.x + dx / 2, from.y + dy / 2);
            var capturedPiece = GetPieceAt(capturedPos);
            
            if (capturedPiece != null && capturedPiece.Color != piece.Color)
            {
                isCapture = true;
                return true;
            }
        }
        
        return false;
    }

    // Public version with forced capture check
    public bool IsValidMove(Position from, Position to, out bool isCapture, out Position capturedPos)
    {
        if (!IsValidMoveInternal(from, to, out isCapture, out capturedPos))
            return false;

        // Check forced capture rule
        if (AnyCaptureExists(_currentPlayer) && !isCapture)
        {
            Console.WriteLine("You must make a capture move!");
            return false;
        }

        return true;
    }

    public void HandleCapturedPiece(Position capturedPos)
    {
        var capturedPiece = GetPieceAt(capturedPos);
        if (capturedPiece != null)
        {
            _board._squares[capturedPos.x, capturedPos.y].Piece = null;
            _allPieces.Remove(capturedPiece);
            
            foreach (var playerData in _playersData)
            {
                if (playerData.Pieces.Contains(capturedPiece))
                {
                    playerData.RemovePiece(capturedPiece);
                    Console.WriteLine($"{playerData.Player.Name} lost a piece!");
                    break;
                }
            }
        }
    }

    private bool CheckMultipleJump(Position currentPosition)
    {
        var piece = GetPieceAt(currentPosition);
        if (piece == null) return false;

        // Check all possible capture moves from current position
        int[] directions = { -2, 2 };
        foreach (int dx in directions)
        {
            foreach (int dy in directions)
            {
                Position to = new Position(currentPosition.x + dx, currentPosition.y + dy);
                bool isCapture;
                Position capturedPos;
                
                if (IsValidMoveInternal(currentPosition, to, out isCapture, out capturedPos) && isCapture)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool AnyCaptureExists(IPlayer player)
    {
        var playerData = _playersData.Find(p => p.Player == player);
        if (playerData == null) return false;

        foreach (var piece in playerData.Pieces)
        {
            int[] directions = { -2, 2 };
            foreach (int dx in directions)
            {
                foreach (int dy in directions)
                {
                    Position to = new Position(piece.Position.x + dx, piece.Position.y + dy);
                    bool isCapture;
                    Position capturedPos;
                    
                    if (IsValidMoveInternal(piece.Position, to, out isCapture, out capturedPos) && isCapture)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private List<Tuple<Position, Position>> GetPossibleMovesForPiece(IPiece piece, bool requireCaptureOnly)
    {
        var moves = new List<Tuple<Position, Position>>();
        
        int[] distances = requireCaptureOnly ? new int[] { 2 } : new int[] { 1, 2 };
        
        foreach (int distance in distances)
        {
            int[] directions = { -distance, distance };
            foreach (int dx in directions)
            {
                foreach (int dy in directions)
                {
                    if (piece.TypePiece == PieceType.Pawn)
                    {
                        // Pawns can only move forward
                        if ((piece.Color == PieceColor.Red && dy <= 0) || 
                            (piece.Color == PieceColor.Black && dy >= 0))
                            continue;
                    }
                    
                    Position to = new Position(piece.Position.x + dx, piece.Position.y + dy);
                    bool isCapture;
                    Position capturedPos;
                    
                    if (IsValidMoveInternal(piece.Position, to, out isCapture, out capturedPos))
                    {
                        if (!requireCaptureOnly || isCapture)
                        {
                            moves.Add(new Tuple<Position, Position>(piece.Position, to));
                        }
                    }
                }
            }
        }
        
        return moves;
    }

    public void DisplayBoard()
    {
        if (_board is CheckersBoard checkersBoard)
        {
            checkersBoard.DisplayBoard();
        }
    }
    
    public void DisplayGameStatus()
    {
        Console.WriteLine($"\n=== Game Status ===");
        Console.WriteLine($"Status: {_status}");
        Console.WriteLine($"Current Player: {_currentPlayer}");
        foreach (var playerData in _playersData)
        {
            Console.WriteLine($"{playerData.Player.Name}: {playerData.Pieces.Count} pieces remaining");
        }
        
        // Show available moves for current player
        var currentPlayerData = _playersData.Find(p => p.Player == _currentPlayer);
        if (currentPlayerData != null)
        {
            bool hasCapture = AnyCaptureExists(_currentPlayer);
            Console.WriteLine($"Available moves for {_currentPlayer.Name} ({(hasCapture ? "CAPTURE REQUIRED" : "REGULAR MOVES")}):");
            bool hasMoves = false;
            foreach (var piece in currentPlayerData.Pieces)
            {
                var moves = GetPossibleMovesForPiece(piece, hasCapture);
                if (moves.Count > 0)
                {
                    hasMoves = true;
                    Console.Write($"  Piece at {piece.Position}: ");
                    foreach (var move in moves)
                    {
                        Console.Write($"{move.Item2} ");
                    }
                    Console.WriteLine();
                }
            }
            if (!hasMoves)
            {
                Console.WriteLine("  No available moves!");
            }
        }
        Console.WriteLine("===================\n");
    }
}

// Main Program
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== CHECKERS GAME ===");
        Console.WriteLine("How to play:");
        Console.WriteLine("- Enter moves as: fromX fromY toX toY");
        Console.WriteLine("- Example: '1 2 2 3' to move from (1,2) to (2,3)");
        Console.WriteLine("- Type 'quit' to exit the game");
        Console.WriteLine("- Red (R) starts at bottom, Black (B) at top");
        Console.WriteLine("- 'p' = Pawn, 'K' = King");
        Console.WriteLine("=====================\n");
        
        // Create players
        IPlayer player1 = new Player("Player 1", PieceColor.Red);
        IPlayer player2 = new Player("Player 2", PieceColor.Black);
        
        // Create game
        CheckersGame game = new CheckersGame(player1, player2);
        game.StartGame();
        
        // Game loop
        while (true)
        {
            game.DisplayBoard();
            game.DisplayGameStatus();
            
            IPlayer currentPlayer = game.GetCurrentPlayer();
            
            // Check for winner
            var winner = game.GetWinner();
            if (winner != null)
            {
                Console.WriteLine($"*** {winner.Name} WINS THE GAME! ***");
                break;
            }
            
            Console.Write($"Enter move for {currentPlayer.Name} (fromX fromY toX toY) or 'quit': ");
            string input = Console.ReadLine();
            
            if (input.ToLower() == "quit")
            {
                Console.WriteLine("Thanks for playing!");
                break;
            }
            
            string[] coordinates = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (coordinates.Length == 4 &&
                int.TryParse(coordinates[0], out int fromX) &&
                int.TryParse(coordinates[1], out int fromY) &&
                int.TryParse(coordinates[2], out int toX) &&
                int.TryParse(coordinates[3], out int toY))
            {
                Position from = new Position(fromX, fromY);
                Position to = new Position(toX, toY);
                
                bool moveSuccess = game.MakeMove(from, to);
                if (!moveSuccess)
                {
                    Console.WriteLine("Move failed! Try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Invalid input! Use format: fromX fromY toX toY");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            
            Console.WriteLine();
        }
        
        Console.WriteLine("Game ended. Press any key to exit...");
        Console.ReadKey();
    }
}