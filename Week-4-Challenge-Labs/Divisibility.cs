namespace Caleb_Marks_Week_4_Challenge_Labs
{
    // Apply challenge arithmetic rule
    internal static class Divisibility
    {
        // Read two numbers & display result
        public static void Run()
        {
            int num1 = Program.ReadInt("Enter the first whole number: ");
            int num2 = Program.ReadInt("Enter the second whole number: ");
            Console.WriteLine("Result: " + DivisibleBy2Or3(num1, num2));
        }

        // Multiply qualifying numbers or add remaining values
        public static long DivisibleBy2Or3(int num1, int num2)
        {
            // Check each number's divisibility
            if ((num1 % 2 == 0 || num1 % 3 == 0) &&
                (num2 % 2 == 0 || num2 % 3 == 0))
            {
                // Prevent product overflow
                return (long)num1 * num2;
            }

            // Prevent sum overflow
            return (long)num1 + num2;
        }
    }
}
