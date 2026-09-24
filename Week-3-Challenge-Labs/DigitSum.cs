namespace Caleb_Marks_Week_3_Challenge_Labs
{
    // Add individual digits and ignore other characters
    internal static class DigitSum
    {
        // Read the text and display the sum of its digits
        public static void Run()
        {
            Console.WriteLine("This adds the digits in your text. For example, 1q2w3e gives 6.");
            Console.Write("Enter text with single digits (e.g., 1q2w3e): ");
            string text = Console.ReadLine() ?? "";
            Console.WriteLine("Sum of digits: " + SumDigitsInString(text));
        }

        // Add the digits in the text
        public static int SumDigitsInString(string text)
        {
            int sum = 0;
            foreach (char character in text)
            {
                int digit;
                if (Int32.TryParse(character.ToString(), out digit))
                {
                    sum = sum + digit;
                }
            }

            return sum;
        }
    }
}
