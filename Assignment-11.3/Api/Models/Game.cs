namespace Caleb_Marks_Assignment_11._3_Api.Models
{
    // Game class maps Games table
    public class Game
    {
        public int Id { get; set; }
        public int Season { get; set; }
        public string Opponent { get; set; } = string.Empty;
        public int OuScore { get; set; }
        public int OpponentScore { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}
