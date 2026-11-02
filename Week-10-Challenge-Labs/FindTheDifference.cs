namespace Caleb_Marks_Week_10_Challenge_Labs
{
    internal static class FindTheDifference
    {
        // Read both strings, find added letter, & display result
        public static void Run()
        {
            string s = ReadLowercase("Enter string s (lowercase letters, can be empty): ");
            string t = ReadLowercase("Enter string t (s shuffled plus one letter): ");

            // Retry t; t needs all s letters & one extra letter
            while (t.Length != s.Length + 1 || FindAddedLetter(s, t) == ' ')
            {
                Console.WriteLine("t must contain every letter of s plus exactly one added letter.");
                t = ReadLowercase("Enter string t: ");
            }

            Console.WriteLine("Added letter: " + FindAddedLetter(s, t));
        }

        // Count letters & find extra one; O(n) time, O(1) extra space
        public static char FindAddedLetter(string s, string t)
        {
            int[] counts = new int[26];

            // Add t letter counts, then subtract s letter counts
            foreach (char letter in t)
            {
                counts[letter - 'a']++;
            }
            foreach (char letter in s)
            {
                counts[letter - 'a']--;
            }

            // Leftover count marks added letter; negative count means t missing letter
            char added = ' ';
            for (int i = 0; i < counts.Length; i++)
            {
                if (counts[i] < 0)
                {
                    return ' ';
                }
                if (counts[i] == 1)
                {
                    added = (char)('a' + i);
                }
            }

            return added;
        }

        // Read lowercase letters & retry invalid input
        static string ReadLowercase(string prompt)
        {
            Console.Write(prompt);
            string input = Program.ReadInput();
            while (!IsLowercase(input))
            {
                Console.Write("Invalid input. Enter lowercase letters only: ");
                input = Program.ReadInput();
            }

            return input;
        }

        // Check every character is lowercase letter
        static bool IsLowercase(string input)
        {
            foreach (char letter in input)
            {
                if (letter < 'a' || letter > 'z')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
