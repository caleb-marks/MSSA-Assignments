namespace Caleb_Marks_OU_Season_Analyzer.Models;

// One historical game
// Missing AP rank means unranked
internal sealed record PastGame(int Season, int Week, bool Postseason, DateTime Date, string OpponentId, string Opponent,
    string Site, int? Points, int? OppPoints, string Notes, int? Rank, int? OppRank)
{
    // Both scores present
    public bool IsFinal => Points.HasValue && OppPoints.HasValue;

    // OU score higher
    public bool IsWin => IsFinal && Points > OppPoints;

    // Opponent score higher
    public bool IsLoss => IsFinal && Points < OppPoints;

    // Score difference; missing scores count zero
    public int Margin => Points.GetValueOrDefault() - OppPoints.GetValueOrDefault();

    // Opponent label including rank
    public string RankedName => OppRank.HasValue ? $"#{OppRank.Value} {Opponent}" : Opponent;
}
