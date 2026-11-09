namespace Caleb_Marks_OU_Season_Analyzer.Models;

// Historical season verdict
// Tied votes produce Cooked
internal sealed class HistoryVerdict
{
    // Minimum sample size
    public const int OddsAt = 5;

    public Finish Call { get; }
    public List<SeasonEnd> Like { get; }
    public int Need { get; }
    public IReadOnlyDictionary<int, int>? Points { get; }

    // Set verdict values
    public HistoryVerdict(Finish call, List<SeasonEnd> like, int need, IReadOnlyDictionary<int, int>? points = null)
    {
        Call = call;
        Like = like;
        Need = need;
        Points = points;
    }

    // Matching season count
    public int Matches => Like.Count;

    // Check sample size
    public bool ShowOdds => Matches >= OddsAt;

    // Count selected outcome
    public int Votes(Finish finish) => Like.Count(s => s.Finish == finish);

    // Calculate outcome percentage
    public int Percent(Finish finish) => Matches == 0 ? 0 : (int)Math.Round(100.0 * Votes(finish) / Matches);

    // Count matching comparison cards
    public int Cards(int season) => Points != null && Points.TryGetValue(season, out int cards) ? cards : 0;

    // O(n log n): order closest matches
    public List<SeasonEnd> MostAlike => Like.OrderByDescending(s => Cards(s.Season)).ThenByDescending(s => s.Season).ToList();
}
