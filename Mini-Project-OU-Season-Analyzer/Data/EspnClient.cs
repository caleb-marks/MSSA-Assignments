using System.Globalization;
using System.Net;
using System.Text.Json;
using Caleb_Marks_OU_Season_Analyzer.Logic;
using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Data;

// Download Oklahoma game data
internal static class EspnClient
{
    // Default ESPN team identifier
    public const string OklahomaId = "201";
    public static readonly int[] Seasons = { 2025, 2026 };

    private const string SiteApi = "https://site.api.espn.com/apis/site/v2/sports/football/college-football/";
    private const string CoreApi = "https://sports.core.api.espn.com/v2/sports/football/leagues/college-football/seasons/";
    private const string LogoApi = "https://a.espncdn.com/combiner/i?img=/i/teamlogos/ncaa/500/";
    private const DateTimeStyles AsUtc = DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal;

    // Retrying client; 20-second timeout
    private static readonly HttpClient http = new HttpClient(new RetryHandler()) { Timeout = TimeSpan.FromSeconds(20) };
    private static readonly TimeZoneInfo central = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

    // Update saved game data
    public static async Task<UpdateResult> UpdateCsvAsync(string path, string teamId, IProgress<string>? progress)
    {
        // Reject unreadable saved data
        List<string> loadProblems = new List<string>();
        List<Game> saved = GameCsv.Load(path, loadProblems);
        if (File.Exists(path) && loadProblems.Count > 0)
        {
            return new UpdateResult(false, "games.csv has lines the app cannot read, so it was not updated. Fix or delete them first. "
                + string.Join(" ", loadProblems.Take(3)));
        }

        List<Game> games = new List<Game>();
        List<string> skipped = new List<string>();
        try
        {
            await ReadSchedulesAsync(teamId, saved, games, skipped, progress);
        }
        catch (Exception error) when (IsDataError(error))
        {
            return new UpdateResult(false, "ESPN could not be reached, so the app kept your last saved games.csv. (" + RetryHandler.Reason(error) + ")");
        }

        // Preserve saved games when response empty
        if (games.Count == 0 && saved.Count > 0)
        {
            return new UpdateResult(false, "ESPN sent no readable games, so the app kept your last saved games.csv.");
        }

        Dictionary<string, Game> savedById = new Dictionary<string, Game>();
        saved.ForEach(g => savedById[g.EventId] = g);
        int keptUnreadable = KeepUnreadable(games, skipped, savedById);
        games.Sort((a, b) => a.Date.CompareTo(b.Date));
        int failed = await ReadDetailsAsync(games, savedById, teamId, progress);

        // Save games first
        GameCsv.Save(path, games);
        progress?.Report("Saving team logos...");
        await SaveLogosAsync(new string[] { teamId }.Concat(games.Select(g => g.OpponentId)));
        string kept = failed > 0 ? " " + failed + " game(s) had no readable details, so their saved box scores were kept." : "";
        if (keptUnreadable > 0)
        {
            kept += " " + keptUnreadable + " game(s) had unreadable ESPN schedule data, so their saved rows were kept.";
        }
        return new UpdateResult(true, "Updated " + games.Count + " games from ESPN." + kept);
    }

    // Download season schedules concurrently
    private static async Task ReadSchedulesAsync(string teamId, List<Game> saved, List<Game> games, List<string> skipped,
        IProgress<string>? progress)
    {
        progress?.Report("Getting " + string.Join(" & ", Seasons) + " schedules...");
        List<Task<List<Game>>> downloads = new List<Task<List<Game>>>();
        List<List<string>> unreadable = new List<List<string>>();
        foreach (int season in Seasons)
        {
            for (int type = 2; type <= 3; type++)
            {
                unreadable.Add(new List<string>());
                downloads.Add(ReadScheduleAsync(teamId, season, type, unreadable[unreadable.Count - 1]));
            }
        }

        for (int i = 0; i < downloads.Count; i++)
        {
            List<Game> found = await downloads[i];
            int season = Seasons[i / 2];
            bool bowls = i % 2 == 1;
            // Preserve missing schedule rows
            if (found.Count == 0)
            {
                found = saved.Where(g => g.Season == season && g.IsBowl == bowls).Select(g => g.Copy()).ToList();
            }
            games.AddRange(found);
            skipped.AddRange(unreadable[i]);
        }
    }

    // O(n + s): restore unreadable events
    private static int KeepUnreadable(List<Game> games, List<string> skipped, Dictionary<string, Game> savedById)
    {
        HashSet<string> freshIds = new HashSet<string>(games.Select(g => g.EventId));
        int kept = 0;
        foreach (string id in skipped)
        {
            if (savedById.ContainsKey(id) && freshIds.Add(id))
            {
                games.Add(savedById[id].Copy());
                kept++;
            }
        }
        return kept;
    }

    // Download game details, eight concurrently
    private static async Task<int> ReadDetailsAsync(List<Game> rows, Dictionary<string, Game> savedById, string teamId, IProgress<string>? progress)
    {
        // Share weekly FPI requests
        Dictionary<string, Task<double?>> fpi = new Dictionary<string, Task<double?>>();
        foreach (Game g in rows)
        {
            if (g.IsFinal && !fpi.ContainsKey(g.Season + "-" + g.Week))
            {
                fpi[g.Season + "-" + g.Week] = ReadFpiAsync(teamId, g.Season, g.Week);
            }
        }

        int failed = 0;
        int done = 0;
        progress?.Report("Reading games: 0 of " + rows.Count + "...");
        await Parallel.ForAsync(0, rows.Count, new ParallelOptions { MaxDegreeOfParallelism = 8 }, async (i, _) =>
        {
            Game game = rows[i];
            Game? old = savedById.GetValueOrDefault(game.EventId);
            try
            {
                ParseSummary(game, await http.GetStringAsync(SiteApi + "summary?event=" + game.EventId).ConfigureAwait(false), teamId);
                if (game.IsFinal)
                {
                    game.FPI = await fpi[game.Season + "-" + game.Week];
                }
                KeepSavedValues(game, old);
            }
            catch (Exception error) when (IsDataError(error))
            {
                // Preserve unavailable details
                Interlocked.Increment(ref failed);
                KeepSavedValues(game, old);
            }
            progress?.Report("Reading games: " + Interlocked.Increment(ref done) + " of " + rows.Count + "...");
        });
        return failed;
    }

    // Classify download errors
    public static bool IsDataError(Exception error) =>
        error is HttpRequestException || error is TaskCanceledException || error is JsonException || error is KeyNotFoundException
        || error is InvalidOperationException || error is IndexOutOfRangeException || error is FormatException;

    // Fill missing values using saved game
    internal static void KeepSavedValues(Game game, Game? old)
    {
        if (old == null)
        {
            return;
        }
        if (!game.WinPct.HasValue && old.WinPct.HasValue)
        {
            game.WinPct = old.WinPct;
            game.OddsAsOf = old.OddsAsOf;
        }
        game.FPI = game.FPI.HasValue ? game.FPI : old.FPI;
        game.OpponentColor = FillBlank(game.OpponentColor, old.OpponentColor);
        game.OpponentAltColor = FillBlank(game.OpponentAltColor, old.OpponentAltColor);
        if (!game.IsCompleted)
        {
            return;
        }

        // Restore missing completed-game stats
        game.OUYards = Fill(game.OUYards, old.OUYards);
        game.OUPlays = Fill(game.OUPlays, old.OUPlays);
        game.OppYards = Fill(game.OppYards, old.OppYards);
        game.OppPlays = Fill(game.OppPlays, old.OppPlays);
        game.OURushYds = Fill(game.OURushYds, old.OURushYds);
        game.OURushAtt = Fill(game.OURushAtt, old.OURushAtt);
        game.OUPassYds = Fill(game.OUPassYds, old.OUPassYds);
        game.OppRushYds = Fill(game.OppRushYds, old.OppRushYds);
        game.OppRushAtt = Fill(game.OppRushAtt, old.OppRushAtt);
        game.OU3rdMade = Fill(game.OU3rdMade, old.OU3rdMade);
        game.OU3rdAtt = Fill(game.OU3rdAtt, old.OU3rdAtt);
        game.OUInt = Fill(game.OUInt, old.OUInt);
        game.OUFumLost = Fill(game.OUFumLost, old.OUFumLost);
        game.FirstDrive = FillBlank(game.FirstDrive, old.FirstDrive);
    }

    // Prefer fresh number
    private static int? Fill(int? fresh, int? saved) => fresh.HasValue ? fresh : saved;

    // Prefer nonblank fresh text
    private static string FillBlank(string fresh, string saved) => string.IsNullOrWhiteSpace(fresh) ? saved : fresh;

    // Download one schedule
    private static async Task<List<Game>> ReadScheduleAsync(string teamId, int season, int seasonType, List<string> skipped)
    {
        string url = FormattableString.Invariant($"{SiteApi}teams/{teamId}/schedule?season={season}&seasontype={seasonType}");
        return ParseSchedule(await http.GetStringAsync(url).ConfigureAwait(false), season, seasonType, skipped, teamId);
    }

    // Parse schedule events
    internal static List<Game> ParseSchedule(string json, int season, int seasonType, List<string>? skipped = null,
        string teamId = OklahomaId)
    {
        using JsonDocument doc = JsonDocument.Parse(json);
        JsonElement events = Child(doc.RootElement, "events");
        if (events.ValueKind != JsonValueKind.Array)
        {
            // Reject missing events array
            throw new JsonException("ESPN " + season + " schedule had no events list.");
        }

        List<Game> games = new List<Game>();
        foreach (JsonElement ev in events.EnumerateArray())
        {
            // Isolate unreadable events
            Game? game = null;
            try
            {
                game = ParseEvent(ev, season, seasonType, teamId);
            }
            catch (Exception error) when (IsDataError(error))
            {
                // Record skipped event
            }
            if (game != null)
            {
                games.Add(game);
            }
            else if (Text(ev, "id").Length > 0 && skipped != null)
            {
                skipped.Add(Text(ev, "id"));
            }
        }
        return games;
    }

    // Parse one schedule event
    private static Game? ParseEvent(JsonElement ev, int season, int seasonType, string teamId)
    {
        string id = Text(ev, "id");
        JsonElement date = Child(ev, "date");
        JsonElement competitions = Child(ev, "competitions");
        if (id.Length == 0 || date.ValueKind != JsonValueKind.String
            || !DateTime.TryParse(date.GetString(), CultureInfo.InvariantCulture, AsUtc, out DateTime utc)
            || competitions.ValueKind != JsonValueKind.Array || competitions.GetArrayLength() == 0)
        {
            return null;
        }
        JsonElement comp = competitions[0];

        // Convert known kickoff times using Central
        bool timeSet = Child(ev, "timeValid").ValueKind != JsonValueKind.False;
        Game game = new Game
        {
            Season = season,
            EventId = id,
            Week = seasonType == 3 ? Game.BowlWeek : ev.GetProperty("week").GetProperty("number").GetInt32(),
            Date = timeSet ? TimeZoneInfo.ConvertTimeFromUtc(utc, central).Date : utc.Date,
        };
        JsonElement type = comp.GetProperty("status").GetProperty("type");
        game.Status = type.GetProperty("completed").GetBoolean() ? Game.StatusFinal
            : type.GetProperty("state").GetString() == "in" ? Game.StatusInProgress : Game.StatusScheduled;

        bool neutral = Child(comp, "neutralSite").ValueKind == JsonValueKind.True;
        foreach (JsonElement team in comp.GetProperty("competitors").EnumerateArray())
        {
            int? score = ReadScore(team);
            if (Text(team.GetProperty("team"), "id") == teamId)
            {
                game.OUScore = score;
                game.Site = neutral ? "Neutral" : team.GetProperty("homeAway").GetString() == "home" ? "Home" : "Away";
            }
            else
            {
                game.OpponentId = Text(team.GetProperty("team"), "id");
                string? location = team.GetProperty("team").GetProperty("location").GetString();
                game.Opponent = location != null ? location : "";
                game.OppScore = score;
            }
        }
        return game;
    }

    // Parse optional score
    private static int? ReadScore(JsonElement team) => GameCsv.ReadInt(Text(Child(team, "score"), "displayValue"));

    // Read optional JSON property
    internal static JsonElement Child(JsonElement parent, string name) =>
        parent.ValueKind == JsonValueKind.Object && parent.TryGetProperty(name, out JsonElement child) ? child : default;

    // Read JSON text
    internal static string Text(JsonElement parent, string name)
    {
        JsonElement value = Child(parent, name);
        return value.ValueKind == JsonValueKind.String || value.ValueKind == JsonValueKind.Number ? value.ToString() : "";
    }

    // Read optional JSON number
    internal static double? Number(JsonElement parent, string name) =>
        Child(parent, name).ValueKind == JsonValueKind.Number ? Child(parent, name).GetDouble() : null;

    // Enumerate available JSON array
    internal static IEnumerable<JsonElement> Items(JsonElement array) =>
        array.ValueKind == JsonValueKind.Array ? array.EnumerateArray() : Enumerable.Empty<JsonElement>();

    // Parse game summary
    internal static void ParseSummary(Game game, string json, string teamId = OklahomaId)
    {
        using JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;
        if (root.ValueKind != JsonValueKind.Object)
        {
            throw new JsonException("ESPN game summary was not an object.");
        }

        // Read prediction independently
        ReadWinChance(game, root, teamId);

        // Read available box scores
        foreach (JsonElement team in Items(Child(Child(root, "boxscore"), "teams")))
        {
            ReadBoxScoreTeam(game, team, teamId);
        }

        // Find team's first drive
        JsonElement drives = Child(Child(root, "drives"), "previous");
        if (game.IsCompleted && drives.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement drive in drives.EnumerateArray())
            {
                if (Text(Child(drive, "team"), "id") == teamId)
                {
                    game.FirstDrive = Text(drive, "displayResult");
                    break;
                }
            }
        }
    }

    // Read unfinished game's win probability
    private static void ReadWinChance(Game game, JsonElement root, string teamId)
    {
        foreach (string side in new string[] { "homeTeam", "awayTeam" })
        {
            JsonElement p = Child(Child(root, "predictor"), side);
            if (!game.IsCompleted && Text(p, "id") == teamId
                && Double.TryParse(Text(p, "gameProjection"), NumberStyles.Float, CultureInfo.InvariantCulture, out double pct)
                && Double.IsFinite(pct) && pct >= 0 && pct <= 100)
            {
                game.WinPct = pct;
                game.OddsAsOf = DateTime.Today;
            }
        }
    }

    // Read team box score
    private static void ReadBoxScoreTeam(Game game, JsonElement team, string teamId)
    {
        JsonElement info = team.GetProperty("team");
        bool isPicked = info.GetProperty("id").GetString() == teamId;
        if (!isPicked)
        {
            game.OpponentColor = Text(info, "color");
            game.OpponentAltColor = Text(info, "alternateColor");
        }
        JsonElement stats = Child(team, "statistics");
        if (!game.IsCompleted || stats.ValueKind != JsonValueKind.Array)
        {
            return;
        }

        // Index named stats
        Dictionary<string, string> s = new Dictionary<string, string>();
        foreach (JsonElement stat in stats.EnumerateArray())
        {
            s[Text(stat, "name")] = Text(stat, "displayValue");
        }

        // Calculate total offensive plays
        int? rushAtt = Number(s, "rushingAttempts");
        int? plays = rushAtt + Number(s, "completionAttempts", '/', 1);
        if (isPicked)
        {
            game.OUYards = Number(s, "totalYards");
            game.OUPlays = plays;
            game.OURushYds = Number(s, "rushingYards");
            game.OURushAtt = rushAtt;
            game.OUPassYds = Number(s, "netPassingYards");
            game.OU3rdMade = Number(s, "thirdDownEff", '-', 0);
            game.OU3rdAtt = Number(s, "thirdDownEff", '-', 1);
            game.OUInt = Number(s, "interceptions");
            game.OUFumLost = Number(s, "fumblesLost");
        }
        else
        {
            game.OppYards = Number(s, "totalYards");
            game.OppPlays = plays;
            game.OppRushYds = Number(s, "rushingYards");
            game.OppRushAtt = rushAtt;
        }
    }

    // Parse selected stat component
    private static int? Number(Dictionary<string, string> stats, string name, char separator = ' ', int piece = 0)
    {
        if (!stats.ContainsKey(name))
        {
            return null;
        }
        string[] parts = separator == ' ' ? new string[] { stats[name] } : stats[name].Split(separator);
        return piece < parts.Length ? GameCsv.ReadInt(parts[piece]) : null;
    }

    // Read available weekly FPI
    private static async Task<double?> ReadFpiAsync(string teamId, int season, int week)
    {
        if (week >= Game.BowlWeek)
        {
            return null;
        }
        try
        {
            return await ReadFpiPageAsync(teamId, season, week).ConfigureAwait(false);
        }
        catch (Exception error) when (IsDataError(error))
        {
            return null;
        }
    }

    // Download weekly ratings
    private static async Task<double?> ReadFpiPageAsync(string teamId, int season, int week)
    {
        string url = FormattableString.Invariant($"{CoreApi}{season}/types/2/weeks/{week}/powerindex?limit=200");
        using HttpResponseMessage response = await http.GetAsync(url).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        using JsonDocument doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        double? fpi = null;
        foreach (JsonElement item in doc.RootElement.GetProperty("items").EnumerateArray())
        {
            if (Text(Child(item, "team"), "$ref").Contains("/teams/" + teamId + "?"))
            {
                foreach (JsonElement p in item.GetProperty("predictives").EnumerateArray())
                {
                    if (p.GetProperty("name").GetString() == "fpi")
                    {
                        fpi = Math.Round(p.GetProperty("value").GetDouble(), 1);
                    }
                }
            }
        }
        return fpi;
    }

    // Download missing team logos
    public static async Task SaveLogosAsync(IEnumerable<string> teamIds)
    {
        string folder = Path.Combine(GameCsv.DataFolder, "logos");
        try
        {
            Directory.CreateDirectory(folder);
        }
        catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
        {
            return;
        }
        HashSet<string> ids = new HashSet<string>(teamIds);
        ids.RemoveWhere(id => string.IsNullOrWhiteSpace(id) || File.Exists(Path.Combine(folder, id + ".png")));
        await Task.WhenAll(ids.Select(async id =>
        {
            try
            {
                // Save complete temporary download
                byte[] png = await http.GetByteArrayAsync(LogoApi + id + ".png&w=160&h=160").ConfigureAwait(false);
                string file = Path.Combine(folder, id + ".png");
                await File.WriteAllBytesAsync(file + ".tmp", png).ConfigureAwait(false);
                File.Move(file + ".tmp", file, true);
            }
            catch (Exception error) when (error is HttpRequestException || error is TaskCanceledException || error is IOException || error is UnauthorizedAccessException)
            {
                // Missing logo uses initials
            }
        }));
    }
}
