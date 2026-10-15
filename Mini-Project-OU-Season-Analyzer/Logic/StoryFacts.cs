using Caleb_Marks_OU_Season_Analyzer.Controls;
using Caleb_Marks_OU_Season_Analyzer.Data;
using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Logic;

// Build saved-data story
// Read four CSV sources
internal sealed class StoryFacts
{
    // Section sequence
    // C3: ordered clip list
    public static readonly string[] Order =
    {
        "cover", "lookalikes", "verdict", "staircase", "linkedlist", "turnovers", "losschanges", "comparerows",
        "firsttime", "replay", "herostat", "whattowatch", "queuewindow", "mergesort", "texasquiz", "rankmap",
        "nextup", "binarysearch", "recursion", "whatif", "recap", "filestorage",
    };

    // Stat labels
    private static readonly Dictionary<string, string> StatWords = new Dictionary<string, string>
    {
        { "pointsFor", "points a game" }, { "pointsAgainst", "points allowed a game" },
        { "passYdsAllowed", "pass yards allowed a game" }, { "totalYdsAllowed", "yards allowed a game" },
        { "thirdDownPctAllowed", "3rd downs allowed" }, { "yardsPerPlayAllowed", "yards a play allowed" },
        { "rushYdsAllowed", "rush yards allowed a game" }, { "sacksMade", "sacks" }, { "takeaways", "takeaways" },
        { "thirdDownPct", "3rd downs made" }, { "rushYpc", "yards a carry" }, { "yardsPerPlay", "yards a play" },
        { "giveaways", "giveaways" },
    };

    private readonly Team team;
    private readonly List<Game> games;
    private readonly int season;
    private readonly List<Game> played;
    private readonly Queue<Game> upcoming;
    private readonly WhatIfStack whatIfs;
    private readonly List<PastGame> history;
    private readonly List<PlayerStar> stars;
    private readonly List<TeamStatValue> fbs;
    private readonly SeasonOutlook outlook;
    private readonly HistoryVerdict verdict;
    private readonly int wins;
    private readonly int losses;

    // Initialize season facts
    public StoryFacts(Team team, List<Game> games, WhatIfStack whatIfs)
    {
        this.team = team;
        this.games = games;
        this.whatIfs = whatIfs;
        season = games.Count == 0 ? DateTime.Now.Year : games.Max(g => g.Season);
        played = games.Where(g => g.Season == season && g.IsFinal).ToList();
        wins = played.Count(g => g.IsWin);
        losses = played.Count - wins;
        upcoming = GameQueue.Upcoming(games, season);
        history = HistoryStore.Load(team.Id);
        stars = ExtrasStore.LoadStars(team.Id);
        fbs = ExtrasStore.LoadStats();
        outlook = Forecast.Outlook(games, upcoming, season, whatIfs);
        verdict = History.Decide(History.Build(history, season));
    }

    // Current season record
    public string Record => $"{wins}-{losses}";

    // Compare season records
    public string HeaderLine
    {
        get
        {
            int lastWins = games.Count(g => g.Season == season - 1 && g.IsWin);
            int lastLosses = games.Count(g => g.Season == season - 1 && g.IsFinal && !g.IsWin);
            return $"{season} season: {Record}   ·   {season - 1} final: {lastWins}-{lastLosses}";
        }
    }

    // Remaining regular-season games
    public List<Game> Left => outlook.Left;

    // Playoff-proxy record
    private string Bar => $"{outlook.SeasonGames - Forecast.PlayoffLosses}-{Forecast.PlayoffLosses}";

    // Playoff-proxy probability
    private double PlayoffPercent => Forecast.PlayoffChance(outlook.Totals, outlook.SeasonGames) * 100;

    // Forecast verdict
    public Finish Call => outlook.SeasonGames - outlook.MostLikely <= Forecast.PlayoffLosses ? Finish.LockedIn : Finish.Cooked;

    // Build available story sections
    // C3: build clip sections
    public List<StorySection> Build()
    {
        List<StorySection> sections = new List<StorySection>();
        foreach (string clip in Order)
        {
            StorySection? section = null;
            try
            {
                section = Make(clip);
            }
            catch (Exception error) when (error is InvalidOperationException || error is ArgumentException || error is NullReferenceException)
            {
                // Skip unavailable section
            }
            if (section != null)
            {
                section.Clip = clip;
                Game? game = ClipGame(clip);
                if (section.Players.Count == 0 && game != null)
                {
                    section.Players = Stars(game.EventId, section.Defense);
                }
                sections.Add(section);
            }
        }
        return sections;
    }

    // Clips & players

    // Find clip metadata
    private static ClipInfo? Info(string clip) => ClipLibrary.All.Find(c => c.Name == clip);

    // Match clip's completed game
    // Require matching date & opponent
    private Game? ClipGame(string clip)
    {
        ClipInfo? info = Info(clip);
        return info == null ? null : games.Find(g => g.IsFinal && g.Date.Date == info.Date.Date && g.Opponent == info.Opponent);
    }

    // Match historical clip game
    private PastGame? ClipPast(string clip)
    {
        ClipInfo? info = Info(clip);
        return info == null ? null : history.Find(h => h.IsFinal && h.Date.Date == info.Date.Date && h.Opponent == info.Opponent);
    }

    // Describe historical result
    private static string Describe(PastGame p) =>
        $"{(p.IsWin ? "W" : p.IsLoss ? "L" : "T")} {p.Points}-{p.OppPoints} {(p.Site == "Away" ? "at" : "vs")} {(p.OppRank.HasValue ? $"No. {p.OppRank} " : "")}{p.Opponent}";

    // Select three distinct leaders maximum
    private List<PlayerStar> Stars(string eventId, bool defense)
    {
        string[] categories = defense ? new[] { "totalTackles", "sacks", "interceptions" } : new[] { "passingYards", "rushingYards", "receivingYards" };
        List<PlayerStar> picked = new List<PlayerStar>();
        foreach (string category in categories)
        {
            PlayerStar? star = stars.Find(s => s.EventId == eventId && s.Category == category);
            if (star != null && !picked.Exists(p => p.AthleteId == star.AthleteId))
            {
                picked.Add(star);
            }
        }
        return picked;
    }

    // Format matchup label
    private static string Versus(Game g) => (g.IsAway ? "at " : "vs ") + g.Opponent;

    // Format ordinal number
    private static string Ordinal(int n) => n + (n % 100 is 11 or 12 or 13 ? "th" : (n % 10) switch { 1 => "st", 2 => "nd", 3 => "rd", _ => "th" });

    // Build selected section
    private StorySection? Make(string clip)
    {
        switch (clip)
        {
            case "cover": return Verdict();
            case "lookalikes": return SameStart();
            case "verdict": return MostAlike();
            case "staircase": return LastYear();
            case "linkedlist": return SoFar();
            case "turnovers": return Turnovers();
            case "losschanges": return WorstLoss();
            case "comparerows": return SameOpponent();
            case "firsttime": return BeatNumberOne();
            case "replay": return LatestGame();
            case "herostat": return Leaders();
            case "whattowatch": return OffenseRanks();
            case "queuewindow": return LastThree();
            case "mergesort": return PointsAllowed();
            case "texasquiz": return DefenseRanks();
            case "rankmap": return Pressure();
            case "nextup": return NextUp();
            case "binarysearch": return RoadAhead();
            case "recursion": return HowItEnds();
            case "whatif": return WhatIf();
            case "recap": return Totals();
            case "filestorage": return Files();
            default: return null;
        }
    }

    // Create story section
    private static StorySection Section(string kicker, string headline, params StoryStat[] stats) =>
        new StorySection { Kicker = kicker, Headline = headline, Stats = stats.ToList() };

    // Verdict & history

    // Summarize forecast verdict
    private StorySection Verdict()
    {
        Finish call = Call;
        // Explain verdict probability
        string headline = outlook.Left.Count == 0 ? $"Finished {outlook.Record}."
            : $"{PlayoffPercent:0}% chance of {Bar}, the usual playoff record.";
        if (verdict.Matches > 0 && verdict.Call != call)
        {
            headline += $" History leans the other way.";
        }
        StorySection section = Section("Locked in or cooked?", headline,
            new StoryStat($"{PlayoffPercent:0}%", $"chance OU finishes {Bar} or better"),
            new StoryStat(outlook.Record, $"most likely finish ({outlook.MostLikelyPercent:0}% chance)"));
        if (verdict.Matches > 0)
        {
            section.Stats.Add(new StoryStat($"{verdict.Votes(Finish.Cooked)} of {verdict.Matches}",
                $"past OU teams that started {Record} missed the playoff"));
        }
        section.Badge = call;
        PastGame? hat = ClipPast("cover");
        if (hat != null)
        {
            section.Note = call == Finish.Cooked && losses > 0
                ? $"The Golden Hat after {Describe(hat)}. One upset still leaves {losses} losses on the record."
                : $"The Golden Hat after {Describe(hat)}.";
        }
        return section;
    }

    // Compare matching season starts
    private StorySection? SameStart()
    {
        if (verdict.Matches == 0)
        {
            return null;
        }
        List<int> playoffYears = verdict.Like.Where(s => s.Playoff).Select(s => s.Season).OrderBy(y => y).ToList();
        string reached = playoffYears.Count == 0 ? "None reached the playoff."
            : $"{playoffYears.Count} reached the playoff ({string.Join(", ", playoffYears)}).";
        StorySection section = Section("Been here before?", $"{verdict.Matches} past OU teams started {Record}. {reached}",
            new StoryStat(verdict.Matches.ToString(), $"OU seasons since {HistoryStore.FirstSeason} that started {Record}"),
            new StoryStat($"{verdict.Votes(Finish.LockedIn)}", "made the playoff or a BCS bowl"),
            new StoryStat($"{verdict.Votes(Finish.Cooked)}", "missed the playoff"));

        // Chart historical season outcomes
        section.Chart = new StoryChart(ChartKind.Bars, $"How past {Record} starts finished");
        foreach (SeasonEnd s in verdict.Like.OrderBy(s => s.Season))
        {
            bool made = s.Finish == Finish.LockedIn;
            section.Chart.Bars.Add(new ChartBar(s.Season.ToString(), s.Wins, s.Record + (made ? " ★ playoff" : ""), made, !made));
        }
        PastGame? clip = ClipPast("lookalikes");
        SeasonEnd? same = clip == null ? null : verdict.Like.Find(s => s.Season == clip.Season);
        if (clip != null && same != null)
        {
            // Locate historical game position
            int gameNumber = history.Count(h => h.Season == clip.Season && !h.Postseason && h.Date <= clip.Date);
            section.Note = gameNumber <= played.Count
                ? $"{same.Season} also started {Record}; this was game {gameNumber}, {Describe(clip)}. It finished {same.Record}."
                : $"{same.Season} also started {Record} and finished {same.Record}. Here: {Describe(clip)}.";
        }
        return section;
    }

    // Show closest historical seasons
    private StorySection? MostAlike()
    {
        List<SeasonEnd> alike = verdict.MostAlike.Take(3).ToList();
        if (alike.Count == 0)
        {
            return null;
        }
        StorySection section = Section("Seasons like this", $"Most alike: {alike[0].Season}, which finished {alike[0].Record}.");
        foreach (SeasonEnd s in alike)
        {
            section.Stats.Add(new StoryStat(s.Record, $"{s.Season}: {(s.Bowl.Length > 0 ? s.Bowl : "no bowl")}"));
        }
        PastGame? clip = ClipPast("verdict");
        int match = clip == null ? -1 : verdict.MostAlike.FindIndex(s => s.Season == clip.Season);
        if (clip != null && match >= 0)
        {
            section.Note = $"{clip.Season}, the {(match == 0 ? "" : Ordinal(match + 1) + " ")}closest match: {Describe(clip)} on the way to {verdict.MostAlike[match].Record}.";
        }
        return section;
    }

    // Compare previous season
    private StorySection? LastYear()
    {
        List<Game> last = games.Where(g => g.Season == season - 1 && g.IsFinal).ToList();
        if (last.Count == 0 || played.Count == 0)
        {
            return null;
        }
        int n = played.Count;
        int startWins = last.Take(n).Count(g => g.IsWin);
        int endWins = last.Count(g => g.IsWin);
        StorySection section = Section("Last year", $"{season - 1} started {startWins}-{n - startWins} and finished {endWins}-{last.Count - endWins}.",
            new StoryStat($"{endWins}-{last.Count - endWins}", $"{season - 1} final record"),
            new StoryStat($"{startWins}-{n - startWins}", $"{season - 1} after {n} games"),
            new StoryStat(Record, $"{season} after {n} games"));
        PastGame? clip = ClipPast("staircase");
        if (clip != null && clip.Season == season - 1)
        {
            int gameNumber = last.Count(g => g.Date.Date <= clip.Date.Date);
            section.Note = $"{season - 1}, game {gameNumber}: {Describe(clip)}. That team finished {endWins}-{last.Count - endWins}.";
        }
        return section;
    }

    // Current season

    // Summarize linked game list
    private StorySection? SoFar()
    {
        GameList list = new GameList();
        foreach (Game g in played)
        {
            list.AddLast(g);
        }
        if (list.Head == null)
        {
            return null;
        }
        Game best = list.Head.Data;
        Game worst = list.Head.Data;
        for (GameNode? node = list.Head; node != null; node = node.Next)
        {
            int margin = node.Data.OUScore!.Value - node.Data.OppScore!.Value;
            best = margin > best.OUScore!.Value - best.OppScore!.Value ? node.Data : best;
            worst = margin < worst.OUScore!.Value - worst.OppScore!.Value ? node.Data : worst;
        }
        Game latest = list.Tail!.Data;
        StorySection section = Section("This season", $"{season} so far: {Record} after {list.Size} games.",
            new StoryStat(best.ResultText, $"biggest win, {best.Matchup}"),
            new StoryStat(worst.ResultText, $"worst loss, {worst.Matchup}"),
            new StoryStat(latest.ResultText, $"latest, {latest.Matchup} ({latest.Date:MMM d})"));

        // Chart chronological scoring margins
        section.Chart = new StoryChart(ChartKind.Split, "Points won or lost by, each game");
        for (GameNode? node = list.Head; node != null; node = node.Next)
        {
            Game g = node.Data;
            section.Chart.Bars.Add(new ChartBar(g.ShortName, g.OUScore!.Value - g.OppScore!.Value, g.ResultText, g == best, !g.IsWin));
        }
        Game? clip = ClipGame("linkedlist");
        if (clip != null)
        {
            section.Note = clip == best ? $"From the biggest win: {clip.ResultText} vs {clip.Opponent}, game {played.IndexOf(clip) + 1}."
                : $"Game {played.IndexOf(clip) + 1}: {clip.ResultText} {Versus(clip)}.";
        }
        return section;
    }

    // Compare giveaways
    private StorySection? Turnovers()
    {
        List<Game> known = played.Where(g => g.Giveaways.HasValue).ToList();
        if (!known.Exists(g => g.IsWin) || !known.Exists(g => !g.IsWin))
        {
            return null;
        }
        double inWins = known.Where(g => g.IsWin).Average(g => g.Giveaways!.Value);
        double inLosses = known.Where(g => !g.IsWin).Average(g => g.Giveaways!.Value);
        StorySection section = Section("What decides it",
            $"Turnovers decide it: {inLosses:0.#} giveaways a game in losses, {inWins:0.#} in wins.",
            new StoryStat($"{inLosses:0.#}", "giveaways a game in losses"),
            new StoryStat($"{inWins:0.#}", "giveaways a game in wins"));
        AddRank(section, "giveaways", "in fewest giveaways");

        // Chart game giveaways
        section.Chart = new StoryChart(ChartKind.Bars, "Giveaways each game");
        foreach (Game g in known)
        {
            section.Chart.Bars.Add(new ChartBar(g.ShortName, g.Giveaways!.Value, $"{(g.IsWin ? "W" : "L")} · {g.Giveaways}", false, !g.IsWin));
        }
        Game? clip = ClipGame("turnovers");
        if (clip != null && clip.Giveaways.HasValue)
        {
            section.Note = $"One of OU's {clip.Giveaways} giveaways in the {clip.ResultText} {(clip.IsWin ? "win" : "loss")} {Versus(clip)}.";
        }
        return section;
    }

    // Describe highest-giveaway loss
    private StorySection? WorstLoss()
    {
        Game? loss = played.Where(g => !g.IsWin && g.Giveaways.HasValue).OrderByDescending(g => g.Giveaways).FirstOrDefault();
        if (loss == null)
        {
            return null;
        }
        PastGame? row = history.Find(h => h.Date.Date == loss.Date.Date);
        StorySection section = Section("Where it went wrong",
            $"{loss.Opponent}: {loss.Giveaways} giveaways in a {loss.OUScore}-{loss.OppScore} loss.",
            new StoryStat(loss.Giveaways!.Value.ToString(), $"giveaways {loss.Matchup}"),
            new StoryStat(loss.OppScore!.Value.ToString(),
                played.Max(g => g.OppScore) == loss.OppScore ? "points allowed, the most this season" : "points allowed"));
        if (row != null && row.OppRank.HasValue)
        {
            section.Stats.Add(new StoryStat($"No. {row.OppRank}", $"{loss.Opponent}'s rank that day"));
        }
        if (ClipGame("losschanges") == loss)
        {
            section.Note = $"One of the {loss.Giveaways} giveaways {Versus(loss)}, turned into {loss.Opponent} points.";
        }
        return section;
    }

    // Compare repeat opponents using binary search
    private StorySection? SameOpponent()
    {
        Game[] byName = BinarySearch.SortByName(games);
        foreach (Game now in played)
        {
            Game? before = BinarySearch.Find(byName, now.Opponent).Find(g => g.Season == season - 1 && g.IsFinal);
            if (before != null && now.OUYards.HasValue && before.OUYards.HasValue)
            {
                StorySection section = Section("Same team, a year apart",
                    $"{now.Opponent}: {before.ResultText} in {season - 1}, {now.ResultText} in {season}.",
                    new StoryStat(before.ResultText, $"{season - 1}, {before.Matchup}"),
                    new StoryStat(now.ResultText, $"{season}, {now.Matchup}"),
                    new StoryStat($"{before.OUYards} → {now.OUYards}", "OU yards, then vs now"));
                section.Chart = new StoryChart(ChartKind.Bars, $"OU yards vs {now.Opponent}");
                section.Chart.Bars.Add(new ChartBar(season - 1 + "", before.OUYards.Value, $"{before.OUYards} ({before.ResultText})", before.IsWin, !before.IsWin));
                section.Chart.Bars.Add(new ChartBar(season + "", now.OUYards!.Value, $"{now.OUYards} ({now.ResultText})", now.IsWin, !now.IsWin));
                Game? clip = ClipGame("comparerows");
                if (clip != null && (clip == before || clip == now))
                {
                    section.Note = $"The {clip.Season} meeting, {clip.ResultText} {Versus(clip)}: OU gained {clip.OUYards} yards that day.";
                }
                return section;
            }
        }
        return null;
    }

    // Show top-ranked upsets
    private StorySection? BeatNumberOne()
    {
        List<PastGame> upsets = history.Where(h => h.OppRank == 1 && h.IsWin).OrderByDescending(h => h.Date).ToList();
        if (upsets.Count == 0 || upsets[0].Season != season)
        {
            return null;
        }
        PastGame latest = upsets[0];
        StorySection section = Section("The upset", $"Beat No. 1 {latest.Opponent} {latest.Points}-{latest.OppPoints}.",
            new StoryStat($"{latest.Points}-{latest.OppPoints}", $"over No. 1 {latest.Opponent}, {latest.Date:MMM d}"),
            new StoryStat(upsets.Count.ToString(), $"OU wins over a No. 1 team since {HistoryStore.FirstSeason}"));
        if (upsets.Count > 1)
        {
            PastGame before = upsets[1];
            section.Headline += $" First win over a No. 1 team since {before.Season}.";
            section.Stats.Add(new StoryStat(before.Season.ToString(), $"last time before: {before.Opponent} {before.Points}-{before.OppPoints}"));
        }
        section.Defense = true;
        if (ClipPast("firsttime") == latest)
        {
            section.Note = $"A takeaway in the {Describe(latest)} upset.";
        }
        return section;
    }

    // Summarize latest available box score
    private StorySection? LatestGame()
    {
        Game? g = played.LastOrDefault(x => x.HasStats && x.Giveaways.HasValue);
        if (g == null)
        {
            return null;
        }
        int clean = played.Count(x => x.Giveaways == 0);
        int margin = Math.Abs(g.OUScore!.Value - g.OppScore!.Value);
        StorySection section = Section("Inside the latest game", $"{g.Opponent}: {g.ResultText}, decided by {margin} points.",
            new StoryStat($"{g.OUYards} - {g.OppYards}", $"yards, OU vs {g.Opponent}"),
            new StoryStat($"{g.OU3rdMade} of {g.OU3rdAtt}", "OU 3rd downs made"),
            new StoryStat(g.Giveaways!.Value.ToString(), $"giveaways ({clean} of {played.Count} games with none)"));
        section.Chart = new StoryChart(ChartKind.Bars, $"Total yards, OU vs {g.Opponent}");
        section.Chart.Bars.Add(new ChartBar("OU", g.OUYards!.Value, g.OUYards.Value.ToString(), true));
        section.Chart.Bars.Add(new ChartBar(g.Opponent, g.OppYards!.Value, g.OppYards.Value.ToString()));
        if (ClipGame("replay") == g)
        {
            section.Note = g.IsWin && g.OppYards > g.OUYards
                ? $"The last play {Versus(g)}: outgained by {g.OppYards - g.OUYards} yards, OU still won by {margin}."
                : $"From the {g.ResultText} game {Versus(g)}.";
        }
        return section;
    }

    // Show season leaders
    private StorySection? Leaders()
    {
        List<PlayerStar> leaders = stars.Where(s => s.EventId == "" &&
            (s.Category == "passingYards" || s.Category == "rushingYards" || s.Category == "receivingYards")).ToList();
        if (leaders.Count == 0)
        {
            return null;
        }
        StorySection section = Section("Who carries it", $"{leaders[0].Name}: {leaders[0].Display}.");
        foreach (PlayerStar star in leaders)
        {
            section.Stats.Add(new StoryStat(star.Value.ToString("N0"), $"{star.Label.ToLowerInvariant()}, {star.Name}"));
        }
        section.Players = Stars("", false);
        Game? clip = ClipGame("herostat");
        if (clip != null)
        {
            section.Note = $"{leaders[0].Name}, OU's {leaders[0].Label.ToLowerInvariant()} leader, in the {clip.ResultText} game {Versus(clip)}.";
        }
        return section;
    }

    // National rankings

    // Sort selected stat rankings
    private List<TeamStatValue> Ranks(params string[] keys)
    {
        List<TeamStatValue> mine = fbs.Where(r => r.TeamId == team.Id && r.Rank > 0 && keys.Contains(r.Key)).ToList();
        MergeSort.Sort(mine, (a, b) => a.Rank.CompareTo(b.Rank));
        return mine;
    }

    // Ranked team count
    private int FbsCount => fbs.Count(r => r.Key == "pointsFor");

    // Format ranked statistic
    private StoryStat RankStat(TeamStatValue r) => new StoryStat($"#{r.Rank}", $"{StatWords[r.Key]} ({Show(r)}), of {FbsCount}");

    // Format stat value
    private static string Show(TeamStatValue r) => r.Key.StartsWith("thirdDown", StringComparison.Ordinal) ? $"{r.Value:0.#}%" : $"{r.Value:0.#}";

    // Append available ranking
    private void AddRank(StorySection section, string key, string words)
    {
        TeamStatValue? r = Ranks(key).FirstOrDefault();
        if (r != null)
        {
            section.Stats.Add(new StoryStat($"#{r.Rank}", $"of {FbsCount} FBS teams {words} ({Show(r)})"));
        }
    }

    // Show weakest offense rankings
    private StorySection? OffenseRanks()
    {
        List<TeamStatValue> worst = Ranks("thirdDownPct", "rushYpc", "pointsFor", "yardsPerPlay");
        worst.Reverse();
        if (worst.Count == 0)
        {
            return null;
        }
        StorySection section = Section("The offense", $"The offense is the worry: #{worst[0].Rank} of {FbsCount} in {StatWords[worst[0].Key]}.");
        foreach (TeamStatValue r in worst.Take(3))
        {
            section.Stats.Add(RankStat(r));
        }
        section.Chart = RankChart("Offense: OU's national rank (left = best)", worst);
        section.Players = Stars("", false);
        Game? clip = ClipGame("whattowatch");
        TeamStatValue? carry = Ranks("rushYpc").FirstOrDefault();
        if (clip != null && carry != null)
        {
            section.Note = $"A rare long run, {Versus(clip)}: OU averages {Show(carry)} yards a carry, #{carry.Rank} of {FbsCount}.";
        }
        return section;
    }

    // Build national-rank chart
    private StoryChart RankChart(string title, List<TeamStatValue> ranks)
    {
        StoryChart chart = new StoryChart(ChartKind.Rank, title) { Of = FbsCount };
        foreach (TeamStatValue r in ranks)
        {
            chart.Bars.Add(new ChartBar(StatWords[r.Key], r.Rank, $"#{r.Rank}", r.Rank <= FbsCount / 4, r.Rank > FbsCount * 3 / 4));
        }
        return chart;
    }

    // Show strongest defense rankings
    private StorySection? DefenseRanks()
    {
        List<TeamStatValue> best = Ranks("pointsAgainst", "passYdsAllowed", "totalYdsAllowed", "thirdDownPctAllowed", "yardsPerPlayAllowed", "rushYdsAllowed");
        if (best.Count == 0)
        {
            return null;
        }
        StorySection section = Section("The defense", $"The defense is elite: #{best[0].Rank} of {FbsCount} in {StatWords[best[0].Key]}.");
        foreach (TeamStatValue r in best.Take(3))
        {
            section.Stats.Add(RankStat(r));
        }
        section.Chart = RankChart("Defense: OU's national rank (left = best)", best);
        section.Defense = true;
        section.Players = Stars("", true);
        Game? clip = ClipGame("texasquiz");
        TeamStatValue? third = Ranks("thirdDownPctAllowed").FirstOrDefault();
        if (clip != null && third != null)
        {
            section.Note = $"A 3rd-down stop {Versus(clip)}: opponents convert only {Show(third)} of 3rd downs, #{third.Rank} of {FbsCount}.";
        }
        return section;
    }

    // Summarize defensive pressure
    private StorySection? Pressure()
    {
        List<TeamStatValue> r = Ranks("sacksMade", "takeaways");
        if (r.Count == 0)
        {
            return null;
        }
        TeamStatValue? sacks = r.Find(x => x.Key == "sacksMade");
        StorySection section = Section("Pressure", sacks != null ? $"{sacks.Value:0} sacks in {played.Count} games, #{sacks.Rank} of {FbsCount}."
            : $"#{r[0].Rank} in {StatWords[r[0].Key]}.");
        foreach (TeamStatValue x in r)
        {
            section.Stats.Add(new StoryStat($"{x.Value:0}", $"{StatWords[x.Key]}, #{x.Rank} of {FbsCount}"));
        }
        section.Chart = RankChart("Pressure: OU's national rank (left = best)", r);
        section.Defense = true;
        Game? clip = ClipGame("rankmap");
        if (clip != null && sacks != null)
        {
            section.Note = $"One of OU's {sacks.Value:0} sacks, in the {clip.ResultText} game {Versus(clip)}.";
        }
        return section;
    }

    // Sort scoring defense using merge sort
    private StorySection? PointsAllowed()
    {
        List<Game> sorted = new List<Game>(played);
        MergeSort.Sort(sorted, (a, b) => a.OppScore!.Value.CompareTo(b.OppScore!.Value));
        if (sorted.Count < 3)
        {
            return null;
        }
        int held = sorted.Count(g => g.OppScore <= 17);
        StorySection section = Section("Fewest points allowed", $"The defense held {held} of {sorted.Count} opponents to 17 points or fewer.");
        foreach (Game g in sorted.Take(3))
        {
            section.Stats.Add(new StoryStat(g.OppScore!.Value.ToString(), $"points allowed, {g.Matchup} ({g.ResultText})"));
        }

        // Chart sorted defensive results
        Game? clip = ClipGame("mergesort");
        section.Chart = new StoryChart(ChartKind.Bars, "Points allowed, fewest first");
        foreach (Game g in sorted)
        {
            section.Chart.Bars.Add(new ChartBar(g.ShortName, g.OppScore!.Value, $"{g.OppScore} ({(g.IsWin ? "W" : "L")})", g == clip, !g.IsWin));
        }
        section.Defense = true;
        if (clip != null)
        {
            int place = sorted.IndexOf(clip);
            section.Note = $"{clip.Opponent} scored {clip.OppScore}, the {(place == 0 ? "" : Ordinal(place + 1) + "-")}fewest points OU allowed this season.";
        }
        return section;
    }

    // Compare three-game windows
    private StorySection? LastThree()
    {
        List<TrendPoint> points = GameQueue.ThreeGameAverages(games, season).Where(p => p.YppAvg3.HasValue).ToList();
        if (points.Count == 0)
        {
            return null;
        }
        double first = points[0].YppAvg3!.Value;
        double latest = points[^1].YppAvg3!.Value;
        string trend = latest >= 0 ? $"Last 3 games: OU gained {latest:0.0} more yards a play than its opponents."
            : $"Last 3 games: opponents gained {-latest:0.0} more yards a play than OU.";
        StorySection section = Section("Trending", trend,
            new StoryStat($"{latest:+0.0;-0.0;0.0}", "yards a play vs opponents, last 3 games"),
            new StoryStat($"{first:+0.0;-0.0;0.0}", "yards a play vs opponents, first 3 games"));
        AddRank(section, "yardsPerPlay", "in yards a play");

        // Chart game efficiency margins
        List<TrendPoint> all = GameQueue.ThreeGameAverages(games, season).Where(p => p.YppMargin.HasValue).ToList();
        section.Chart = new StoryChart(ChartKind.Split, "Yards a play edge each game (last 3 in bold)");
        for (int i = 0; i < all.Count; i++)
        {
            double edge = all[i].YppMargin!.Value;
            section.Chart.Bars.Add(new ChartBar(all[i].Game.ShortName, edge, $"{edge:+0.0;-0.0;0.0}", i >= all.Count - 3, edge < 0));
        }
        Game? clip = ClipGame("queuewindow");
        int at = clip == null ? -1 : played.IndexOf(clip);
        if (clip != null && at >= played.Count - 3)
        {
            section.Note = $"{clip.Opponent} ({clip.ResultText}) is the {Ordinal(at - (played.Count - 3) + 1)} of the last 3 games.";
        }
        return section;
    }

    // Season outlook

    // Describe next opponent
    private StorySection? NextUp()
    {
        if (upcoming.Count == 0)
        {
            return null;
        }
        Game next = upcoming.Peek();
        PastGame? row = history.Find(h => h.Season == season && h.OpponentId == next.OpponentId);
        int met = history.Count(h => h.OpponentId == next.OpponentId && h.IsFinal);
        string chance = next.WinPct.HasValue ? $" ESPN gives OU {next.WinPct:0}%." : "";
        StorySection section = Section("Next up", $"Next: {next.Matchup}, {next.Date:ddd MMM d}.{chance}");
        if (next.WinPct.HasValue)
        {
            section.Stats.Add(new StoryStat($"{next.WinPct:0}%", "OU win chance (ESPN)"));
        }
        if (row != null && row.OppRank.HasValue)
        {
            section.Stats.Add(new StoryStat($"No. {row.OppRank}", $"{next.Opponent}'s rank"));
        }
        section.Stats.Add(new StoryStat(met.ToString(), $"games vs {next.Opponent} since {HistoryStore.FirstSeason}"));
        ClipInfo? clip = Info("nextup");
        if (clip != null && clip.Opponent == next.Opponent)
        {
            section.Note = met == 0 ? $"No games vs {next.Opponent} since {HistoryStore.FirstSeason}, so this is from {clip.Date.Year}."
                : $"OU vs {next.Opponent} back in {clip.Date.Year}.";
        }
        return section;
    }

    // Compare remaining schedule
    private StorySection? RoadAhead()
    {
        List<Game> left = outlook.Left.Where(g => g.WinPct.HasValue).ToList();
        if (left.Count == 0)
        {
            return null;
        }
        int ranked = outlook.Left.Count(g => history.Exists(h => h.Season == season && h.OpponentId == g.OpponentId && h.OppRank.HasValue));
        Game toughest = left.OrderBy(g => g.WinPct).First();
        Game easiest = left.OrderByDescending(g => g.WinPct).First();
        StorySection section = Section("The road ahead", $"{outlook.Left.Count} games left, {ranked} against ranked teams.",
            new StoryStat(ranked.ToString(), $"ranked opponents in {outlook.Left.Count} games left"),
            new StoryStat($"{toughest.WinPct:0}%", $"toughest: {toughest.Matchup}"),
            new StoryStat($"{easiest.WinPct:0}%", $"easiest: {easiest.Matchup}"));

        // Chart remaining win probabilities
        section.Chart = new StoryChart(ChartKind.Bars, "ESPN chance OU wins, games left");
        foreach (Game g in left)
        {
            section.Chart.Bars.Add(new ChartBar(g.ShortName, g.WinPct!.Value, $"{g.WinPct:0}%", g == left[0], g.WinPct < 50));
        }
        ClipInfo? clip = Info("binarysearch");
        if (clip != null && clip.GameLine.Contains(left[0].Opponent))
        {
            section.Note = $"Scouting {left[0].Opponent}, the first of {outlook.Left.Count} games left: its {clip.Date:MMM d} game vs {clip.Opponent}.";
        }
        return section;
    }

    // Describe likely season finish
    private StorySection? HowItEnds()
    {
        if (outlook.Left.Count == 0)
        {
            return null;
        }
        StorySection section = Section("How it ends", $"Most likely finish: {outlook.Record} ({outlook.MostLikelyPercent:0}% chance).",
            new StoryStat(outlook.Record, "most likely final record"),
            new StoryStat($"{outlook.BowlPercent:0}%", "chance of a bowl (6+ wins)"),
            new StoryStat($"{PlayoffPercent:0}%", $"chance of {Bar} or better"));
        section.Chart = RecordChart();
        section.Note = AheadNote("recursion");
        return section;
    }

    // Chart final-record probabilities
    // Include outcomes reaching 1%
    private StoryChart RecordChart()
    {
        StoryChart chart = new StoryChart(ChartKind.Bars, "Chance of each final record");
        for (int w = outlook.Totals.Length - 1; w >= 0; w--)
        {
            double percent = outlook.Totals[w] * 100;
            if (percent >= 1)
            {
                chart.Bars.Add(new ChartBar($"{w}-{outlook.SeasonGames - w}", percent, $"{percent:0}%", w == outlook.MostLikely, w < 6));
            }
        }
        return chart;
    }

    // Explain upcoming-opponent clip
    private string AheadNote(string clip)
    {
        PastGame? past = ClipPast(clip);
        Game? ahead = past == null ? null : outlook.Left.Find(g => g.OpponentId == past.OpponentId);
        if (past == null || ahead == null)
        {
            return "";
        }
        string chance = ahead.WinPct.HasValue ? $" (ESPN gives OU {ahead.WinPct:0}%)" : "";
        bool toughest = outlook.Left.Where(g => g.WinPct.HasValue).OrderBy(g => g.WinPct).FirstOrDefault() == ahead;
        return $"{ahead.Opponent}{(toughest ? ", the toughest game left," : "")} is still ahead{chance}. In {past.Season}: {Describe(past)}.";
    }

    // Describe hypothetical forecast
    private StorySection? WhatIf()
    {
        if (outlook.Left.Count == 0)
        {
            return null;
        }
        string headline = whatIfs.Count == 0 ? "Your call: pick any game left as a win or a loss."
            : $"With your picks: {outlook.Record}, {PlayoffPercent:0}% chance of {Bar}.";
        StorySection section = Section("What if?", headline,
            new StoryStat(outlook.Record, "most likely finish"),
            new StoryStat($"{PlayoffPercent:0}%", $"chance of {Bar}"));
        section.WhatIf = true;
        section.Note = AheadNote("whatif");
        return section;
    }

    // Summary

    // Summarize season scoring
    private StorySection? Totals()
    {
        if (played.Count == 0)
        {
            return null;
        }
        int scored = played.Sum(g => g.OUScore!.Value);
        int allowed = played.Sum(g => g.OppScore!.Value);
        StorySection section = Section("The season in one line", $"{season}: {scored} points scored, {allowed} allowed in {played.Count} games.");
        foreach (TeamStatValue r in Ranks("pointsFor", "pointsAgainst"))
        {
            section.Stats.Add(new StoryStat($"{r.Value:0.#}", $"{StatWords[r.Key]}, #{r.Rank} of {FbsCount}"));
        }
        section.Stats.Add(new StoryStat($"{wins} - {losses}", "wins - losses"));

        // Chart scoring differences
        section.Chart = new StoryChart(ChartKind.Split, "Points scored minus allowed, each game");
        foreach (Game g in played)
        {
            section.Chart.Bars.Add(new ChartBar(g.ShortName, g.OUScore!.Value - g.OppScore!.Value, $"{g.OUScore}-{g.OppScore}", false, !g.IsWin));
        }
        PastGame? clip = ClipPast("recap");
        if (clip != null && clip.Season == season)
        {
            section.Note = $"From the {Describe(clip)} game, game {played.FindIndex(g => g.Date.Date == clip.Date.Date) + 1} of {played.Count}.";
        }
        return section;
    }

    // Describe saved data sources
    private StorySection Files()
    {
        int pastSeasons = history.Select(h => h.Season).Distinct().Count();
        StorySection section = Section("Where the numbers come from", "Every number here comes from 4 saved CSV files. No database.",
            new StoryStat(games.Count.ToString(), $"games in games.csv ({games.Min(g => g.Season)}-{season})"),
            new StoryStat(history.Count.ToString("N0"), $"games in history-{team.Id}.csv ({pastSeasons} seasons)"),
            new StoryStat(FbsCount.ToString(), "FBS teams ranked in fbs-stats.csv"));
        Game? clip = ClipGame("filestorage");
        if (clip != null && clip == played.LastOrDefault())
        {
            section.Note = $"The newest final in games.csv: OU {clip.OUScore}, {clip.Opponent} {clip.OppScore} ({clip.Date:MMM d}).";
        }
        return section;
    }
}
