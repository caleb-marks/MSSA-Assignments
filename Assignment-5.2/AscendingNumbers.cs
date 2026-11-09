namespace Caleb_Marks_Assignment_5._2
{
    internal static class AscendingNumbers
    {
        // Read count & display numbers upward
        public static void Run()
        {
            int count = Program.ReadInt("How many numbers to print: ");

            // Limit count so recursion cannot overflow
            while (count <= 0 || count > 10000)
            {
                count = Program.ReadInt("Enter a number from 1 to 10000: ");
            }

            PrintUpward(1, count);
            Console.WriteLine();
        }

        // Print numbers upward recursively
        public static void PrintUpward(int current, int max)
        {
            if (current > max)
            {
                return;
            }

            Console.Write(current + " ");
            PrintUpward(current + 1, max);
        }
    }
}
