using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Logic;

// First-in, first-out game queues
// Upcoming schedule
// Three-game trend window
internal static class GameQueue
{
    // Sliding window size
    public const int Window = 3;

    // O(n): collect unfinished season games
    // Completed status excludes missing-score games
    public static Queue<Game> Upcoming(List<Game> games, int season)
    {
        Queue<Game> upcoming = new Queue<Game>();
        foreach (Game game in games)
        {
            if (game.Season == season && !game.IsCompleted)
            {
                upcoming.Enqueue(game);
            }
        }
        return upcoming;
    }

    // O(n): build season trend points
    // Full windows yield three-game averages
    public static List<TrendPoint> ThreeGameAverages(List<Game> games, int season)
    {
        List<TrendPoint> points = new List<TrendPoint>();
        Queue<Game> lastThree = new Queue<Game>();
        foreach (Game game in games)
        {
            // Skip unrelated or unfinished games
            if (game.Season != season || !game.IsCompleted)
            {
                continue;
            }

            // Maintain three-game window
            // C5: enqueue newest game
            lastThree.Enqueue(game);
            if (lastThree.Count > Window)
            {
                // C5: dequeue oldest game
                lastThree.Dequeue();
            }

            // Store current game's trend values
            TrendPoint point = new TrendPoint(game);
            point.YppMargin = Margin(game);
            point.Fpi = game.FPI;

            // Average full window
            if (lastThree.Count == Window)
            {
                point.YppAvg3 = Average(lastThree);
            }
            points.Add(point);
        }
        return points;
    }

    // Read available yards-per-play edge
    private static double? Margin(Game game)
    {
        if (game.HasStats)
        {
            return game.YppMargin;
        }
        return null;
    }

    // O(1): average three-game window
    // Missing stats prevent averaging
    private static double? Average(Queue<Game> window)
    {
        double sum = 0;
        foreach (Game game in window)
        {
            double? margin = Margin(game);
            if (margin == null)
            {
                return null;
            }
            sum += margin.Value;
        }
        return sum / window.Count;
    }
}
