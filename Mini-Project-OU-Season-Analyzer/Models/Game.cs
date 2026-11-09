namespace Caleb_Marks_OU_Season_Analyzer.Models;

// One saved Oklahoma game
// Schedule, forecast & box score
internal class Game
{
    // Postseason week marker
    public const int BowlWeek = 16;

    // Saved status labels
    public const string StatusFinal = "Final";
    public const string StatusInProgress = "In Progress";
    public const string StatusScheduled = "Scheduled";

    // Nullable columns allow missing values
    public int Season { get; set; }
    public int Week { get; set; }
    public DateTime Date { get; set; }
    public string EventId { get; set; } = "";
    public string Opponent { get; set; } = "";
    public string OpponentId { get; set; } = "";
    public string OpponentColor { get; set; } = "";
    public string OpponentAltColor { get; set; } = "";
    public string Site { get; set; } = "";
    public string Status { get; set; } = "";
    public int? OUScore { get; set; }
    public int? OppScore { get; set; }
    public double? WinPct { get; set; }
    public DateTime? OddsAsOf { get; set; }
    public int? OUYards { get; set; }
    public int? OUPlays { get; set; }
    public int? OppYards { get; set; }
    public int? OppPlays { get; set; }
    public int? OURushYds { get; set; }
    public int? OURushAtt { get; set; }
    public int? OUPassYds { get; set; }
    public int? OppRushYds { get; set; }
    public int? OppRushAtt { get; set; }
    public int? OU3rdMade { get; set; }
    public int? OU3rdAtt { get; set; }
    public int? OUInt { get; set; }
    public int? OUFumLost { get; set; }
    public string FirstDrive { get; set; } = "";
    public double? FPI { get; set; }

    // Completed status, scores optional
    public bool IsCompleted => Status == StatusFinal;

    // Completed game, both scores required
    public bool IsFinal => IsCompleted && OUScore.HasValue && OppScore.HasValue;

    // Live game status
    public bool IsInProgress => Status == StatusInProgress;

    // Postseason game marker
    public bool IsBowl => Week == BowlWeek;

    // Road game marker
    public bool IsAway => Site == "Away";

    // Completed Oklahoma win
    public bool IsWin => IsFinal && OUScore > OppScore;

    // Required box-score values
    // Giveaways & first drive remain optional
    public bool HasStats =>
        IsFinal && OUPlays > 0 && OppPlays > 0 && OU3rdAtt > 0 && OppRushAtt > 0 && OURushAtt > 0
        && OUYards.HasValue && OppYards.HasValue && OURushYds.HasValue && OUPassYds.HasValue
        && OppRushYds.HasValue && OU3rdMade.HasValue;

    // Opponent label including site
    public string Matchup => IsAway ? "at " + Opponent : Site == "Neutral" ? Opponent + " (neutral site)" : Opponent;

    // Short opponent label
    public string ShortName => (IsAway ? "at " : "") + Opponent;

    // Final score or scheduled date
    public string ResultText => IsFinal ? (IsWin ? "W " : "L ") + OUScore + "-" + OppScore : Date.ToString("ddd, MMM d");

    // Create independent game copy
    public Game Copy() => (Game)MemberwiseClone();

    // OU yards-per-play; requires HasStats
    public double YardsPerPlay => (double)OUYards!.Value / OUPlays!.Value;

    // Opponent yards-per-play; requires HasStats
    public double OppYardsPerPlay => (double)OppYards!.Value / OppPlays!.Value;

    // Yards-per-play difference
    public double YppMargin => YardsPerPlay - OppYardsPerPlay;

    // Opponent yards-per-carry; requires HasStats
    public double OppYardsPerCarry => (double)OppRushYds!.Value / OppRushAtt!.Value;

    // Third-down percentage; requires HasStats
    public double ThirdDownPct => 100.0 * OU3rdMade!.Value / OU3rdAtt!.Value;

    // Total giveaways; missing counts remain null
    public int? Giveaways => OUInt + OUFumLost;

    // First-drive scoring result; missing drive remains null
    public bool? ScoredFirstDrive =>
        string.IsNullOrWhiteSpace(FirstDrive) ? null : FirstDrive == "Touchdown" || FirstDrive == "Field Goal";
}
