using System.Reflection;
using Caleb_Marks_OU_Season_Analyzer.Data;

namespace Caleb_Marks_OU_Season_Analyzer.Controls;

// Highlight clip metadata
internal class ClipInfo
{
    public string Name = "";
    public int Step;
    public string Caption = "";
    public string Credit = "";
    public string Opponent = "";
    // Source game description
    public string GameLine = "";
    public DateTime Date;
}

// Saved highlight library
internal static class ClipLibrary
{
    private static List<ClipInfo>? all;

    // Local video folder
    public static string Folder = Path.Combine(AppContext.BaseDirectory, "Assets", "clips");

    // Cached clip list
    public static List<ClipInfo> All
    {
        get
        {
            if (all == null)
            {
                all = Load();
            }
            return all;
        }
    }

    // Select section clips
    public static List<ClipInfo> ForStep(int step)
    {
        List<ClipInfo> clips = new List<ClipInfo>();
        foreach (ClipInfo clip in All)
        {
            if (clip.Step == step)
            {
                clips.Add(clip);
            }
        }
        return clips;
    }

    // Read enabled highlight rows
    private static List<ClipInfo> Load()
    {
        List<ClipInfo> clips = new List<ClipInfo>();
        string[] lines = Resource("clips.highlights.csv").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length == 0)
        {
            return clips;
        }
        List<string> head = new List<string>(GameCsv.SplitLine(lines[0].TrimEnd('\r')));
        for (int i = 1; i < lines.Length; i++)
        {
            string[] row = GameCsv.SplitLine(lines[i].TrimEnd('\r'));
            if (Field(row, head, "status") == "use")
            {
                ClipInfo clip = new ClipInfo();
                clip.Name = Field(row, head, "post_id");
                clip.Caption = Field(row, head, "play");
                clip.Credit = "Video: " + Field(row, head, "channel");
                clip.Opponent = Field(row, head, "opponent");
                clip.GameLine = Field(row, head, "game");
                DateTime.TryParse(Field(row, head, "date"), System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out clip.Date);
                int.TryParse(Field(row, head, "step"), out clip.Step);
                clips.Add(clip);
            }
        }
        return clips;
    }

    // Read named CSV column
    private static string Field(string[] row, List<string> head, string name)
    {
        int index = head.IndexOf(name);
        if (index < 0 || index >= row.Length)
        {
            return "";
        }
        return row[index];
    }

    // Locate available video file
    public static string? VideoPath(ClipInfo clip)
    {
        string path = Path.Combine(Folder, clip.Name + ".mp4");
        if (File.Exists(path))
        {
            return path;
        }
        return null;
    }

    // Load embedded poster
    public static Bitmap? Poster(ClipInfo clip)
    {
        using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("clips." + clip.Name + ".jpg");
        if (stream == null)
        {
            return null;
        }
        return new Bitmap(stream);
    }

    // Read embedded text
    public static string Resource(string name)
    {
        using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
        if (stream == null)
        {
            return "";
        }
        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
