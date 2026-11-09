using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Logic;

// Season outcome probabilities
// Recursion calculates final-win totals
internal static class Forecast
{
    public const int BowlWins = 6;
    public const double MissingChance = 0.5;
    // Playoff proxy: maximum two losses
    public const int PlayoffLosses = 2;

    // Calculate season outlook
    public static SeasonOutlook Outlook(List<Game> games, IEnumerable<Game> upcoming, int season, WhatIfStack whatIfs)
    {
        // Exclude remaining bowl games
        List<Game> left = new List<Game>();
        foreach (Game game in upcoming)
        {
            if (!game.IsBowl)
            {
                left.Add(game);
            }
        }

        // Count completed results
        int wins = 0;
        int losses = 0;
        foreach (Game game in games)
        {
            if (game.Season == season && game.IsWin)
            {
                wins++;
            }
            else if (game.Season == season && game.IsFinal)
            {
                losses++;
            }
        }

        // Calculate final-win probabilities
        double[] totals = Recursion.FinalWins(Chances(left, whatIfs), wins);
        return new SeasonOutlook(left, wins, losses, wins + losses + left.Count, totals, MostLikely(totals), BowlChance(totals) * 100);
    }

    // Calculate remaining game probabilities
    // What-if values: win = 1, loss = 0
    public static double[] Chances(List<Game> upcoming, WhatIfStack whatIfs)
    {
        // Convert percentages; missing defaults 50%
        double[] chances = new double[upcoming.Count];
        for (int i = 0; i < upcoming.Count; i++)
        {
            double? pct = upcoming[i].WinPct;
            chances[i] = pct.HasValue ? pct.Value / 100 : MissingChance;
        }

        // Apply newest prediction last
        foreach (WhatIf pick in whatIfs.OldestFirst())
        {
            int index = upcoming.IndexOf(pick.Game);
            if (index >= 0)
            {
                chances[index] = pick.Win ? 1 : 0;
            }
        }
        return chances;
    }

    // O(n): select likeliest win total
    public static int MostLikely(double[] totals)
    {
        int best = 0;
        for (int wins = 1; wins < totals.Length; wins++)
        {
            if (totals[wins] > totals[best])
            {
                best = wins;
            }
        }
        return best;
    }

    // O(n): sum bowl-eligible probabilities
    public static double BowlChance(double[] totals)
    {
        double sum = 0;
        for (int wins = BowlWins; wins < totals.Length; wins++)
        {
            sum += totals[wins];
        }
        return sum;
    }

    // O(n): sum playoff-proxy probabilities
    public static double PlayoffChance(double[] totals, int seasonGames)
    {
        double sum = 0;
        for (int wins = seasonGames - PlayoffLosses; wins < totals.Length; wins++)
        {
            // Skip negative win totals
            if (wins >= 0)
            {
                sum += totals[wins];
            }
        }
        return sum;
    }
}
