using System.Globalization;

namespace Caleb_Marks_OU_Season_Analyzer.Models;

// National stat ranking
internal sealed record StatRank(string Key, string Label, string Unit, bool HigherIsBetter, double Value, int Rank, int Count,
    string BestTeamId, string BestTeamName, string BestColor, double BestValue, double Middle)
{
    // Rank badge text
    public string Badge => "#" + Rank + " of " + Count;

    // Check first place
    public bool IsBest => Rank == 1;

    // Rank position: 0 = last, 1 = first
    public double Fraction => Count > 1 ? (double)(Count - Rank) / (Count - 1) : 1;

    // Format stat value
    public string Show(double value) => Key.StartsWith("turnoverMargin", StringComparison.Ordinal)
        ? value.ToString(Key == "turnoverMargin" ? "+0;-0;0" : "+0.0;-0.0;0.0", CultureInfo.InvariantCulture)
        : value.ToString("0.0", CultureInfo.InvariantCulture) + (Unit == "%" ? "%" : "");
}
