using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Logic;

// Stat ranking definition
internal sealed record StatDef(string Key, string Label, string Unit, bool HigherIsBetter);

// Calculate national stat rankings
// Recompute ranks using saved values
internal static class Ladder
{
    // Primary stat definitions
    public static readonly StatDef[] Ladders =
    {
        new StatDef("pointsFor", "Points scored", "pts/game", true),
        new StatDef("pointsAgainst", "Points allowed", "pts/game", false),
        new StatDef("rushYds", "Rush yards", "yds/game", true),
        new StatDef("passYds", "Pass yards", "yds/game", true),
        new StatDef("turnoverMarginPG", "Turnover margin", "/game", true),
        new StatDef("thirdDownPct", "3rd-down %", "%", true),
    };

    // Additional stat definitions
    private static readonly StatDef[] Extras =
    {
        new StatDef("totalYds", "Total yards", "yds/game", true),
        new StatDef("totalYdsAllowed", "Yards allowed", "yds/game", false),
        new StatDef("rushYdsAllowed", "Rush yards allowed", "yds/game", false),
        new StatDef("passYdsAllowed", "Pass yards allowed", "yds/game", false),
        new StatDef("thirdDownPctAllowed", "3rd-down % allowed", "%", false),
        new StatDef("yardsPerPlay", "Yards per play", "yds", true),
        new StatDef("yardsPerPlayAllowed", "Yards per play allowed", "yds", false),
        new StatDef("rushYpc", "Yards per carry", "yds", true),
        new StatDef("rushYpcAllowed", "Yards per carry allowed", "yds", false),
        new StatDef("sacksMade", "Sacks", "sacks", true),
        new StatDef("turnoverMargin", "Turnover margin", "total", true),
        new StatDef("takeaways", "Takeaways", "total", true),
        new StatDef("giveaways", "Giveaways", "total", false),
        new StatDef("giveawaysPG", "Giveaways", "/game", false),
        new StatDef("takeawaysPG", "Takeaways", "/game", true),
    };

    // Find stat definition
    public static StatDef? Def(string key)
    {
        StatDef? def = Array.Find(Ladders, d => d.Key == key);
        if (def == null)
        {
            def = Array.Find(Extras, d => d.Key == key);
        }
        return def;
    }

    // Build available primary rankings
    public static List<StatRank> Build(string teamId, List<TeamStatValue> all)
    {
        List<StatRank> ladders = new List<StatRank>();

        // Calculate each primary ranking
        foreach (StatDef def in Ladders)
        {
            StatRank? rank = Find(teamId, def.Key, all);
            if (rank != null)
            {
                ladders.Add(rank);
            }
        }
        return ladders;
    }

    // O(n log n): summarize stat ranking
    public static StatRank? Find(string teamId, string key, List<TeamStatValue> all)
    {
        StatDef? def = Def(key);
        List<TeamStatValue> rows = all.Where(r => r.Key == key).ToList();
        TeamStatValue? mine = rows.Find(r => r.TeamId == teamId);
        if (def == null || mine == null)
        {
            return null;
        }

        // Count strictly better teams
        int better = 0;
        TeamStatValue best = rows[0];
        foreach (TeamStatValue row in rows)
        {
            better += IsBetter(def, row.Value, mine.Value) ? 1 : 0;
            bool beatsBest = IsBetter(def, row.Value, best.Value)
                || (row.Value == best.Value && string.Compare(row.TeamName, best.TeamName, StringComparison.OrdinalIgnoreCase) < 0);
            best = beatsBest ? row : best;
        }

        // Calculate median using merge sort
        List<double> values = rows.Select(r => r.Value).ToList();
        MergeSort.Sort(values, (a, b) => a.CompareTo(b));
        double middle = values.Count % 2 == 1 ? values[values.Count / 2] : (values[values.Count / 2 - 1] + values[values.Count / 2]) / 2;
        return new StatRank(key, def.Label, def.Unit, def.HigherIsBetter, mine.Value, better + 1, rows.Count,
            best.TeamId, best.TeamName, best.Color, best.Value, middle);
    }

    // Find ranking matching stat description
    public static StatRank? ForGap(string teamId, string gapName, List<TeamStatValue> all)
    {
        string name = gapName.Trim().ToLowerInvariant();
        bool opponent = name.StartsWith("opponent", StringComparison.Ordinal);

        // Select first matching phrase
        string[] phrases = { "3rd-down conversion", "yards per carry", "yards per play", "turnovers per game", "rushing yards per game", "passing yards per game" };
        string[] ownKeys = { "thirdDownPct", "rushYpc", "yardsPerPlay", "giveawaysPG", "rushYds", "passYds" };
        string[] oppKeys = { "thirdDownPct", "rushYpcAllowed", "yardsPerPlayAllowed", "giveawaysPG", "rushYdsAllowed", "passYdsAllowed" };
        for (int i = 0; i < phrases.Length; i++)
        {
            if (name.Contains(phrases[i]))
            {
                return Find(teamId, opponent ? oppKeys[i] : ownKeys[i], all);
            }
        }
        return null;
    }

    // Compare stat values
    private static bool IsBetter(StatDef def, double a, double b) => def.HigherIsBetter ? a > b : a < b;

    // O(k n log n): assign competition ranks
    internal static List<TeamStatValue> RankAll(List<TeamStatValue> rows)
    {
        // Group row indexes using stat keys
        Dictionary<string, List<int>> byKey = new Dictionary<string, List<int>>();
        for (int i = 0; i < rows.Count; i++)
        {
            if (!byKey.ContainsKey(rows[i].Key))
            {
                byKey[rows[i].Key] = new List<int>();
            }
            byKey[rows[i].Key].Add(i);
        }

        TeamStatValue[] ranked = rows.ToArray();
        foreach (KeyValuePair<string, List<int>> group in byKey)
        {
            StatDef? def = Def(group.Key);
            if (def == null)
            {
                continue;
            }

            // Equal values share rank
            List<int> spots = group.Value;
            MergeSort.Sort(spots, (a, b) => def.HigherIsBetter ? rows[b].Value.CompareTo(rows[a].Value) : rows[a].Value.CompareTo(rows[b].Value));
            for (int n = 0; n < spots.Count; n++)
            {
                bool tied = n > 0 && rows[spots[n]].Value == rows[spots[n - 1]].Value;
                TeamStatValue r = rows[spots[n]];
                ranked[spots[n]] = new TeamStatValue(r.TeamId, r.TeamName, r.Color, r.Key, r.Value, tied ? ranked[spots[n - 1]].Rank : n + 1);
            }
        }
        return ranked.ToList();
    }
}

// Select saved player leaders
internal static class Stars
{
    // Select season category leaders
    public static List<PlayerStar> Season(List<PlayerStar> all)
    {
        List<PlayerStar> row = new List<PlayerStar>();
        foreach (string category in new string[] { "passingYards", "rushingYards", "receivingYards", "totalTackles", "sacks" })
        {
            // O(n): find category maximum
            PlayerStar? top = null;
            foreach (PlayerStar star in all)
            {
                if (star.EventId.Length == 0 && star.Category == category && (top == null || star.Value > top.Value))
                {
                    top = star;
                }
            }
            if (top != null)
            {
                row.Add(top);
            }
        }
        return row;
    }

    // Select weighted game leader
    public static PlayerStar? ForGame(List<PlayerStar> all, string eventId)
    {
        if (eventId.Length == 0)
        {
            return null;
        }

        // Category order breaks ties
        string[] categories = { "passingYards", "rushingYards", "receivingYards" };
        double[] divisors = { 2.5, 1.0, 1.0 };
        PlayerStar? star = null;
        double bestScore = double.MinValue;
        for (int c = 0; c < categories.Length; c++)
        {
            // O(n): examine matching player rows
            foreach (PlayerStar row in all)
            {
                if (row.EventId == eventId && row.Category == categories[c] && row.Value / divisors[c] > bestScore)
                {
                    star = row;
                    bestScore = row.Value / divisors[c];
                }
            }
        }
        return star;
    }
}
