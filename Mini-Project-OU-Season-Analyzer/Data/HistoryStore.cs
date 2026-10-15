using System.Globalization;
using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Data;

// Saved historical games
internal static class HistoryStore
{
    private const string DateFormat = "yyyy-MM-dd";
    // Earliest supported season
    public const int FirstSeason = 1998;

    // Build history file path
    public static string PathFor(string teamId) =>
        Path.Combine(GameCsv.DataFolder, "history-" + new string(teamId.Where(char.IsAsciiLetterOrDigit).ToArray()) + ".csv");

    // Load team history
    public static List<PastGame> Load(string teamId) => Read(PathFor(teamId));

    // Read valid historical rows
    internal static List<PastGame> Read(string path)
    {
        List<PastGame> games = new List<PastGame>();
        foreach (string line in ReadLines(path))
        {
            string[] f = GameCsv.SplitLine(line);
            int? season = f.Length == 12 ? GameCsv.ReadInt(f[0]) : null;
            int? week = f.Length == 12 ? GameCsv.ReadInt(f[1]) : null;
            // Reject unsupported seasons
            if (season.HasValue && week.HasValue && season.Value >= FirstSeason
                && DateTime.TryParseExact(f[3], DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                games.Add(new PastGame(season.Value, week.Value, f[2] == "1", date, f[4], f[5], f[6], GameCsv.ReadInt(f[7]),
                    GameCsv.ReadInt(f[8]), f[11], GameCsv.ReadInt(f[9]), GameCsv.ReadInt(f[10])));
            }
        }
        return games;
    }

    // Read nonempty data lines
    private static List<string> ReadLines(string path)
    {
        try
        {
            return File.ReadAllLines(path).Skip(1).Where(l => l.Trim().Length > 0).ToList();
        }
        catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
        {
            return new List<string>();
        }
    }
}
