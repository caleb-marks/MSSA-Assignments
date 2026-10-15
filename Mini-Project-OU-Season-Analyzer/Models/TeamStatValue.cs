namespace Caleb_Marks_OU_Season_Analyzer.Models;

// One saved national stat
internal sealed record TeamStatValue(string TeamId, string TeamName, string Color, string Key, double Value, int Rank);
