namespace Caleb_Marks_OU_Season_Analyzer.Logic;

// Stable merge sort
// Recursive divide & conquer
// O(n log n) time; O(n) storage
internal static class MergeSort
{
    // Sort existing collection
    // Reuse temporary array
    public static void Sort<T>(IList<T> items, Comparison<T> compare)
    {
        T[] temp = new T[items.Count];
        Split(items, temp, 0, items.Count, compare);
    }

    // Sort half-open range recursively
    private static void Split<T>(IList<T> items, T[] temp, int low, int high, Comparison<T> compare)
    {
        // Base case: fewer than two items
        if (high - low < 2)
        {
            return;
        }

        // Calculate safe midpoint
        int mid = low + (high - low) / 2;

        // Sort halves, then merge
        Split(items, temp, low, mid, compare);
        Split(items, temp, mid, high, compare);
        Merge(items, temp, low, mid, high, compare);
    }

    // O(n): merge sorted halves
    private static void Merge<T>(IList<T> items, T[] temp, int low, int mid, int high, Comparison<T> compare)
    {
        // Track each half's position
        int left = low;
        int right = mid;
        for (int k = low; k < high; k++)
        {
            // Choose smaller available item
            // Left-first ties preserve order
            if (right >= high || (left < mid && compare(items[left], items[right]) <= 0))
            {
                temp[k] = items[left];
                left++;
            }
            else
            {
                temp[k] = items[right];
                right++;
            }
        }

        // Copy merged values back
        for (int k = low; k < high; k++)
        {
            items[k] = temp[k];
        }
    }
}
