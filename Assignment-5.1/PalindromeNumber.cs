namespace Caleb_Marks_Assignment_5._1
{
    internal static class PalindromeNumber
    {
        // Read number & display palindrome result
        public static void Run()
        {
            int number = Program.ReadInt("Enter a whole number: ");
            Console.WriteLine("Is palindrome: " + IsPalindrome(number));
        }

        // Reverse digits & compare values
        public static bool IsPalindrome(int number)
        {
            // Reject negative numbers
            if (number < 0)
            {
                return false;
            }

            int originalNumber = number;
            long reversedNumber = 0;

            // Reverse each digit
            while (number > 0)
            {
                int digit = number % 10;
                reversedNumber = reversedNumber * 10 + digit;
                number = number / 10;
            }

            return originalNumber == reversedNumber;
        }
    }
}
