namespace Caleb_Marks_Assignment_7._2
{
    internal static class ReverseVowels
    {
        // Read text, reverse its vowels, & show result
        public static void Run()
        {
            string text = Program.ReadText("Enter text: ");
            string reversed = Reverse(text);
            Console.WriteLine("Output: " + reversed);
        }

        // Swap vowels starting both ends moving inward
        public static string Reverse(string text)
        {
            char[] letters = text.ToCharArray();
            int left = 0;
            int right = letters.Length - 1;
            while (left < right)
            {
                // Skip non-vowels, left side
                while (left < right && !IsVowel(letters[left]))
                {
                    left++;
                }

                // Skip non-vowels, right side
                while (left < right && !IsVowel(letters[right]))
                {
                    right--;
                }

                // Swap both vowels
                char temp = letters[left];
                letters[left] = letters[right];
                letters[right] = temp;
                left++;
                right--;
            }

            return new string(letters);
        }

        // Check whether character is vowel, both cases
        private static bool IsVowel(char letter)
        {
            char lower = Char.ToLower(letter);
            return lower == 'a' || lower == 'e' || lower == 'i' || lower == 'o' || lower == 'u';
        }
    }
}
