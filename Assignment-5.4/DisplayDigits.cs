namespace Caleb_Marks_Assignment_5._4
{
    internal static class DisplayDigits
    {
        // Read number & display digits
        public static void Run()
        {
            int number = Program.ReadInt("Input any number: ");
            Console.Write("The digits in the number " + number + " are : ");
            long magnitude = Math.Abs((long)number);
            PrintDigits(magnitude);
            Console.WriteLine();
        }

        // Print leading digits first, then last digit
        public static void PrintDigits(long number)
        {
            if (number >= 10)
            {
                PrintDigits(number / 10);
                Console.Write(" ");
            }

            Console.Write(number % 10);
        }
    }
}
