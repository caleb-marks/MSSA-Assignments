namespace Caleb_Marks_OU_Season_Analyzer.Models;

// Chart layout choices
internal enum ChartKind { Bars, Split, Rank }

// One labeled chart value
// Loss styling includes stripes
internal sealed record ChartBar(string Label, double Value, string Text, bool Highlight = false, bool Loss = false);

// Section chart data
internal sealed class StoryChart
{
    public ChartKind Kind;
    public string Title = "";
    // Ranked team count
    public int Of;
    public List<ChartBar> Bars = new List<ChartBar>();

    // Set chart identity
    public StoryChart(ChartKind kind, string title)
    {
        Kind = kind;
        Title = title;
    }

    // Describe chart values aloud
    public string Describe() => Title + ": " + string.Join(", ", Bars.Select(b => b.Label + " " + b.Text)) + ".";
}
