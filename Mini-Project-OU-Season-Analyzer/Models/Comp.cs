namespace Caleb_Marks_OU_Season_Analyzer.Models;

// One historical comparison
// Compared = false means insufficient data
internal sealed class Comp
{
    public string Key { get; }
    public string Title { get; }
    public string Now { get; }
    public List<SeasonEnd> Matches { get; }
    public string Note { get; }
    public bool Compared { get; }

    // Set comparison values
    public Comp(string key, string title, string now, List<SeasonEnd> matches, string note, bool compared = true)
    {
        Key = key;
        Title = title;
        Now = now;
        Matches = matches;
        Note = note;
        Compared = compared;
    }

    // Newest matching season
    public SeasonEnd? Last => Matches.Count > 0 ? Matches[0] : null;

    // Count matching outcomes
    public int Count(Finish finish) => Matches.Count(m => m.Finish == finish);
}
