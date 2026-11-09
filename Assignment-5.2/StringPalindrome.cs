namespace Caleb_Marks_Assignment_5._2
{
    internal static class StringPalindrome
    {
        // Read text & display palindrome result
        public static void Run()
        {
            Console.Write("Input a string: ");
            string text = Console.ReadLine() ?? "";

            // Limit recursive calls to protect stack
            while (text.Length > 10000)
            {
                Console.Write("Enter 10000 characters or fewer: ");
                text = Console.ReadLine() ?? "";
            }

            if (IsPalindrome(text, 0, text.Length - 1))
            {
                Console.WriteLine("The string is Palindrome.");
            }
            else
            {
                Console.WriteLine("The string is not Palindrome.");
            }
        }

        // Compare ends then check middle recursively
        public static bool IsPalindrome(string text, int left, int right)
        {
            if (left >= right)
            {
                return true;
            }

            if (text[left] != text[right])
            {
                return false;
            }

            return IsPalindrome(text, left + 1, right - 1);
        }
    }
}
