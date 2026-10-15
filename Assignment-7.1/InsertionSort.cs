namespace Caleb_Marks_Assignment_7._1
{
    internal static class InsertionSort
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

        // Hold each score, shift bigger scores right, then place key
        public static void Sort(int[] scores)
        {
            for (int i = 1; i < scores.Length; i++)
            {
                // Keep current score safe while shifting
                int key = scores[i];
                int j = i - 1;
                while (j >= 0 && scores[j] > key)
                {
                    scores[j + 1] = scores[j];
                    j--;
                }

                scores[j + 1] = key;
            }
        }

        // Print scores, format [90, 85, 77]
        private static void Print(int[] scores)
        {
            Console.WriteLine("[" + string.Join(", ", scores) + "]");
        }
    }
}
