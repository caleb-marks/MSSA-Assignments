namespace Caleb_Marks_Assignment_5._2
{
    internal static class DescendingNumbers
    {
        // Read count & display numbers downward
        public static void Run()
        {
            int count = Program.ReadInt("How many numbers to print: ");

            // Limit count so recursion cannot overflow
            while (count <= 0 || count > 10000)
            {
                count = Program.ReadInt("Enter a number from 1 to 10000: ");
            }

            PrintDownward(count);
            Console.WriteLine();
        }

        // Print numbers downward recursively
        public static void PrintDownward(int current)
        {
            if (current <= 0)
            {
                return;
            }

            Console.Write(current + " ");
            PrintDownward(current - 1);
        }
    }
}
