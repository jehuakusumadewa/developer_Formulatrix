using WebApplication1.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

// ini pelayannya

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // ini untuk url: api/othello
    public class OthelloController: ControllerBase
    {
        //service (dependency injection)
        private readonly IOthelloService _othelloService;
        // pelayan kan bisa mencatat makanan apa yg dipesan

        // CONSTRUCTOR - Service otomatis disediakan oleh .NET
        public OthelloController(IOthelloService othelloService)
        {
            _othelloService = othelloService;
        }

        [HttpGet("{gameId}")]
        public IActionResult GetGameState(string gameId)
        {
            try
            {
                // ini manggil service untuk dapat status game
                var gameState = _othelloService.GetGameState(gameId);
                //kirim response suksess
                return Ok(gameState);
            }
            catch (ArgumentException ex)
            {
                // jika game tidak ditemukan
                return NotFound(new
                {
                    error = "Game tidak ditemukan",
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // JIKA ADA ERROR LAIN
                return StatusCode(500, new { 
                    error = "Terjadi kesalahan internal",
                    message = ex.Message 
                });
            }

        }



    }
}