using System.Globalization;
using System.Net;
using System.Text.Json;
using Caleb_Marks_OU_Season_Analyzer.Logic;
using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Data;

// Download supplemental team data
internal static class ExtrasClient
{
    private const string StatsApi = "https://site.web.api.espn.com/apis/common/v3/sports/football/college-football/statistics/byteam"
        + "?region=us&lang=en&contentorigin=espn&limit=300&seasontype=2&group=80&season=";
    private const string SiteApi = "https://site.api.espn.com/apis/site/v2/sports/football/college-football/";
    private const string CoreApi = "https://sports.core.api.espn.com/v2/sports/football/leagues/college-football/seasons/";
    private const string PhotoApi = "https://a.espncdn.com/combiner/i?img=/i/headshots/college-football/players/full/";

    // Player leader categories
    private static readonly string[] StarCategories = { "passingYards", "rushingYards", "receivingYards", "totalTackles", "sacks", "interceptions" };

    // ESPN stat mappings
    private static readonly (string Key, bool Opponent, string Category, string Stat)[] Sources =
    {
        ("pointsFor", false, "passing", "totalPointsPerGame"),
        ("pointsAgainst", true, "passing", "totalPointsPerGame"),
        ("rushYds", false, "rushing", "rushingYardsPerGame"),
        ("rushYdsAllowed", true, "rushing", "rushingYardsPerGame"),
        ("passYds", false, "passing", "passingYardsPerGame"),
        ("passYdsAllowed", true, "passing", "passingYardsPerGame"),
        ("totalYds", false, "passing", "yardsPerGame"),
        ("totalYdsAllowed", true, "passing", "yardsPerGame"),
        ("thirdDownPct", false, "miscellaneous", "thirdDownConvPct"),
        ("thirdDownPctAllowed", true, "miscellaneous", "thirdDownConvPct"),
        ("rushYpc", false, "rushing", "yardsPerRushAttempt"),
        ("rushYpcAllowed", true, "rushing", "yardsPerRushAttempt"),
        ("sacksMade", true, "passing", "sacks"),
        ("gamesPlayed", false, "general", "gamesPlayed"),
    };

    // Retrying client; 20-second timeout
    private static readonly HttpClient http = new HttpClient(new RetryHandler()) { Timeout = TimeSpan.FromSeconds(20) };

    // Season turnover totals
    internal sealed record Turnovers(double Margin, double Giveaways, double Takeaways, double Games);

    // Update supplemental data
    public static async Task<string> UpdateAsync(Team team, List<Game> games, IProgress<string>? progress)
    {
        List<string> problems = new List<string>();
        try
        {
            string season = (games.Count > 0 ? games.Max(g => g.Season) : EspnClient.Seasons.Max()).ToString(CultureInfo.InvariantCulture);

            // Start independent downloads
            progress?.Report("Getting national stats & star players...");
            Task<string?> statsPage = TryReadAsync(StatsApi + season);
            Task<string?> colorsPage = TryReadAsync(SiteApi + "teams?limit=1000");
            Task<string?> leadersPage = TryReadAsync(CoreApi + season + "/types/2/teams/" + team.Id + "/leaders");
            Task<string?> rosterPage = TryReadAsync(SiteApi + "teams/" + team.Id + "/roster");
            string? statsJson = await statsPage;
            string? colorsJson = await colorsPage;
            string? leadersJson = await leadersPage;
            string? rosterJson = await rosterPage;

            problems.Add(await UpdateStatsAsync(team, season, statsJson, colorsJson, progress));
            List<PlayerStar> stars = await UpdateStarsAsync(team, season, games, leadersJson, rosterJson, problems, progress);
            problems.Add(await SavePhotosAsync(stars, progress));

            // Download stat leaders' logos
            await EspnClient.SaveLogosAsync(Ladder.Build(team.Id, ExtrasStore.LoadStats()).Select(l => l.BestTeamId));
        }
        catch (Exception error)
        {
            // Report unexpected update failure
            problems.Add("stopped (" + error.Message + ")");
        }
        problems.RemoveAll(p => p.Length == 0);
        return problems.Count == 0 ? "" : "Stars & ranks not fully updated: " + string.Join(", ", problems) + ". Saved data kept.";
    }

    // Update opponent leaders
    public static async Task<string> UpdateOpponentStarsAsync(string teamId, int season)
    {
        try
        {
            string year = season.ToString(CultureInfo.InvariantCulture);
            string? leaders = await TryReadAsync(CoreApi + year + "/types/2/teams/" + teamId + "/leaders");
            string? roster = await TryReadAsync(SiteApi + "teams/" + teamId + "/roster");
            if (leaders == null)
            {
                return "Opponent's star players could not be downloaded.";
            }
            Dictionary<string, string> names = roster == null ? new Dictionary<string, string>() : ParseRosterNames(roster);
            List<PlayerStar> stars = await FillNamesAsync(ParseSeasonLeaders(leaders, names), year);
            stars = stars.FindAll(s => s.Name.Length > 0);
            ExtrasStore.WriteStars(ExtrasStore.OpponentStarsPath(teamId), stars);
            string photos = await SavePhotosAsync(stars, null);
            return photos.Length == 0 ? "" : $"Opponent's {photos} could not be saved.";
        }
        catch (Exception error) when (EspnClient.IsDataError(error) || error is IOException || error is UnauthorizedAccessException)
        {
            return "Opponent's star players could not be saved (" + error.Message + ").";
        }
    }

    // Read available page text
    private static async Task<string?> TryReadAsync(string url)
    {
        try
        {
            return await http.GetStringAsync(url).ConfigureAwait(false);
        }
        catch (Exception error) when (EspnClient.IsDataError(error))
        {
            return null;
        }
    }

    // Run eight concurrent jobs maximum
    private static Task RunAllAsync(int count, Func<int, Task> job) =>
        Parallel.ForAsync(0, count, new ParallelOptions { MaxDegreeOfParallelism = 8 }, async (i, _) => await job(i).ConfigureAwait(false));

    // Update national stats
    private static async Task<string> UpdateStatsAsync(Team team, string season, string? statsJson, string? colorsJson,
        IProgress<string>? progress)
    {
        if (statsJson == null)
        {
            return "national ranks";
        }
        try
        {
            List<TeamStatValue> saved = ExtrasStore.LoadStats();
            List<TeamStatValue> fresh = ParseTeamStats(statsJson);
            if (fresh.Count == 0)
            {
                return "national ranks (ESPN sent no team stats)";
            }

            // Preserve missing stat categories
            HashSet<string> sent = new HashSet<string>(fresh.Select(r => r.Key));
            List<string> missing = Sources.Select(s => s.Key).Concat(new string[] { "yardsPerPlay", "yardsPerPlayAllowed" }).Distinct()
                .Where(key => !sent.Contains(key)).ToList();
            fresh.AddRange(saved.Where(r => missing.Contains(r.Key)));
            fresh = AddColors(fresh, colorsJson, saved);

            // Refresh outdated turnover totals
            Dictionary<string, Turnovers> have = SavedTurnovers(saved);
            List<string> need = fresh.Any(r => r.TeamId == team.Id) ? NeedTurnovers(fresh, have) : new List<string>();
            int failed = await DownloadTurnoversAsync(need, have, season, progress);
            ExtrasStore.SaveStats(Ladder.RankAll(fresh.Concat(TurnoverRows(fresh, have)).ToList()));
            string note = missing.Count > 0 ? "some national stats" : "";
            if (failed > 0)
            {
                return note.Length > 0 ? "some turnover stats, " + note : "some turnover stats";
            }
            return note;
        }
        catch (Exception error) when (EspnClient.IsDataError(error) || error is IOException || error is UnauthorizedAccessException)
        {
            return "national ranks (" + error.Message + ")";
        }
    }

    // Fill team colors
    private static List<TeamStatValue> AddColors(List<TeamStatValue> rows, string? teamsJson, List<TeamStatValue> saved)
    {
        Dictionary<string, string> colors = new Dictionary<string, string>();
        foreach (TeamStatValue r in saved.Where(r => r.Color.Length > 0))
        {
            colors[r.TeamId] = r.Color;
        }
        try
        {
            if (teamsJson != null)
            {
                foreach (KeyValuePair<string, string> pair in ParseTeamColors(teamsJson))
                {
                    colors[pair.Key] = pair.Value;
                }
            }
        }
        catch (Exception error) when (EspnClient.IsDataError(error))
        {
            // Preserve saved colors
        }
        return rows.Select(r => new TeamStatValue(r.TeamId, r.TeamName, colors.GetValueOrDefault(r.TeamId, ""), r.Key, r.Value, r.Rank)).ToList();
    }

    // O(n): parse team colors
    internal static Dictionary<string, string> ParseTeamColors(string teamsJson)
    {
        using JsonDocument doc = JsonDocument.Parse(teamsJson);
        JsonElement sports = Child(doc.RootElement, "sports");
        if (sports.ValueKind != JsonValueKind.Array)
        {
            throw new JsonException("ESPN team list had no sports list.");
        }

        Dictionary<string, string> colors = new Dictionary<string, string>();
        foreach (JsonElement sport in sports.EnumerateArray())
        {
            foreach (JsonElement league in Items(Child(sport, "leagues")))
            {
                foreach (JsonElement item in Items(Child(league, "teams")))
                {
                    JsonElement team = Child(item, "team");
                    if (Text(team, "id").Length > 0 && Text(team, "color").Length > 0)
                    {
                        colors[Text(team, "id")] = Text(team, "color");
                    }
                }
            }
        }
        return colors;
    }

    // Parse ranked team stats
    internal static List<TeamStatValue> ParseTeamStats(string byteamJson)
    {
        using JsonDocument doc = JsonDocument.Parse(byteamJson);
        JsonElement teams = Child(doc.RootElement, "teams");
        JsonElement categories = Child(doc.RootElement, "categories");
        if (teams.ValueKind != JsonValueKind.Array || categories.ValueKind != JsonValueKind.Array)
        {
            throw new JsonException("ESPN team stats had no teams list.");
        }

        // Index category stat names
        Dictionary<string, List<string>> statNames = new Dictionary<string, List<string>>();
        foreach (JsonElement category in categories.EnumerateArray())
        {
            statNames[Text(category, "name")] = Items(Child(category, "names")).Select(n => n.GetString() ?? "").ToList();
        }

        List<TeamStatValue> rows = new List<TeamStatValue>();
        foreach (JsonElement team in teams.EnumerateArray())
        {
            JsonElement info = Child(team, "team");
            string id = Text(info, "id");
            Dictionary<string, double> v = ReadTeamValues(Child(team, "categories"), statNames);
            if (id.Length > 0 && v.ContainsKey("own|general|gamesPlayed") && v["own|general|gamesPlayed"] >= 1)
            {
                AddTeamRows(rows, id, FirstText(Text(info, "shortDisplayName"), Text(info, "displayName"), Text(info, "nickname")), v);
            }
        }
        return Ladder.RankAll(rows);
    }

    // Build team stat rows
    private static void AddTeamRows(List<TeamStatValue> rows, string id, string name, Dictionary<string, double> v)
    {
        foreach (var source in Sources)
        {
            string slot = Slot(source.Opponent, source.Category, source.Stat);
            if (v.ContainsKey(slot))
            {
                rows.Add(new TeamStatValue(id, name, "", source.Key, Math.Round(v[slot], 3), 0));
            }
        }
        foreach (bool opponent in new bool[] { false, true })
        {
            double plays = v.GetValueOrDefault(Slot(opponent, "rushing", "rushingAttempts")) + v.GetValueOrDefault(Slot(opponent, "passing", "passingAttempts"));
            string yards = Slot(opponent, "passing", "totalYards");
            if (plays > 0 && v.ContainsKey(yards))
            {
                rows.Add(new TeamStatValue(id, name, "", opponent ? "yardsPerPlayAllowed" : "yardsPerPlay", Math.Round(v[yards] / plays, 3), 0));
            }
        }
    }

    // Build stat lookup key
    private static string Slot(bool opponent, string category, string stat) => (opponent ? "opp|" : "own|") + category + "|" + stat;

    // Index team stat values
    private static Dictionary<string, double> ReadTeamValues(JsonElement categories, Dictionary<string, List<string>> statNames)
    {
        Dictionary<string, double> values = new Dictionary<string, double>();
        foreach (JsonElement category in Items(categories))
        {
            string name = Text(category, "name");
            bool opponent = Text(category, "splitId") == "900" || Text(category, "displayName").StartsWith("Opponent", StringComparison.Ordinal);
            if (!statNames.ContainsKey(name))
            {
                continue;
            }

            // Match values using name order
            List<string> names = statNames[name];
            int spot = 0;
            foreach (JsonElement value in Items(Child(category, "values")))
            {
                if (spot < names.Count && value.ValueKind == JsonValueKind.Number)
                {
                    values.TryAdd(Slot(opponent, name, names[spot]), value.GetDouble());
                }
                spot++;
            }
        }
        return values;
    }

    // Select teams needing turnover updates
    private static List<string> NeedTurnovers(List<TeamStatValue> fresh, Dictionary<string, Turnovers> have)
    {
        List<string> need = new List<string>();
        foreach (TeamStatValue r in fresh.Where(r => r.Key == "gamesPlayed"))
        {
            if (!need.Contains(r.TeamId) && (!have.ContainsKey(r.TeamId) || have[r.TeamId].Games != r.Value))
            {
                need.Add(r.TeamId);
            }
        }
        return need;
    }

    // Download turnover totals
    private static async Task<int> DownloadTurnoversAsync(List<string> need, Dictionary<string, Turnovers> have, string season,
        IProgress<string>? progress)
    {
        Turnovers?[] got = new Turnovers?[need.Count];
        int done = 0;
        int failed = 0;
        await RunAllAsync(need.Count, async i =>
        {
            try
            {
                got[i] = ParseTurnovers(await http.GetStringAsync(CoreApi + season + "/types/2/teams/" + need[i] + "/statistics").ConfigureAwait(false));
            }
            catch (Exception error) when (EspnClient.IsDataError(error))
            {
                // Leave failed download unknown
            }
            if (got[i] == null)
            {
                Interlocked.Increment(ref failed);
            }
            progress?.Report("Reading turnovers: " + Interlocked.Increment(ref done) + " of " + need.Count + "...");
        });
        for (int i = 0; i < need.Count; i++)
        {
            if (got[i] != null)
            {
                have[need[i]] = got[i]!;
            }
        }
        return failed;
    }

    // O(n): restore saved turnover totals
    private static Dictionary<string, Turnovers> SavedTurnovers(List<TeamStatValue> saved)
    {
        Dictionary<string, Dictionary<string, double>> byTeam = new Dictionary<string, Dictionary<string, double>>();
        foreach (TeamStatValue r in saved.Where(r => r.Key == "turnoverMargin" || r.Key == "giveaways" || r.Key == "takeaways" || r.Key == "turnoverGames"))
        {
            if (!byTeam.ContainsKey(r.TeamId))
            {
                byTeam[r.TeamId] = new Dictionary<string, double>();
            }
            byTeam[r.TeamId][r.Key] = r.Value;
        }

        Dictionary<string, Turnovers> have = new Dictionary<string, Turnovers>();
        foreach (KeyValuePair<string, Dictionary<string, double>> team in byTeam)
        {
            Dictionary<string, double> k = team.Value;
            if (k.Count == 4)
            {
                have[team.Key] = new Turnovers(k["turnoverMargin"], k["giveaways"], k["takeaways"], k["turnoverGames"]);
            }
        }
        return have;
    }

    // Build turnover stat rows
    internal static List<TeamStatValue> TurnoverRows(List<TeamStatValue> rows, Dictionary<string, Turnovers> byTeam)
    {
        string[] keys = { "turnoverMargin", "giveaways", "takeaways", "turnoverGames", "turnoverMarginPG", "giveawaysPG", "takeawaysPG" };
        List<TeamStatValue> extra = new List<TeamStatValue>();
        HashSet<string> seen = new HashSet<string>();
        foreach (TeamStatValue r in rows)
        {
            if (!seen.Add(r.TeamId) || !byTeam.ContainsKey(r.TeamId))
            {
                continue;
            }
            Turnovers t = byTeam[r.TeamId];
            double games = t.Games > 0 ? t.Games : 1;
            double[] values = { t.Margin, t.Giveaways, t.Takeaways, t.Games,
                Math.Round(t.Margin / games, 3), Math.Round(t.Giveaways / games, 3), Math.Round(t.Takeaways / games, 3) };
            for (int k = 0; k < keys.Length; k++)
            {
                extra.Add(new TeamStatValue(r.TeamId, r.TeamName, r.Color, keys[k], values[k], 0));
            }
        }
        return extra;
    }

    // Parse complete turnover totals
    internal static Turnovers? ParseTurnovers(string coreJson)
    {
        using JsonDocument doc = JsonDocument.Parse(coreJson);
        JsonElement categories = Child(Child(doc.RootElement, "splits"), "categories");
        double? margin = StatValue(categories, "miscellaneous", "turnOverDifferential");
        double? giveaways = StatValue(categories, "miscellaneous", "totalGiveaways");
        double? takeaways = StatValue(categories, "miscellaneous", "totalTakeaways");
        double? games = StatValue(categories, "general", "gamesPlayed");
        if (!margin.HasValue || !giveaways.HasValue || !takeaways.HasValue || !games.HasValue)
        {
            return null;
        }
        return new Turnovers(margin.Value, giveaways.Value, takeaways.Value, games.Value);
    }

    // Find named category stat
    private static double? StatValue(JsonElement categories, string category, string stat)
    {
        foreach (JsonElement c in Items(categories).Where(c => Text(c, "name") == category))
        {
            foreach (JsonElement s in Items(Child(c, "stats")).Where(s => Text(s, "name") == stat))
            {
                return Number(s, "value");
            }
        }
        return null;
    }

    // Merge player leader updates
    private static async Task<List<PlayerStar>> UpdateStarsAsync(Team team, string season, List<Game> games,
        string? leadersJson, string? rosterJson, List<string> problems, IProgress<string>? progress)
    {
        List<PlayerStar> saved = ExtrasStore.LoadStars(team.Id);
        List<PlayerStar> seasonStars = saved.Where(s => s.EventId.Length == 0).ToList();
        List<PlayerStar> gameStars = saved.Where(s => s.EventId.Length > 0).ToList();
        bool changed = !File.Exists(ExtrasStore.StarsPath(team.Id));

        // Resolve season leader names
        if (leadersJson == null)
        {
            problems.Add("season stars");
        }
        else
        {
            List<PlayerStar>? merged = await ReadSeasonStarsAsync(leadersJson, rosterJson, season, seasonStars, problems);
            // Keep saved leaders when update empty
            if (merged != null && merged.Count > 0 && !merged.SequenceEqual(seasonStars))
            {
                seasonStars = merged;
                changed = true;
            }
        }

        // Append new game leaders
        foreach (List<PlayerStar> stars in await ReadGameStarsAsync(team.Id, games, gameStars, problems, progress))
        {
            gameStars.AddRange(stars);
            changed = true;
        }

        List<PlayerStar> all = seasonStars.Concat(gameStars).ToList();
        if (changed && all.Count > 0)
        {
            try
            {
                ExtrasStore.SaveStars(team.Id, all);
            }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
            {
                problems.Add("stars file (" + error.Message + ")");
            }
        }
        return all;
    }

    // Read updated season leaders
    private static async Task<List<PlayerStar>?> ReadSeasonStarsAsync(string leadersJson, string? rosterJson, string season,
        List<PlayerStar> seasonStars, List<string> problems)
    {
        try
        {
            Dictionary<string, string> names = rosterJson == null ? new Dictionary<string, string>() : ParseRosterNames(rosterJson);
            List<PlayerStar> fresh = await FillNamesAsync(ParseSeasonLeaders(leadersJson, names), season);
            List<PlayerStar> merged = new List<PlayerStar>();
            foreach (PlayerStar star in fresh)
            {
                PlayerStar? keep = star.Name.Length > 0 ? star : seasonStars.Find(old => old.Category == star.Category);
                if (keep != null)
                {
                    merged.Add(keep);
                }
            }
            if (fresh.Exists(s => s.Name.Length == 0))
            {
                problems.Add("some star names");
            }
            return merged;
        }
        catch (Exception error) when (EspnClient.IsDataError(error))
        {
            problems.Add("season stars (" + error.Message + ")");
            return null;
        }
    }

    // Download missing game leaders
    private static async Task<List<List<PlayerStar>>> ReadGameStarsAsync(string teamId, List<Game> games, List<PlayerStar> gameStars,
        List<string> problems, IProgress<string>? progress)
    {
        HashSet<string> haveEvents = new HashSet<string>(gameStars.Select(s => s.EventId));
        List<Game> todo = games.FindAll(g => g.IsFinal && g.EventId.Length > 0 && haveEvents.Add(g.EventId));
        List<PlayerStar>?[] found = new List<PlayerStar>?[todo.Count];
        int done = 0;
        await RunAllAsync(todo.Count, async i =>
        {
            try
            {
                found[i] = ParseGameLeaders(await http.GetStringAsync(SiteApi + "summary?event=" + todo[i].EventId).ConfigureAwait(false), todo[i].EventId, teamId);
            }
            catch (Exception error) when (EspnClient.IsDataError(error))
            {
                // Retry game next update
            }
            progress?.Report("Reading game stars: " + Interlocked.Increment(ref done) + " of " + todo.Count + "...");
        });
        if (found.Any(f => f == null))
        {
            problems.Add("some game stars");
        }
        return found.Where(f => f != null && f.Count > 0).Select(f => f!).ToList();
    }

    // O(n): parse roster names
    internal static Dictionary<string, string> ParseRosterNames(string rosterJson)
    {
        using JsonDocument doc = JsonDocument.Parse(rosterJson);
        JsonElement groups = Child(doc.RootElement, "athletes");
        if (groups.ValueKind != JsonValueKind.Array)
        {
            throw new JsonException("ESPN roster had no athletes list.");
        }

        // Accept grouped or flat rosters
        Dictionary<string, string> names = new Dictionary<string, string>();
        foreach (JsonElement group in groups.EnumerateArray())
        {
            JsonElement items = Child(group, "items");
            IEnumerable<JsonElement> athletes = items.ValueKind == JsonValueKind.Array ? Items(items) : new JsonElement[] { group };
            foreach (JsonElement athlete in athletes)
            {
                string name = FirstText(Text(athlete, "displayName"), Text(athlete, "fullName"));
                if (Text(athlete, "id").Length > 0 && name.Length > 0)
                {
                    names[Text(athlete, "id")] = name;
                }
            }
        }
        return names;
    }

    // Parse season category leaders
    internal static List<PlayerStar> ParseSeasonLeaders(string leadersJson, Dictionary<string, string> namesById)
    {
        using JsonDocument doc = JsonDocument.Parse(leadersJson);
        JsonElement categories = Child(doc.RootElement, "categories");
        if (categories.ValueKind != JsonValueKind.Array)
        {
            throw new JsonException("ESPN season leaders had no categories list.");
        }

        List<PlayerStar> stars = new List<PlayerStar>();
        foreach (string category in StarCategories)
        {
            JsonElement leader = TopLeader(categories, category);
            string id = IdFromLink(Text(Child(leader, "athlete"), "$ref"), "/athletes/");
            double? value = Number(leader, "value");
            if (id.Length == 0 || !value.HasValue)
            {
                continue;
            }

            // Select matching athlete details
            string detailName = category.EndsWith("Yards", StringComparison.Ordinal) ? category.Replace("Yards", "Leader") : "";
            JsonElement detail = detailName.Length > 0 ? TopLeader(categories, detailName) : default;
            bool sameAthlete = IdFromLink(Text(Child(detail, "athlete"), "$ref"), "/athletes/") == id && Text(detail, "displayValue").Length > 0;
            stars.Add(new PlayerStar("", category, id, namesById.GetValueOrDefault(id, ""), value.Value,
                sameAthlete ? Text(detail, "displayValue") : Text(leader, "displayValue")));
        }
        return stars;
    }

    // Parse game category leaders
    internal static List<PlayerStar> ParseGameLeaders(string summaryJson, string eventId, string teamId)
    {
        using JsonDocument doc = JsonDocument.Parse(summaryJson);
        if (doc.RootElement.ValueKind != JsonValueKind.Object)
        {
            throw new JsonException("ESPN game page was not an object.");
        }

        List<PlayerStar> stars = new List<PlayerStar>();
        foreach (JsonElement side in Items(Child(doc.RootElement, "leaders")).Where(s => Text(Child(s, "team"), "id") == teamId))
        {
            JsonElement categories = Child(side, "leaders");
            foreach (string category in StarCategories)
            {
                JsonElement leader = TopLeader(categories, category);
                JsonElement athlete = Child(leader, "athlete");
                string name = FirstText(Text(athlete, "displayName"), Text(athlete, "fullName"), Text(athlete, "shortName"));
                double? value = Number(leader, "value");
                if (Text(athlete, "id").Length > 0 && value.HasValue)
                {
                    stars.Add(new PlayerStar(eventId, category, Text(athlete, "id"), name, value.Value, Text(leader, "displayValue")));
                }
            }
        }
        return stars;
    }

    // O(c x l): select category maximum
    private static JsonElement TopLeader(JsonElement categories, string name)
    {
        JsonElement top = default;
        double most = double.MinValue;
        foreach (JsonElement category in Items(categories).Where(c => Text(c, "name") == name))
        {
            foreach (JsonElement leader in Items(Child(category, "leaders")))
            {
                double? value = Number(leader, "value");
                if (value.HasValue && value.Value > most)
                {
                    top = leader;
                    most = value.Value;
                }
            }
        }
        return top;
    }

    // Resolve missing player names
    private static async Task<List<PlayerStar>> FillNamesAsync(List<PlayerStar> stars, string season)
    {
        PlayerStar[] filled = stars.ToArray();
        await RunAllAsync(filled.Length, async i =>
        {
            PlayerStar s = filled[i];
            if (s.Name.Length > 0)
            {
                return;
            }
            string? page = await TryReadAsync(CoreApi + season + "/athletes/" + s.AthleteId);
            try
            {
                using JsonDocument athlete = JsonDocument.Parse(page != null ? page : "{}");
                filled[i] = new PlayerStar(s.EventId, s.Category, s.AthleteId, Text(athlete.RootElement, "displayName"), s.Value, s.Display);
            }
            catch (JsonException)
            {
                // Leave unresolved name blank
            }
        });
        return filled.ToList();
    }

    // Download missing player photos
    private static async Task<string> SavePhotosAsync(List<PlayerStar> stars, IProgress<string>? progress)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ExtrasStore.PhotoPath("0"))!);
        }
        catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
        {
            return "player photos";
        }

        // Accept numeric athlete identifiers
        List<string> ids = stars.Select(s => s.AthleteId).Where(id => id.Length > 0 && id.All(char.IsAsciiDigit)).Distinct()
            .Where(id => !File.Exists(ExtrasStore.PhotoPath(id))).ToList();
        int done = 0;
        int failed = 0;
        await RunAllAsync(ids.Count, async i =>
        {
            try
            {
                // Save complete temporary photo
                byte[] png = await http.GetByteArrayAsync(PhotoApi + ids[i] + ".png&w=240").ConfigureAwait(false);
                if (IsPng(png))
                {
                    string file = ExtrasStore.PhotoPath(ids[i]);
                    await File.WriteAllBytesAsync(file + ".tmp", png).ConfigureAwait(false);
                    File.Move(file + ".tmp", file, true);
                }
            }
            catch (HttpRequestException error) when (error.StatusCode == HttpStatusCode.NotFound)
            {
                // Missing photo uses silhouette
            }
            catch (Exception error) when (EspnClient.IsDataError(error) || error is IOException || error is UnauthorizedAccessException)
            {
                Interlocked.Increment(ref failed);
            }
            progress?.Report("Saving player photos: " + Interlocked.Increment(ref done) + " of " + ids.Count + "...");
        });
        return failed > 0 ? "some player photos" : "";
    }

    // Check four-byte PNG prefix
    private static bool IsPng(byte[] data) =>
        data.Length > 8 && data[0] == 0x89 && data[1] == 0x50 && data[2] == 0x4E && data[3] == 0x47;

    // Extract identifier following URL marker
    private static string IdFromLink(string link, string marker)
    {
        int start = link.IndexOf(marker, StringComparison.Ordinal);
        if (start < 0)
        {
            return "";
        }
        start += marker.Length;
        int end = link.IndexOfAny(new char[] { '?', '/' }, start);
        if (end < 0)
        {
            end = link.Length;
        }
        return link.Substring(start, end - start);
    }

    // Select first nonempty text
    private static string FirstText(params string[] texts)
    {
        foreach (string text in texts)
        {
            if (text.Length > 0)
            {
                return text;
            }
        }
        return "";
    }

    // Shared JSON readers
    private static JsonElement Child(JsonElement parent, string name) => EspnClient.Child(parent, name);
    // Read JSON text
    private static string Text(JsonElement parent, string name) => EspnClient.Text(parent, name);
    // Read optional JSON number
    private static double? Number(JsonElement parent, string name) => EspnClient.Number(parent, name);
    // Read JSON array items
    private static IEnumerable<JsonElement> Items(JsonElement array) => EspnClient.Items(array);
}
