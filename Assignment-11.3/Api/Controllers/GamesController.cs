using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Caleb_Marks_Assignment_11._3_Api.Data;
using Caleb_Marks_Assignment_11._3_Api.Models;

namespace Caleb_Marks_Assignment_11._3_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameContext _context;

        // Receive context using dependency injection
        public GamesController(GameContext context)
        {
            _context = context;
        }

        // Return every game
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            return await _context.Games.ToListAsync();
        }

        // Return one game using ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            Game? game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // Replace matching game; route & body IDs must match
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Add new game & return it
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // Remove matching game
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            Game? game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Check whether game still exists
        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
