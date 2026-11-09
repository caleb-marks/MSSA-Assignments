namespace Caleb_Marks_Assignment_7._2
{
    internal static class ValidAnagram
    {
        // Read two strings & show whether anagram
        public static void Run()
        {
            string s = Program.ReadText("Enter first string: ");
            string t = Program.ReadText("Enter second string: ");
            bool isAnagram = IsAnagram(s, t);
            Console.WriteLine("Is anagram: " + isAnagram);
        }

        // Compare letter counts; equal counts mean anagram
        public static bool IsAnagram(string s, string t)
        {
            // Different lengths cannot be anagrams
            if (s.Length != t.Length)
            {
                return false;
            }

            // Count each letter seen, first string
            Dictionary<char, int> counts = new Dictionary<char, int>();
            for (int i = 0; i < s.Length; i++)
            {
                if (counts.ContainsKey(s[i]))
                {
                    counts[s[i]]++;
                }
                else
                {
                    counts[s[i]] = 1;
                }
            }

            // Subtract each letter count, second string
            for (int i = 0; i < t.Length; i++)
            {
                if (!counts.ContainsKey(t[i]) || counts[t[i]] == 0)
                {
                    return false;
                }

                counts[t[i]]--;
            }

            return true;
        }
    }
}
