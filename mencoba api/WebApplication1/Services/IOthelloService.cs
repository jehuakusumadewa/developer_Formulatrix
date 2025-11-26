using WebApplication1.DTOs;
//ini resepnya
namespace WebApplication1.Services
{
    //interface - kontrak untuk service

    public interface IOthelloService
    {
        GameStateResponse GetGameState(string gameId);
    }
}