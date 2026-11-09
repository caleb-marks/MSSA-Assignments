using System.Globalization;
using System.Text;
using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Data;

// Game CSV storage
// Report invalid records
internal static class GameCsv
{
    public const string Header = "Season,Week,Date,EventId,Opponent,OpponentId,OpponentColor,OpponentAltColor,Site,Status," +
        "OUScore,OppScore,WinPct,OddsAsOf,OUYards,OUPlays,OppYards,OppPlays,OURushYds,OURushAtt,OUPassYds," +
        "OppRushYds,OppRushAtt,OU3rdMade,OU3rdAtt,OUInt,OUFumLost,FirstDrive,FPI";

    public static readonly int ColumnCount = Header.Split(',').Length;

    private const string DateFormat = "yyyy-MM-dd";

    // CSV quoting triggers
    private static readonly char[] NeedsQuotes = { ',', '"', '\r', '\n' };

    // Application data folder
    public static string DataFolder => Path.Combine(AppContext.BaseDirectory, "GameData");

    // Current games file
    public static string DefaultPath => TeamStore.GamesPath(TeamStore.Current);

    // Load valid game records
    // Collect skipped-record errors
    public static List<Game> Load(string path, List<string> problems)
    {
        List<Game> games = new List<Game>();
        string? text = ReadText(path, problems);
        if (text == null)
        {
            return games;
        }

        // Skip header record
        List<int> lineNumbers = new List<int>();
        List<string> records = SplitRecords(text, lineNumbers);
        for (int i = 1; i < records.Count; i++)
        {
            Game? game = ParseRecord(records[i], lineNumbers[i], problems);
            if (game != null)
            {
                games.Add(game);
            }
        }
        return games;
    }

    // Read file text
    private static string? ReadText(string path, List<string> problems)
    {
        if (!File.Exists(path))
        {
            problems.Add("No games.csv yet. Click Update from ESPN to download games.");
            return null;
        }
        try
        {
            // Allow shared file access
            // Dispose file handles automatically
            using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
        {
            problems.Add("games.csv could not be read: " + error.Message);
            return null;
        }
    }

    // Parse one game record
    private static Game? ParseRecord(string record, int line, List<string> problems)
    {
        if (record.Trim().Length == 0)
        {
            return null;
        }
        string[] fields = SplitLine(record);
        if (fields.Length != ColumnCount)
        {
            problems.Add("Line " + line + " skipped: expected " + ColumnCount + " columns, found " + fields.Length + ".");
            return null;
        }
        Game? game = Parse(fields, out string problem);
        if (game == null)
        {
            problems.Add("Line " + line + " skipped: " + problem);
        }
        return game;
    }

    // Split CSV records
    // Preserve quoted line breaks
    private static List<string> SplitRecords(string text, List<int> lineNumbers)
    {
        List<string> records = new List<string>();
        bool quoted = false;
        int start = 0;
        int line = 1;
        int firstLine = 1;
        for (int i = 0; i < text.Length; i++)
        {
            if (quoted && text[i] == '"' && i + 1 < text.Length && text[i + 1] == '"')
            {
                // Skip escaped quote
                i++;
            }
            else if (text[i] == '"' && (quoted || i == start || text[i - 1] == ','))
            {
                // Toggle field quoting
                quoted = !quoted;
            }
            else if (text[i] == '\r' || text[i] == '\n')
            {
                int end = i;
                if (text[i] == '\r' && i + 1 < text.Length && text[i + 1] == '\n')
                {
                    i++;
                }
                line++;
                if (!quoted)
                {
                    records.Add(text.Substring(start, end - start));
                    lineNumbers.Add(firstLine);
                    start = i + 1;
                    firstLine = line;
                }
            }
        }
        AddRest(text.Substring(start), quoted, firstLine, records, lineNumbers);
        return records;
    }

    // Recover trailing records
    // Unclosed quotes trigger line-based recovery
    private static void AddRest(string rest, bool quoted, int firstLine, List<string> records, List<int> lineNumbers)
    {
        if (!quoted)
        {
            if (rest.Length > 0)
            {
                records.Add(rest);
                lineNumbers.Add(firstLine);
            }
            return;
        }
        string[] lines = rest.Split('\n');
        for (int k = 0; k < lines.Length; k++)
        {
            records.Add(lines[k].TrimEnd('\r'));
            lineNumbers.Add(firstLine + k);
        }
    }

    // Split CSV fields
    // Unescape doubled quotes
    internal static string[] SplitLine(string line)
    {
        List<string> fields = new List<string>();
        StringBuilder field = new StringBuilder();
        bool quoted = false;
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (quoted && c == '"' && i + 1 < line.Length && line[i + 1] == '"')
            {
                field.Append('"');
                i++;
            }
            else if (c == '"' && (quoted || field.Length == 0))
            {
                quoted = !quoted;
            }
            else if (c == ',' && !quoted)
            {
                fields.Add(field.ToString());
                field.Clear();
            }
            else
            {
                field.Append(c);
            }
        }
        fields.Add(field.ToString());
        return fields.ToArray();
    }

    // Parse game fields
    // Optional stats remain nullable
    private static Game? Parse(string[] f, out string problem)
    {
        problem = "";
        if (!Int32.TryParse(f[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int season)
            || !Int32.TryParse(f[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int week)
            || !TryReadDate(f[2], out DateTime date))
        {
            problem = "season, week or date is not valid.";
            return null;
        }

        // Reject invalid forecast values
        if (!TryReadDouble(f[12], out double? winPct) || winPct < 0 || winPct > 100)
        {
            problem = "WinPct \"" + f[12] + "\" must be a number from 0 to 100.";
            return null;
        }
        if (!TryReadDouble(f[28], out double? fpi))
        {
            problem = "FPI \"" + f[28] + "\" must be a finite number.";
            return null;
        }

        // Map columns using header order
        return new Game
        {
            Season = season, Week = week, Date = date, EventId = f[3], Opponent = f[4], OpponentId = f[5],
            OpponentColor = f[6], OpponentAltColor = f[7], Site = f[8], Status = f[9],
            OUScore = ReadInt(f[10]), OppScore = ReadInt(f[11]), WinPct = winPct,
            OddsAsOf = TryReadDate(f[13], out DateTime oddsAsOf) ? oddsAsOf : null,
            OUYards = ReadInt(f[14]), OUPlays = ReadInt(f[15]), OppYards = ReadInt(f[16]), OppPlays = ReadInt(f[17]),
            OURushYds = ReadInt(f[18]), OURushAtt = ReadInt(f[19]), OUPassYds = ReadInt(f[20]),
            OppRushYds = ReadInt(f[21]), OppRushAtt = ReadInt(f[22]), OU3rdMade = ReadInt(f[23]), OU3rdAtt = ReadInt(f[24]),
            OUInt = ReadInt(f[25]), OUFumLost = ReadInt(f[26]), FirstDrive = f[27], FPI = fpi,
        };
    }

    // Parse invariant date
    private static bool TryReadDate(string text, out DateTime date) =>
        DateTime.TryParseExact(text, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

    // Parse nullable integer
    internal static int? ReadInt(string? text) =>
        Int32.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value) ? value : null;

    // Parse optional finite number
    private static bool TryReadDouble(string text, out double? value)
    {
        bool ok = Double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double number) && Double.IsFinite(number);
        value = ok ? number : null;
        return ok || text.Trim().Length == 0;
    }

    // Write game records
    public static void Save(string path, List<Game> games)
    {
        StringBuilder text = new StringBuilder();
        text.AppendLine(Header);
        foreach (Game g in games)
        {
            text.AppendLine(ToLine(g));
        }
        SwapIn(path, text.ToString());
    }

    // Format one CSV record
    // Quote text fields when needed
    private static string ToLine(Game g)
    {
        return string.Join(",",
            Text(g.Season), Text(g.Week), Text(g.Date), Quote(g.EventId), Quote(g.Opponent), Quote(g.OpponentId),
            Quote(g.OpponentColor), Quote(g.OpponentAltColor), Quote(g.Site), Quote(g.Status), Text(g.OUScore),
            Text(g.OppScore), Text(g.WinPct), Text(g.OddsAsOf), Text(g.OUYards), Text(g.OUPlays), Text(g.OppYards),
            Text(g.OppPlays), Text(g.OURushYds), Text(g.OURushAtt), Text(g.OUPassYds), Text(g.OppRushYds), Text(g.OppRushAtt),
            Text(g.OU3rdMade), Text(g.OU3rdAtt), Text(g.OUInt), Text(g.OUFumLost), Quote(g.FirstDrive), Text(g.FPI));
    }

    // Replace file using temporary copy
    // Temporary writes preserve original
    internal static void SwapIn(string path, string text)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        string temp = path + ".tmp";
        File.WriteAllText(temp, text);
        try
        {
            File.Move(temp, path, true);
        }
        catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
        {
            // Remove failed temporary copy
            File.Delete(temp);
            throw;
        }
    }

    // Format nullable integer
    private static string Text(int? value) => value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : "";

    // Format maximum three decimals
    private static string Text(double? value) => value.HasValue ? value.Value.ToString("0.###", CultureInfo.InvariantCulture) : "";

    // Format nullable date
    private static string Text(DateTime? value) => value.HasValue ? value.Value.ToString(DateFormat, CultureInfo.InvariantCulture) : "";

    // Escape CSV field
    internal static string Quote(string text) => text.IndexOfAny(NeedsQuotes) < 0 ? text : "\"" + text.Replace("\"", "\"\"") + "\"";
}
