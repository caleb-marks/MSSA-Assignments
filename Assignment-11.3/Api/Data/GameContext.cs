using Caleb_Marks_Assignment_11._3_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Caleb_Marks_Assignment_11._3_Api.Data
{
    // Database session holding Games table
    public class GameContext : DbContext
    {
        public DbSet<Game> Games { get; set; }

        // Receive options using dependency injection
        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {
        }

        // Seed starting games
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasData(
                new Game { Id = 1, Season = 2017, Opponent = "Ohio State", OuScore = 31, OpponentScore = 16, Location = "Columbus" },
                new Game { Id = 2, Season = 2018, Opponent = "Texas", OuScore = 39, OpponentScore = 27, Location = "Arlington" },
                new Game { Id = 3, Season = 2023, Opponent = "Texas", OuScore = 34, OpponentScore = 30, Location = "Dallas" });
        }
    }
}
