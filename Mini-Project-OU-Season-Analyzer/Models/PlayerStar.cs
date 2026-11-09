using System.Globalization;

namespace Caleb_Marks_OU_Season_Analyzer.Models;

// One player leader
// Blank EventId means season leader
internal sealed record PlayerStar(string EventId, string Category, string AthleteId, string Name, double Value, string Display)
{
    // Name suffixes
    private static readonly string[] Endings = { "jr", "jr.", "sr", "sr.", "ii", "iii", "iv", "v" };

    // Category labels
    private static readonly Dictionary<string, string> Labels = new Dictionary<string, string>
    {
        { "passingYards", "Pass yds" }, { "rushingYards", "Rush yds" }, { "receivingYards", "Catch yds" },
        { "totalTackles", "Tackles" }, { "sacks", "Sacks" }, { "interceptions", "Picks" },
    };

    // Last name, suffixes excluded
    public string LastName
    {
        get
        {
            string[] words = Name.Replace(",", " ").Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int last = words.Length - 1;

            // O(w): skip trailing suffixes
            while (last > 0 && Endings.Contains(words[last].ToLowerInvariant()))
            {
                last--;
            }
            return last >= 0 ? words[last] : "";
        }
    }

    // Readable category label
    public string Label => Labels.ContainsKey(Category) ? Labels[Category] : Category;
}
