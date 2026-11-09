namespace Caleb_Marks_Week_3_Challenge_Labs
{
    // Remove AB and CD from the text
    internal static class StringReduction
    {
        // Read uppercase letters and display the smallest remaining length
        public static void Run()
        {
            Console.WriteLine("Removes AB and CD pairs and counts the letters left.");
            Console.Write("Enter capital letters, no spaces (e.g., ABFCACDB): ");
            string text = Console.ReadLine() ?? "";
            while (!HasOnlyUppercaseLetters(text))
            {
                Console.Write("Capital letters only, no spaces. Try again: ");
                text = Console.ReadLine() ?? "";
            }

            Console.WriteLine("Letters remaining: " + MinimumLength(text));
        }

        // Keep removing AB and CD until the length stops changing
        public static int MinimumLength(string text)
        {
            int previousLength;
            do
            {
                previousLength = text.Length;
                text = text.Replace("AB", "");
                text = text.Replace("CD", "");
            }
            while (text.Length != previousLength);

            return text.Length;
        }

        // Check that the text contains only uppercase letters
        private static bool HasOnlyUppercaseLetters(string text)
        {
            foreach (char letter in text)
            {
                if (letter < 'A' || letter > 'Z')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
