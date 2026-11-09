using System.Globalization;
using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Logic;

// Compare historical season starts
// Exclude current & future seasons
internal static class History
{
    // Maximum scoring-margin difference
    public const double MarginWindow = 5;
    // Minimum significant poll change
    public const int RankMove = 5;
    // Unranked poll placeholder
    private const int Unranked = 26;
    // First playoff season
    public const int FirstPlayoffYear = 2014;

    // Two-word bowl names
    private static readonly string[] TwoWordBowls = { "Forces", "Mexico", "Responder", "Diego", "Orleans", "Jersey", "Frisco", "Boca" };

    // Pre-2014 qualifying bowls
    private static readonly string[] BigBowls = { "Rose", "Orange", "Sugar", "Fiesta", "BCS National Championship" };

    // OU seasons missing playoff notes
    // 2000 Orange; 2007 Fiesta; 2015 Orange
    private static readonly int[] BlankPlayoffYears = { 2000, 2007, 2015 };

    // Historical season comparison data
    private sealed record PastSeason(List<PastGame> Games, List<PastGame> First, SeasonEnd End);

    // O(n log n): group chronological seasons
    public static SortedDictionary<int, List<PastGame>> BySeason(List<PastGame> all)
    {
        SortedDictionary<int, List<PastGame>> seasons = new SortedDictionary<int, List<PastGame>>();
        foreach (PastGame game in all)
        {
            if (!seasons.ContainsKey(game.Season))
            {
                seasons[game.Season] = new List<PastGame>();
            }
            seasons[game.Season].Add(game);
        }
        foreach (List<PastGame> list in seasons.Values)
        {
            MergeSort.Sort(list, (a, b) => a.Date.CompareTo(b.Date));
        }
        return seasons;
    }

    // O(g): select earliest completed games
    public static List<PastGame> FirstGames(List<PastGame> season, int n) => season.Where(g => g.IsFinal).Take(n).ToList();

    // O(g): summarize season outcome
    public static SeasonEnd End(int year, List<PastGame> season)
    {
        List<PastGame> finals = season.FindAll(g => g.IsFinal);
        int wins = finals.Count(g => g.IsWin);
        int losses = finals.Count(g => g.IsLoss);
        PastGame? bowl = finals.FindLast(g => g.Postseason);
        string bowlText = bowl == null ? "" : (bowl.IsWin ? "won " : bowl.IsLoss ? "lost " : "tied ") + BowlName(bowl.Notes);
        // Default: no playoff appearance
        bool playoff = false;
        if (bowl != null)
        {
            playoff = IsPlayoffLevel(year, bowl.Notes);
        }
        return new SeasonEnd(year, wins, losses, bowlText, finals.Count - wins - losses, playoff);
    }

    // Check playoff-level postseason
    // 2014+ requires explicit playoff notes
    // C8: classify postseason
    public static bool IsPlayoffLevel(int year, string notes)
    {
        // Check known missing-note seasons
        if (notes.Trim().Length == 0)
        {
            foreach (int blankYear in BlankPlayoffYears)
            {
                if (blankYear == year)
                {
                    return true;
                }
            }
            return false;
        }

        // Normalize note case
        string upper = notes.ToUpper();
        if (year >= FirstPlayoffYear)
        {
            return upper.Contains("CFP") || upper.Contains("PLAYOFF");
        }

        // Check qualifying BCS bowls
        foreach (string bigBowl in BigBowls)
        {
            if (upper.Contains(bigBowl.ToUpper()))
            {
                return true;
            }
        }
        return false;
    }

    // Extract short bowl name
    public static string BowlName(string notes)
    {
        if (notes.Contains("Playoff") || notes.Contains("CFP"))
        {
            return PlayoffName(notes);
        }
        string name = notes.Split(" - ")[0].Trim();
        return name.Length > 0 ? DropSponsor(name) : "bowl game";
    }

    // Identify playoff round
    private static string PlayoffName(string notes)
    {
        if (notes.Contains("Championship"))
        {
            return "Playoff final";
        }
        if (notes.Contains("Semifinal"))
        {
            return "Playoff semifinal";
        }
        return notes.Contains("Quarterfinal") ? "Playoff quarterfinal" : notes.Contains("First Round") ? "Playoff 1st round" : "Playoff";
    }

    // Remove bowl sponsor
    private static string DropSponsor(string name)
    {
        string[] words = name.Split(' ');
        int bowl = Array.FindIndex(words, w => w.Equals("Bowl", StringComparison.OrdinalIgnoreCase));
        if (bowl < 1)
        {
            return name;
        }
        int first = bowl >= 2 && TwoWordBowls.Contains(words[bowl - 1]) ? bowl - 2 : bowl - 1;
        string shortName = string.Join(" ", words, first, bowl + 1 - first);
        return shortName == shortName.ToUpperInvariant() ? CultureInfo.InvariantCulture.TextInfo.ToTitleCase(shortName.ToLowerInvariant()) : shortName;
    }

    // Build five historical comparisons
    public static List<Comp> Build(List<PastGame> all, int current)
    {
        SortedDictionary<int, List<PastGame>> seasons = BySeason(all);
        List<PastGame> now = SeasonGames(seasons, current);
        int n = now.Count(g => g.IsFinal);
        List<PastSeason> past = Past(seasons, current, n);
        Comp rivalry = Rivalry(all, now, current, RivalId(all, current), past);
        if (n == 0)
        {
            string wait = $"No {current} games played yet";
            return new List<Comp> { Empty("start", "Same start", wait), Empty("margin", "Same scoring margin", wait),
                Empty("ranked", "Vs. ranked teams", wait), rivalry, Empty("poll", "AP poll move", wait) };
        }
        return new List<Comp> { SameStart(now, n, past), SameMargin(now, n, past), RankedGames(now, n, past), rivalry, PollChange(now, n, past) };
    }

    // Read saved season games
    private static List<PastGame> SeasonGames(SortedDictionary<int, List<PastGame>> seasons, int year)
    {
        return seasons.ContainsKey(year) ? seasons[year] : new List<PastGame>();
    }

    // O(s x g): select eligible past seasons
    private static List<PastSeason> Past(SortedDictionary<int, List<PastGame>> seasons, int current, int n)
    {
        List<PastSeason> past = new List<PastSeason>();
        foreach (KeyValuePair<int, List<PastGame>> season in seasons.Reverse())
        {
            if (season.Key < current && season.Value.Count(g => g.IsFinal) >= Math.Max(1, n))
            {
                past.Add(new PastSeason(season.Value, FirstGames(season.Value, n), End(season.Key, season.Value)));
            }
        }
        return past;
    }

    // Create unavailable comparison
    private static Comp Empty(string key, string title, string why) => new Comp(key, title, why, new List<SeasonEnd>(), "", false);

    // O(s): select matching outcomes
    private static List<SeasonEnd> Matching(List<PastSeason> past, Func<PastSeason, bool> test) =>
        past.Where(test).Select(s => s.End).ToList();

    // Match opening record
    private static Comp SameStart(List<PastGame> now, int n, List<PastSeason> past)
    {
        int wins = FirstGames(now, n).Count(g => g.IsWin);
        // Check losses too; historical ties differ
        List<SeasonEnd> matches = Matching(past, s => s.First.Count(g => g.IsWin) == wins && s.First.Count(g => g.IsLoss) == n - wins);
        return new Comp("start", "Same start", $"{wins}-{n - wins} after {n} games", matches, $"same record after {n} games");
    }

    // Match average scoring margin
    private static Comp SameMargin(List<PastGame> now, int n, List<PastSeason> past)
    {
        double margin = FirstGames(now, n).Average(g => g.Margin);
        List<SeasonEnd> matches = Matching(past, s => Math.Abs(s.First.Average(g => g.Margin) - margin) <= MarginWindow);
        string words = margin >= 0 ? $"Won by {margin:0.0} a game" : $"Lost by {-margin:0.0} a game";
        return new Comp("margin", "Same scoring margin", words, matches, $"average margin within {MarginWindow:0} points after {n} games");
    }

    // Match ranked-opponent record
    private static Comp RankedGames(List<PastGame> now, int n, List<PastSeason> past)
    {
        List<PastGame> first = FirstGames(now, n);
        int wins = RankedCount(first, true);
        int losses = RankedCount(first, false);
        PastGame? best = BestRanked(first, true);
        string words = $"{wins}-{losses} vs ranked" + (best != null ? $", beat {best.RankedName}" : "");
        List<SeasonEnd> matches = Matching(past, s => RankedCount(s.First, true) == wins && RankedCount(s.First, false) == losses);
        return new Comp("ranked", "Vs. ranked teams", words, matches, $"same record vs AP top 25 after {n} games");
    }

    // O(g): count ranked-opponent results
    private static int RankedCount(List<PastGame> games, bool win) => games.Count(g => (win ? g.IsWin : g.IsLoss) && g.OppRank.HasValue);

    // O(g): select highest-ranked opponent result
    public static PastGame? BestRanked(List<PastGame> games, bool win)
    {
        PastGame? best = null;
        foreach (PastGame g in games)
        {
            if (g.IsFinal && g.IsWin == win && g.OppRank.HasValue && (best == null || g.OppRank < best.OppRank))
            {
                best = g;
            }
        }
        return best;
    }

    // Rival selection season window
    public const int RivalWindow = 25;

    // O(g): select most-played recent opponent
    public static string RivalId(List<PastGame> all, int current)
    {
        // Track opponent meetings
        Dictionary<string, int> meetings = new Dictionary<string, int>();
        Dictionary<string, DateTime> lastMet = new Dictionary<string, DateTime>();
        foreach (PastGame g in all.Where(g => g.Season < current && g.Season >= current - RivalWindow && g.IsFinal && g.OpponentId.Length > 0))
        {
            meetings[g.OpponentId] = meetings.ContainsKey(g.OpponentId) ? meetings[g.OpponentId] + 1 : 1;
            if (!lastMet.ContainsKey(g.OpponentId) || g.Date > lastMet[g.OpponentId])
            {
                lastMet[g.OpponentId] = g.Date;
            }
        }
        // Break frequency ties using latest meeting
        string rival = "";
        foreach (string id in meetings.Keys)
        {
            if (rival.Length == 0 || meetings[id] > meetings[rival] || (meetings[id] == meetings[rival] && lastMet[id] > lastMet[rival]))
            {
                rival = id;
            }
        }
        return rival;
    }

    // Match rivalry results
    private static Comp Rivalry(List<PastGame> all, List<PastGame> now, int current, string rivalId, List<PastSeason> past)
    {
        List<PastGame> series = all.FindAll(g => g.OpponentId == rivalId && g.Season < current && g.IsFinal);
        if (series.Count == 0)
        {
            return Empty("rival", "Rivalry", "No rival found yet");
        }
        string name = series[series.Count - 1].Opponent;
        int wins = series.Count(g => g.IsWin);
        string title = $"Rivalry: {name}";
        string note = $"series {wins}-{series.Count - wins} since {series.Min(g => g.Season)} · same result, first game each year";
        PastGame? game = now.Find(g => g.OpponentId == rivalId && g.IsFinal);
        if (game == null)
        {
            return new Comp("rival", title, $"{name} game not played yet", new List<SeasonEnd>(), note, false);
        }
        string words = (game.IsWin ? "Beat " : "Lost to ") + $"{name} {game.Points}-{game.OppPoints}";
        return new Comp("rival", title, words, Matching(past, s => RivalResult(s.Games, rivalId) == game.IsWin), note);
    }

    // Read first rivalry result
    private static bool? RivalResult(List<PastGame> season, string rivalId)
    {
        PastGame? game = season.Find(g => g.OpponentId == rivalId && g.IsFinal);
        if (game == null || (!game.IsWin && !game.IsLoss))
        {
            return null;
        }
        return game.IsWin;
    }

    // Match AP poll movement
    private static Comp PollChange(List<PastGame> now, int n, List<PastSeason> past)
    {
        int? before = now[0].Rank;
        int? after = RankAfter(now, n);
        string move = Move(before, after);
        string words = $"{RankText(before)} → {RankText(after)}";
        List<SeasonEnd> matches = Matching(past, s => Move(s.Games[0].Rank, RankAfter(s.Games, n)) == move);
        return new Comp("poll", "AP poll move", words, matches, MoveWords(move) + " since preseason");
    }

    // Read next game's AP rank
    public static int? RankAfter(List<PastGame> season, int n)
    {
        int finished = 0;
        // O(g): locate next game
        for (int i = 0; i < season.Count; i++)
        {
            if (finished == n)
            {
                return season[i].Rank;
            }
            finished += season[i].IsFinal ? 1 : 0;
        }
        if (season.Count == 0)
        {
            return null;
        }
        return season[season.Count - 1].Rank;
    }

    // Count eligible historical seasons
    public static int SeasonsCompared(List<PastGame> all, int current)
    {
        SortedDictionary<int, List<PastGame>> seasons = BySeason(all);
        int n = SeasonGames(seasons, current).Count(g => g.IsFinal);
        return Past(seasons, current, n).Count;
    }

    // O(g): count completed season games
    public static int GamesPlayed(List<PastGame> all, int current)
    {
        int played = 0;
        foreach (PastGame game in all)
        {
            if (game.Season == current && game.IsFinal)
            {
                played++;
            }
        }
        return played;
    }

    // Classify poll movement
    public static string Move(int? before, int? after)
    {
        if (before == null && after == null)
        {
            return "unranked";
        }
        int change = (before.HasValue ? before.Value : Unranked) - (after.HasValue ? after.Value : Unranked);
        if (change >= RankMove)
        {
            return "up";
        }
        return change <= -RankMove ? "down" : "same";
    }

    // Format AP rank
    public static string RankText(int? rank) => rank.HasValue ? $"#{rank.Value}" : "unranked";

    // Describe poll movement
    private static string MoveWords(string move)
    {
        if (move == "up" || move == "down")
        {
            return $"moved {move} 5+ spots";
        }
        return move == "same" ? "moved less than 5 spots" : "unranked both times";
    }

    // O(c x m): build historical verdict
    // Matching cards rank similar seasons
    // C8: count historical votes
    public static HistoryVerdict Decide(List<Comp> comps)
    {
        Dictionary<int, int> points = new Dictionary<int, int>();
        List<SeasonEnd> like = new List<SeasonEnd>();
        foreach (Comp comp in comps)
        {
            // Count matching cards
            foreach (SeasonEnd end in comp.Matches)
            {
                points[end.Season] = points.ContainsKey(end.Season) ? points[end.Season] + 1 : 1;
            }

            // Select same-start voters
            if (comp.Key == "start")
            {
                like.AddRange(comp.Matches);
            }
        }
        return new HistoryVerdict(Pick(like), like, 1, points);
    }

    // Select majority outcome; ties produce Cooked
    private static Finish Pick(List<SeasonEnd> like)
    {
        int lockedIn = 0;
        int cooked = 0;
        foreach (SeasonEnd season in like)
        {
            if (season.Finish == Finish.LockedIn)
            {
                lockedIn++;
            }
            else
            {
                cooked++;
            }
        }

        if (lockedIn > cooked)
        {
            return Finish.LockedIn;
        }
        return Finish.Cooked;
    }
}
