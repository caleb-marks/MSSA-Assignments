namespace Caleb_Marks_Assignment_5._3
{
    internal static class ClimbingStairs
    {
        // Read stair count & show distinct ways
        public static void Run()
        {
            int steps = Program.ReadInt("How many steps? ");

            // long holds every count through 91 steps
            while (steps <= 0 || steps > 91)
            {
                steps = Program.ReadInt("Enter a number from 1 to 91: ");
            }

            Console.WriteLine("Ways to climb: " + CountWays(steps));
        }

        // Dynamic programming: each stair count = sum of previous two counts
        public static long CountWays(int steps)
        {
            long[] ways = new long[steps + 1];
            ways[0] = 1;
            ways[1] = 1;
            for (int i = 2; i <= steps; i++)
            {
                ways[i] = ways[i - 1] + ways[i - 2];
            }

            return ways[steps];
        }
    }
}
