namespace Caleb_Marks_Assignment_7._1
{
    internal static class MergeStrings
    {
        // Read two words, merge alternately, & show result
        public static void Run()
        {
            string word1 = Program.ReadText("Enter first word: ");
            string word2 = Program.ReadText("Enter second word: ");
            string merged = Merge(word1, word2);
            Console.WriteLine("Merged: " + merged);
        }

        // Add letters alternately starting word1, then remaining letters
        public static string Merge(string word1, string word2)
        {
            string merged = "";
            int i = 0;
            while (i < word1.Length || i < word2.Length)
            {
                // Add word1 letter when available
                if (i < word1.Length)
                {
                    merged += word1[i];
                }

                // Add word2 letter when available
                if (i < word2.Length)
                {
                    merged += word2[i];
                }

                i++;
            }

            return merged;
        }
    }
}
