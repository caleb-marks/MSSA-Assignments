namespace Caleb_Marks_Week_3_Challenge_Labs
{
    // Compare letters from opposite ends of a word
    internal static class Palindrome
    {
        // Read lowercase letters and display the result
        public static void Run()
        {
            Console.WriteLine("A palindrome reads the same forward and backward, like eye.");
            Console.Write("Enter a lowercase word (e.g., eye or home): ");
            string text = Console.ReadLine() ?? "";
            while (!HasOnlyLowercaseLetters(text))
            {
                Console.Write("Use only lowercase letters a-z, with no spaces (e.g., eye): ");
                text = Console.ReadLine() ?? "";
            }

            Console.WriteLine("Is palindrome: " + IsPalindrome(text));
        }

        // Check whether the letters match from both ends
        public static bool IsPalindrome(string text)
        {
            char[] letters = text.ToCharArray();
            for (int i = 0; i < letters.Length / 2; i++)
            {
                if (letters[i] != letters[letters.Length - 1 - i])
                {
                    return false;
                }
            }

            return true;
        }

        // Check that the text contains only lowercase letters
        private static bool HasOnlyLowercaseLetters(string text)
        {
            foreach (char letter in text)
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
