namespace Caleb_Marks_OU_Season_Analyzer.Logic;

// Recursive season forecast
// Each call processes one game
// Merged win totals avoid exponential branching
internal static class Recursion
{
    // Recursive call count
    public static int Calls;

    // Calculate final-win probabilities
    public static double[] FinalWins(double[] chances, int winsSoFar)
    {
        // Allocate possible win totals
        double[] totals = new double[winsSoFar + chances.Length + 1];

        // Initialize current-win probability
        totals[winsSoFar] = 1;
        Calls = 0;
        AddGame(chances, 0, winsSoFar, totals);
        return totals;
    }

    // O(u²): process remaining games recursively
    // C6: recursive game step
    private static void AddGame(double[] chances, int index, int winsSoFar, double[] totals)
    {
        Calls++;

        // Base case: no games remain
        // C6: recursion base case
        if (index == chances.Length)
        {
            return;
        }
        double p = chances[index];

        // Combine win & loss probabilities
        // Descending updates preserve previous values
        for (int wins = winsSoFar + index + 1; wins > winsSoFar; wins--)
        {
            totals[wins] = totals[wins] * (1 - p) + totals[wins - 1] * p;
        }

        // Update lowest-win probability
        totals[winsSoFar] = totals[winsSoFar] * (1 - p);

        // Process next game
        AddGame(chances, index + 1, winsSoFar, totals);
    }
}
