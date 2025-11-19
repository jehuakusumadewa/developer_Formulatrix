using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCheckers
{
    // ---------------- Position ----------------
    public class Position
    {
        public int Column { get; set; } // 0..7
        public int Row { get; set; }    // 0..7 (0 = top)
        public Position(int col, int row) { Column = col; Row = row; }
        public Position() { }

        public bool IsValid() => Column >= 0 && Column < 8 && Row >= 0 && Row < 8;

        // playable dark square when (row+col)%2==1
        public bool IsDarkSquare() => IsValid() && ((Row + Column) % 2 == 1);

        public Position Clone() => new Position(Column, Row);

        public override bool Equals(object obj)
        {
            if (obj is Position p) return p.Column == Column && p.Row == Row;
            return false;
        }

        public override int GetHashCode() => (Column << 3) ^ Row;
    }

    // ---------------- Piece Interfaces and Classes ----------------
    public interface IPiece
    {
        string Warna { get; set; }
        Position CurrentPosition { get; set; }
        bool IsRaja { get; set; }
        void MoveTo(Position newPosition);
        bool ShouldBePromoted();
        void PromoteToKing();
    }

    public class Piece : IPiece
    {
        public string Warna { get; set; } // "White" or "Black"
        public Position CurrentPosition { get; set; }
        public bool IsRaja { get; set; } = false;

        public Piece(string warna, Position pos)
        {
            Warna = warna;
            CurrentPosition = pos;
        }

        public void MoveTo(Position newPosition)
        {
            CurrentPosition = newPosition;
        }

        public void PromoteToKing() => IsRaja = true;

        public bool ShouldBePromoted()
        {
            if (IsRaja) return false;
            if (Warna == "White" && CurrentPosition.Row == 0) return true;
            if (Warna == "Black" && CurrentPosition.Row == 7) return true;
            return false;
        }

        public override string ToString() => Warna[0] + (IsRaja ? "K" : "P");
    }

    // ---------------- Player ----------------
    public class Player
    {
        public string Name { get; set; }
        public string Warna { get; set; } // "White"/"Black"
        public List<Piece> DaftarBidak { get; set; } = new List<Piece>();

        public Player(string name, string warna)
        {
            Name = name;
            Warna = warna;
        }

        public bool HasPiecesLeft() => DaftarBidak.Any();

        public bool CanMakeaMove(Board board)
        {
            foreach (var piece in DaftarBidak)
            {
                var moves = MoveCalculator.GetPossibleMovesForPiece(board, piece, requireCaptureOnly: false);
                if (moves.Any()) return true;
            }
            return false;
        }
    }

    // ---------------- Board ----------------
    public class Board
    {
        // Grid stores Piece or null. Indices: [col,row]
        public Piece[,] Grid { get; } = new Piece[8, 8];

        public bool IsPositionEmpty(Position position)
        {
            return position.IsValid() && Grid[position.Column, position.Row] == null;
        }
    }

    // ---------------- PieceManager ----------------
    public class PieceManager
    {
        public void InitializePieces(Player player, Board board)
        {
            player.DaftarBidak.Clear();
            if (player.Warna == "Black")
            {
                // Rows 0..2 on dark squares
                for (int r = 0; r <= 2; r++)
                    for (int c = 0; c < 8; c++)
                    {
                        var pos = new Position(c, r);
                        if (pos.IsDarkSquare())
                        {
                            var p = new Piece(player.Warna, pos);
                            player.DaftarBidak.Add(p);
                            PlacePiece(board, p, pos);
                        }
                    }
            }
            else // White
            {
                // Rows 5..7 on dark squares
                for (int r = 5; r <= 7; r++)
                    for (int c = 0; c < 8; c++)
                    {
                        var pos = new Position(c, r);
                        if (pos.IsDarkSquare())
                        {
                            var p = new Piece(player.Warna, pos);
                            player.DaftarBidak.Add(p);
                            PlacePiece(board, p, pos);
                        }
                    }
            }
        }

        public void PlacePiece(Board board, Piece piece, Position position)
        {
            if (!position.IsValid()) throw new ArgumentException("Invalid position");
            board.Grid[position.Column, position.Row] = piece;
            piece.CurrentPosition = position;
        }

        public Piece GetPieceAt(Board board, Position position)
        {
            if (!position.IsValid()) return null;
            return board.Grid[position.Column, position.Row];
        }

        public void RemovePiece(Board board, Position position)
        {
            if (!position.IsValid()) return;
            var p = board.Grid[position.Column, position.Row];
            if (p != null)
            {
                board.Grid[position.Column, position.Row] = null;
            }
        }

        public void MovePiece(Board board, Position from, Position to)
        {
            if (!from.IsValid() || !to.IsValid()) throw new ArgumentException("Invalid move coords");
            var p = GetPieceAt(board, from);
            if (p == null) throw new InvalidOperationException("No piece at source");
            board.Grid[to.Column, to.Row] = p;
            board.Grid[from.Column, from.Row] = null;
            p.MoveTo(to);
        }

        public void RemovePieceFromPlayerList(Player player1, Player player2, Position pos)
        {
            // captured piece: find which player had it
            var p = player1.DaftarBidak.FirstOrDefault(x => x.CurrentPosition.Equals(pos));
            if (p != null) 
            {
                player1.DaftarBidak.Remove(p);
            }
            else
            {
                p = player2.DaftarBidak.FirstOrDefault(x => x.CurrentPosition.Equals(pos));
                if (p != null) 
                {
                    player2.DaftarBidak.Remove(p);
                }
            }
        }
    }

    // ---------------- Display ----------------
    public class Display
    {
        public void Print(Board board, PieceManager pieceManager)
        {
            Console.WriteLine();
            Console.WriteLine("   a b c d e f g h");
            for (int r = 0; r < 8; r++)
            {
                Console.Write(8 - r + "  ");
                for (int c = 0; c < 8; c++)
                {
                    var pos = new Position(c, r);
                    if (!pos.IsDarkSquare())
                    {
                        Console.Write("  "); // light square
                    }
                    else
                    {
                        var p = pieceManager.GetPieceAt(board, pos);
                        if (p == null) 
                            Console.Write(". ");
                        else
                        {
                            char ch;
                            if (p.Warna == "White") 
                                ch = p.IsRaja ? 'W' : 'w';
                            else 
                                ch = p.IsRaja ? 'B' : 'b';
                            Console.Write(ch + " ");
                        }
                    }
                }
                Console.WriteLine(" " + (8 - r));
            }
            Console.WriteLine("   a b c d e f g h");
            Console.WriteLine();
        }
    }

    // ---------------- UserInputHandler ----------------
    public class UserInputHandler
    {
        public static bool TryParseAlgebraic(string token, out Position pos)
        {
            pos = null;
            token = token.Trim().ToLower();
            if (token.Length < 2) return false;
            char colC = token[0];
            if (colC < 'a' || colC > 'h') return false;
            if (!int.TryParse(token.Substring(1), out int rowInput)) return false;
            if (rowInput < 1 || rowInput > 8) return false;
            int col = colC - 'a';
            int row = 8 - rowInput; // convert to 0=top
            pos = new Position(col, row);
            return true;
        }
    }

    // ---------------- MoveCalculator ----------------
    public class MoveCalculator
    {
        public static List<(Position to, Position captured)> GetPossibleMovesForPiece(Board board, Piece piece, bool requireCaptureOnly)
        {
            var res = new List<(Position, Position)>();
            if (piece == null) return res;
            
            var dirs = new List<(int dc, int dr)>();
            if (piece.IsRaja)
            {
                dirs.AddRange(new[] { (-1, -1), (1, -1), (-1, 1), (1, 1) });
            }
            else
            {
                int dir = piece.Warna == "White" ? -1 : 1;
                dirs.AddRange(new[] { (-1, dir), (1, dir) });
            }

            Position p = piece.CurrentPosition;
            var pieceManager = new PieceManager();

            // First check captures (jump over opponent)
            foreach (var (dc, dr) in dirs)
            {
                var adj = new Position(p.Column + dc, p.Row + dr);
                var land = new Position(p.Column + 2 * dc, p.Row + 2 * dr);
                if (adj.IsValid() && land.IsValid())
                {
                    var adjPiece = pieceManager.GetPieceAt(board, adj);
                    if (adjPiece != null && adjPiece.Warna != piece.Warna && board.IsPositionEmpty(land))
                    {
                        res.Add((land, adj));
                    }
                }
            }

            if (res.Any() || requireCaptureOnly) return res;

            // Non-capture moves
            foreach (var (dc, dr) in dirs)
            {
                var dest = new Position(p.Column + dc, p.Row + dr);
                if (dest.IsValid() && board.IsPositionEmpty(dest))
                    res.Add((dest, null));
            }

            return res;
        }

        public static bool AnyCaptureExists(Board board, Player player)
        {
            foreach (var piece in player.DaftarBidak)
            {
                var moves = GetPossibleMovesForPiece(board, piece, requireCaptureOnly: true);
                if (moves.Any()) return true;
            }
            return false;
        }
    }

    // ---------------- MoveValidator ----------------
    public class MoveValidator
    {
        private Board board;
        private Player currentPlayer;
        private PieceManager pieceManager;

        public MoveValidator(Board board, Player currentPlayer)
        {
            this.board = board;
            this.currentPlayer = currentPlayer;
            this.pieceManager = new PieceManager();
        }

        public bool IsValidMove(Position from, Position to, out bool isCapture, out Position capturedPos)
        {
            isCapture = false;
            capturedPos = null;

            if (!from.IsValid() || !to.IsValid()) return false;
            var piece = pieceManager.GetPieceAt(board, from);
            if (piece == null) return false;
            if (piece.Warna != currentPlayer.Warna) return false;
            // ensure target dark square and empty
            if (!to.IsDarkSquare() || !board.IsPositionEmpty(to)) return false;

            bool forcingCapture = MoveCalculator.AnyCaptureExists(board, currentPlayer);

            var possible = MoveCalculator.GetPossibleMovesForPiece(board, piece, requireCaptureOnly: false);
            // see if any capture moves exist (and must be chosen)
            var captureOnly = MoveCalculator.GetPossibleMovesForPiece(board, piece, requireCaptureOnly: true);
            if (forcingCapture && !captureOnly.Any()) return false; // this piece can't capture while some other piece can

            // check if desired dest is in possible moves
            foreach (var mv in (forcingCapture ? captureOnly : possible))
            {
                if (mv.to.Column == to.Column && mv.to.Row == to.Row)
                {
                    if (mv.captured != null)
                    {
                        isCapture = true;
                        capturedPos = mv.captured;
                    }
                    return true;
                }
            }

            return false;
        }
    }

    // ---------------- MoveExecutor ----------------
    public class MoveExecutor
    {
        private Board board;
        private Player player1;
        private Player player2;
        private PieceManager pieceManager;

        public MoveExecutor(Board board, Player player1, Player player2)
        {
            this.board = board;
            this.player1 = player1;
            this.player2 = player2;
            this.pieceManager = new PieceManager();
        }

        public (bool success, bool shouldContinueTurn) MakeMove(Position from, Position to, Player currentPlayer)
        {
            var validator = new MoveValidator(board, currentPlayer);
            if (!validator.IsValidMove(from, to, out bool isCapture, out Position capturedPos))
            {
                return (false, false);
            }

            var piece = pieceManager.GetPieceAt(board, from);
            pieceManager.MovePiece(board, from, to);

            if (isCapture && capturedPos != null)
            {
                // remove captured piece
                pieceManager.RemovePiece(board, capturedPos);
                pieceManager.RemovePieceFromPlayerList(player1, player2, capturedPos);
            }

            // Promotion
            if (piece.ShouldBePromoted())
            {
                piece.PromoteToKing();
            }

            // If capture happened, check for multi-capture from new position
            if (isCapture)
            {
                var more = MoveCalculator.GetPossibleMovesForPiece(board, piece, requireCaptureOnly: true);
                if (more.Any())
                {
                    // allow the same player to continue capturing with same piece
                    return (true, true);
                }
            }

            return (true, false);
        }
    }

    // ---------------- GameState ----------------
    public class GameState
    {
        public Board Board { get; private set; }
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public Player CurrentPlayer { get; private set; }
        public string Status { get; private set; }

        private PieceManager pieceManager;
        private MoveExecutor moveExecutor;

        public GameState(string whiteName = "White", string blackName = "Black")
        {
            Board = new Board();
            Player1 = new Player(whiteName, "White");
            Player2 = new Player(blackName, "Black");
            pieceManager = new PieceManager();
            moveExecutor = new MoveExecutor(Board, Player1, Player2);
            Status = "NotStarted";
        }

        public void InitializeGame()
        {
            // clear board
            Board = new Board();
            pieceManager.InitializePieces(Player1, Board);
            pieceManager.InitializePieces(Player2, Board);
            CurrentPlayer = Player1;
            Status = "Playing";
        }

        public bool MakeMove(Position from, Position to)
        {
            var result = moveExecutor.MakeMove(from, to, CurrentPlayer);
            if (result.success)
            {
                if (!result.shouldContinueTurn)
                {
                    // switch turn only if no continuation needed
                    SwitchTurn();
                }
                CheckGameOver();
            }
            return result.success;
        }

        public void SwitchTurn()
        {
            CurrentPlayer = (CurrentPlayer == Player1) ? Player2 : Player1;
        }

        public void CheckGameOver()
        {
            if (!Player1.HasPiecesLeft() || !Player1.CanMakeaMove(Board))
            {
                Status = "BlackWins";
            }
            else if (!Player2.HasPiecesLeft() || !Player2.CanMakeaMove(Board))
            {
                Status = "WhiteWins";
            }
        }

        public Piece GetPieceAt(Position pos)
        {
            return pieceManager.GetPieceAt(Board, pos);
        }

        public bool AnyCaptureExists()
        {
            return MoveCalculator.AnyCaptureExists(Board, CurrentPlayer);
        }
    }

    // ---------------- Program (Main) ----------------
    class Program
    {
        static void Main(string[] args)
        {
            var gameState = new GameState("White", "Black");
            gameState.InitializeGame();
            var display = new Display();
            var pieceManager = new PieceManager();

            Console.WriteLine("=== Console Checkers ===");
            Console.WriteLine("Moves: enter like 'b6 c5' (columns a-h, rows 1-8; 1 is bottom).");
            Console.WriteLine("White is bottom (w), Black is top (b). White moves first.");
            Console.WriteLine("Type 'exit' to quit.\n");

            while (gameState.Status == "Playing")
            {
                display.Print(gameState.Board, pieceManager);
                Console.WriteLine($"{gameState.CurrentPlayer.Name} ({gameState.CurrentPlayer.Warna}) to move.");
                
                // show if capture(s) forced
                bool anyCapture = gameState.AnyCaptureExists();
                if (anyCapture) Console.WriteLine("You have at least one capture — you must capture!");

                Console.Write("Enter move: ");
                var line = Console.ReadLine();
                if (line == null) break;
                line = line.Trim();
                if (line.ToLower() == "exit") break;
                if (string.IsNullOrEmpty(line)) continue;

                // accept formats: "b6 c5" or "b6,c5"
                var parts = line.Split(new[] { ' ', ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                {
                    Console.WriteLine("Invalid input. Use format: 'b6 c5'");
                    continue;
                }

                if (!UserInputHandler.TryParseAlgebraic(parts[0], out var from) ||
                    !UserInputHandler.TryParseAlgebraic(parts[1], out var to))
                {
                    Console.WriteLine("Couldn't parse positions. Use columns a-h and rows 1-8.");
                    continue;
                }

                // check that from has a piece
                var piece = gameState.GetPieceAt(from);
                if (piece == null)
                {
                    Console.WriteLine("No piece at source.");
                    continue;
                }
                if (piece.Warna != gameState.CurrentPlayer.Warna)
                {
                    Console.WriteLine("That is not your piece.");
                    continue;
                }

                // make move
                bool moved = gameState.MakeMove(from, to);
                if (!moved)
                {
                    Console.WriteLine("Invalid move.");
                    continue;
                }

                // Check if player can capture again with the same piece
                if (moved)
                {
                    var movedPiece = gameState.GetPieceAt(to);
                    var moreCapture = MoveCalculator.GetPossibleMovesForPiece(gameState.Board, movedPiece, requireCaptureOnly: true);
                    if (moreCapture.Any())
                    {
                        Console.WriteLine("You captured and can capture again with the same piece. Continue (same player).");
                        // The turn will not switch due to shouldContinueTurn = true
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("Final board:");
            display.Print(gameState.Board, pieceManager);
            if (gameState.Status == "WhiteWins")
                Console.WriteLine("White wins!");
            else if (gameState.Status == "BlackWins")
                Console.WriteLine("Black wins!");
            else
                Console.WriteLine("Game ended.");
        }
    }
}