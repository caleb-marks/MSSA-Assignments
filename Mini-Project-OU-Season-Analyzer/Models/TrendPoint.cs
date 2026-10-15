namespace Caleb_Marks_OU_Season_Analyzer.Models;

// One game's trend data
internal class TrendPoint
{
    public Game Game { get; set; }
    public double? YppMargin { get; set; }
    public double? YppAvg3 { get; set; }
    public double? Fpi { get; set; }

    // Set associated game
    public TrendPoint(Game game)
    {
        Game = game;
    }
}
