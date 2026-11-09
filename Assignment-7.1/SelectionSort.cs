namespace Caleb_Marks_Assignment_7._1
{
    internal static class SelectionSort
    {
        // Read scores, sort, & display results
        public static void Run()
        {
            int count = Program.ReadInt("How many scores? ");
            while (count <= 0)
            {
                count = Program.ReadInt("Enter a count greater than zero: ");
            }

            int[] scores = new int[count];
            for (int i = 0; i < scores.Length; i++)
            {
                scores[i] = Program.ReadInt("Enter score " + (i + 1) + ": ");
            }

            Console.Write("Unsorted: ");
            Print(scores);
            Sort(scores);
            Console.Write("Sorted:   ");
            Print(scores);
        }

        // Find smallest unsorted score & swap positions
        public static void Sort(int[] scores)
        {
            int minPosition = 0;
            for (int i = 0; i < scores.Length - 1; i++)
            {
                // Assume current spot holds smallest so far
                minPosition = i;
                for (int j = i + 1; j < scores.Length; j++)
                {
                    if (scores[j] < scores[minPosition])
                    {
                        minPosition = j;
                    }
                }

                // Swap only when smaller score found
                if (minPosition != i)
                {
                    int temp = scores[i];
                    scores[i] = scores[minPosition];
                    scores[minPosition] = temp;
                }
            }
        }

        // Print scores, format [90, 85, 77]
        private static void Print(int[] scores)
        {
            Console.WriteLine("[" + string.Join(", ", scores) + "]");
        }
    }
}
