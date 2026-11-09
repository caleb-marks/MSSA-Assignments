using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Data;

// Current team configuration
internal static class TeamStore
{
    // Oklahoma team identity
    public static Team Current
    {
        get { return Team.Oklahoma; }
    }

    // Oklahoma games file path
    public static string GamesPath(Team team)
    {
        return Path.Combine(GameCsv.DataFolder, "games.csv");
    }
}
