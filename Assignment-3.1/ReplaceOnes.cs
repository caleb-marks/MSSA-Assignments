namespace Caleb_Marks_Assignment_3._1
{
    // Replace only the first adjacent pair of ones
    internal static class ReplaceOnes
    {
        // Read an array and print the changed values
        public static void Run()
        {
            int count = Program.ReadInt("How many numbers? Enter 0 or more (e.g., 7): ");
            while (count < 0)
            {
                count = Program.ReadInt("Enter a count of 0 or more (e.g., 7): ");
            }

            // Fill the array one element at a time
            int[] numbers = new int[count];
            for (int i = 0; i < numbers.Length; i = i + 1)
            {
                numbers[i] = Program.ReadInt("Number " + (i + 1) + " of " + count + " (e.g., 1): ");
            }

            ReplaceFirstPair(numbers);
            Console.Write("Result: [");
            for (int i = 0; i < numbers.Length; i = i + 1)
            {
                if (i > 0)
                {
                    Console.Write(",");
                }
                Console.Write(numbers[i]);
            }
            Console.WriteLine("]");
        }

        // Stop immediately after changing the first pair
        public static void ReplaceFirstPair(int[] numbers)
        {
            for (int i = 0; i < numbers.Length - 1; i = i + 1)
            {
                if (numbers[i] == 1 && numbers[i + 1] == 1)
                {
                    numbers[i] = 0;
                    numbers[i + 1] = 0;
                    break;
                }
            }
        }
    }
}
