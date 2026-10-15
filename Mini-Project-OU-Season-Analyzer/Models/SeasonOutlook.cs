namespace Caleb_Marks_OU_Season_Analyzer.Models;

// Season forecast results
internal sealed record SeasonOutlook(List<Game> Left, int WinsSoFar, int Losses, int SeasonGames, double[] Totals, int MostLikely,
    double BowlPercent)
{
    // Likeliest regular-season record
    public string Record => $"{MostLikely}-{SeasonGames - MostLikely}";

    // Likeliest record's percentage
    public double MostLikelyPercent => Totals[MostLikely] * 100;
}
