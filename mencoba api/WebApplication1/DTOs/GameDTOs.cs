namespace WebApplication1.DTOs
{
    // response untuk client untuk state game
    public class GameStateResponse {
        public string GameId {get; set;} = string.Empty;
        public string Status {get; set;} = string.Empty;
        public string CurrentPlayer {get; set;} = string.Empty;
        public int BlackScore {get; set;}
        public int WhiteScore {get; set;}
        public string Winner {get; set;}
        public string Message {get; set;}
    }



}