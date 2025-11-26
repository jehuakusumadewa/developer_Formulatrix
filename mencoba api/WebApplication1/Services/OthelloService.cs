
using WebApplication1.DTOs;
using WebApplication1.Models;

// ini koki yg masak
namespace WebApplication1.Services
{
    public class OthelloService: IOthelloService
    {
        // simpan game di memory (seperti lemari)
        private readonly Dictionary<string, OthelloGame> _games = new();

        public OthelloService()
        {
            //buat 1 game sample saat service dibuat
            InitializeSampleGame();
        }

        private void InitializeSampleGame()
        {
            //buat player
            var player1 = new Player { Name = "Alice", Color = DiskColor.Black };
            var player2 = new Player { Name = "Bob", Color = DiskColor.White };

            var game = new OthelloGame(
                players: new List<IPlayer> { player1, player2 },
                board: new OthelloBoard(),
                size: 8,
                status: StatusType.NotStart,
                currentPlayer: player1,
                directions: GetDirections()
            );

            game.StartGame();
            _games["sample-game"] = game;
       
        }

        public GameStateResponse GetGameState(string gameId)
        {
            // cek apakah game sudah di buat dan di simpan ?
            if (!_games.ContainsKey(gameId))
            {
                throw new ArgumentException($"Game dengan ID '{gameId}' tidak ditemukan");
            }
            //ambil game dari penyimpanan
            var game = _games[gameId];

            return new GameStateResponse
            {
                GameId = gameId,
                Status = game.IsGameActive() ? "Playing" : "Finished",
                CurrentPlayer = game.GetCurrentPlayer().Name,
                BlackScore = game.GetPlayerScore(game.GetPlayerByColor(DiskColor.Black)),
                WhiteScore = game.GetPlayerScore(game.GetPlayerByColor(DiskColor.White)),
                Winner = game.GetWinner()?.Name ?? "",
                Message = "Berhasil mengambil status game"
            };
        }

        private static List<Position> GetDirections()
        {
            return new List<Position>
            {
                new Position(-1, -1), new Position(-1, 0), new Position(-1, 1),
                new Position(0, -1),                      new Position(0, 1),
                new Position(1, -1),  new Position(1, 0),  new Position(1, 1)
            };
        }






    }


}