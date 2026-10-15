namespace Caleb_Marks_OU_Season_Analyzer.Models;

// Historical season outcome
// Empty Bowl means no bowl
internal sealed class SeasonEnd
{
    public int Season { get; }
    public int Wins { get; }
    public int Losses { get; }
    public string Bowl { get; }
    public int Ties { get; }
    // Playoff-level outcome
    public bool Playoff { get; }

    // Set season values
    public SeasonEnd(int season, int wins, int losses, string bowl, int ties = 0, bool playoff = false)
    {
        Season = season;
        Wins = wins;
        Losses = losses;
        Bowl = bowl;
        Ties = ties;
        Playoff = playoff;
    }

    // Format final record
    public string Record => Ties > 0 ? $"{Wins}-{Losses}-{Ties}" : $"{Wins}-{Losses}";

    // Select verdict category
    public Finish Finish
    {
        get
        {
            if (Playoff)
            {
                return Finish.LockedIn;
            }
            return Finish.Cooked;
        }
    }

    // Format season summary
    public string Text => $"{Season}: {Record}, " + (Bowl.Length > 0 ? Bowl : "no bowl");
}
