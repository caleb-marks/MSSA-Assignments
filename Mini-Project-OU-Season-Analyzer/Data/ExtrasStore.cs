using System.Globalization;
using System.Text;
using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Data;

// Player & national-stat storage
internal static class ExtrasStore
{
    private const string StarsHeader = "EventId,Category,AthleteId,Name,Value,Display";
    private const string StatsHeader = "TeamId,TeamName,Color,Key,Value,Rank";

    // Team leaders file path
    public static string StarsPath(string teamId) => Path.Combine(GameCsv.DataFolder, "stars-" + SafeName(teamId) + ".csv");

    // Opponent leaders file path
    public static string OpponentStarsPath(string teamId) => Path.Combine(GameCsv.DataFolder, "opp-stars-" + SafeName(teamId) + ".csv");

    // National stats file path
    public static string StatsPath => Path.Combine(GameCsv.DataFolder, "fbs-stats.csv");

    // Player photo path
    public static string PhotoPath(string athleteId) => Path.Combine(GameCsv.DataFolder, "players", SafeName(athleteId) + ".png");

    // Load team leaders
    public static List<PlayerStar> LoadStars(string teamId) => ReadStars(StarsPath(teamId));

    // Save team leaders
    public static void SaveStars(string teamId, List<PlayerStar> stars) => WriteStars(StarsPath(teamId), stars);

    // Load national stats
    public static List<TeamStatValue> LoadStats() => ReadStats(StatsPath);

    // Save national stats
    public static void SaveStats(List<TeamStatValue> rows) => WriteStats(StatsPath, rows);

    // Keep safe filename characters
    private static string SafeName(string text)
    {
        string safe = new string(text.Where(c => char.IsAsciiLetterOrDigit(c) || c == '-' || c == '_').ToArray());
        return safe.Length > 0 ? safe : "unknown";
    }

    // Read valid player rows
    internal static List<PlayerStar> ReadStars(string path)
    {
        List<PlayerStar> stars = new List<PlayerStar>();
        foreach (string line in ReadLines(path))
        {
            // Validate player columns
            string[] f = GameCsv.SplitLine(line);
            if (f.Length == 6 && f[1].Length > 0 && f[2].Length > 0 && TryReadNumber(f[4], out double value))
            {
                stars.Add(new PlayerStar(f[0], f[1], f[2], f[3], value, f[5]));
            }
        }
        return stars;
    }

    // Write player rows
    internal static void WriteStars(string path, List<PlayerStar> stars)
    {
        StringBuilder text = new StringBuilder();
        text.AppendLine(StarsHeader);
        foreach (PlayerStar s in stars)
        {
            text.AppendLine(string.Join(",", GameCsv.Quote(s.EventId), GameCsv.Quote(s.Category), GameCsv.Quote(s.AthleteId),
                GameCsv.Quote(OneLine(s.Name)), Number(s.Value), GameCsv.Quote(OneLine(s.Display))));
        }
        GameCsv.SwapIn(path, text.ToString());
    }

    // Read valid stat rows
    internal static List<TeamStatValue> ReadStats(string path)
    {
        List<TeamStatValue> rows = new List<TeamStatValue>();
        foreach (string line in ReadLines(path))
        {
            // Validate stat columns
            string[] f = GameCsv.SplitLine(line);
            if (f.Length == 6 && f[0].Length > 0 && f[3].Length > 0 && TryReadNumber(f[4], out double value))
            {
                rows.Add(new TeamStatValue(f[0], f[1], f[2], f[3], value, GameCsv.ReadInt(f[5]).GetValueOrDefault()));
            }
        }
        return rows;
    }

    // Write stat rows
    internal static void WriteStats(string path, List<TeamStatValue> rows)
    {
        StringBuilder text = new StringBuilder();
        text.AppendLine(StatsHeader);
        foreach (TeamStatValue r in rows)
        {
            text.AppendLine(string.Join(",", GameCsv.Quote(r.TeamId), GameCsv.Quote(OneLine(r.TeamName)), GameCsv.Quote(r.Color),
                GameCsv.Quote(r.Key), Number(r.Value), r.Rank.ToString(CultureInfo.InvariantCulture)));
        }
        GameCsv.SwapIn(path, text.ToString());
    }

    // Read nonempty data lines
    private static List<string> ReadLines(string path)
    {
        try
        {
            using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd().Split('\n').Skip(1).Select(l => l.TrimEnd('\r')).Where(l => l.Trim().Length > 0).ToList();
        }
        catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
        {
            return new List<string>();
        }
    }

    // Parse finite invariant number
    private static bool TryReadNumber(string text, out double value) =>
        double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value) && double.IsFinite(value);

    // Format maximum three decimals
    private static string Number(double value) => value.ToString("0.###", CultureInfo.InvariantCulture);

    // Replace line breaks
    private static string OneLine(string text) => text.Replace("\r", " ").Replace("\n", " ");
}
