namespace Caleb_Marks_Week_4_Challenge_Labs
{
    // Check number digits mathematically
    internal static class ContainsThree
    {
        // Read positive number & display result
        public static void Run()
        {
            int number = Program.ReadInt("Enter a positive whole number: ");

            // Require positive input
            while (number <= 0)
            {
                number = Program.ReadInt("Enter a number greater than zero: ");
            }

            Console.WriteLine("Contains 3: " + IfNumberContains3(number));
        }

        // Check each digit
        public static bool IfNumberContains3(int number)
        {
            while (number > 0)
            {
                // Check last digit & remove it
                if (number % 10 == 3)
                {
                    return true;
                }

                number /= 10;
            }

            return false;
        }
    }
}
