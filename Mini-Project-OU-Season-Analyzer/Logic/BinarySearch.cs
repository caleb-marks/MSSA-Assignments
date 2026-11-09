using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Logic;

// Opponent prefix search
// Sorted input required
internal static class BinarySearch
{
    // O(n log n): sort opponent names
    // Array indexing supports constant-time midpoint access
    public static Game[] SortByName(List<Game> games)
    {
        Game[] sorted = games.ToArray();
        MergeSort.Sort(sorted, CompareGames);
        return sorted;
    }

    // Compare opponent names, then dates
    private static int CompareGames(Game a, Game b)
    {
        int byName = string.Compare(a.Opponent, b.Opponent, StringComparison.OrdinalIgnoreCase);
        if (byName != 0)
        {
            return byName;
        }
        return a.Date.CompareTo(b.Date);
    }

    // O(log n + k): collect prefix matches
    // k = match count
    public static List<Game> Find(Game[] sorted, string text)
    {
        List<Game> found = new List<Game>();

        // Normalize search text
        string query = text.Trim();
        if (query.StartsWith("at ", StringComparison.OrdinalIgnoreCase))
        {
            query = query.Substring(3).Trim();
        }

        // Reject empty query
        if (query.Length == 0)
        {
            return found;
        }

        // Locate first match
        int first = FirstMatch(sorted, query);
        if (first < 0)
        {
            return found;
        }

        // Collect adjacent matches
        for (int i = first; i < sorted.Length && ComparePrefix(sorted[i].Opponent, query) == 0; i++)
        {
            found.Add(sorted[i]);
        }
        return found;
    }

    // O(log n): locate earliest match
    // C6: halve search range
    private static int FirstMatch(Game[] sorted, string query)
    {
        // Initialize search bounds
        int low = 0;
        int high = sorted.Length - 1;
        int first = -1;
        while (low <= high)
        {
            // Compare midpoint
            int mid = low + (high - low) / 2;
            int result = ComparePrefix(sorted[mid].Opponent, query);

            // Search right half
            if (result < 0)
            {
                low = mid + 1;
            }
            else
            {
                // Remember matching index
                if (result == 0)
                {
                    first = mid;
                }

                // Search earlier matches
                high = mid - 1;
            }
        }
        return first;
    }

    // Compare prefixes ignoring case
    private static int ComparePrefix(string name, string query)
    {
        return string.Compare(name, 0, query, 0, query.Length, StringComparison.OrdinalIgnoreCase);
    }
}
