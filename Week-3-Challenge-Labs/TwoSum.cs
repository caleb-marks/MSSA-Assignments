namespace Caleb_Marks_Week_3_Challenge_Labs
{
    // Find two numbers that add to the target
    internal static class TwoSum
    {
        // Read the array and target from the user
        public static void Run()
        {
            Console.WriteLine("Enter numbers one at a time, pressing Enter after each number.");
            Console.WriteLine("For example, enter 4 numbers: 2, 7, 11, 15. A target of 9 matches 2 + 7.");
            int count = Program.ReadInt("How many numbers will you enter? Enter 2 or more (e.g., 4): ");
            while (count < 2)
            {
                count = Program.ReadInt("Enter a count of 2 or more: ");
            }

            int[] numbers = new int[count];
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = Program.ReadInt("Number " + (i + 1) + " of " + count + ": ");
            }

            int target = Program.ReadInt("What should the two numbers add up to (e.g., 9)? ");
            int[] indices = FindIndices(numbers, target);
            if (indices.Length == 2)
            {
                Console.WriteLine("Positions start at 0, so 0 is the first number and 1 is the second.");
                Console.WriteLine("Indices: [" + indices[0] + ", " + indices[1] + "]");
            }
            else
            {
                Console.WriteLine("No two different positions add to that target.");
            }
        }

        // Start the second index after the first so an element is never reused
        public static int[] FindIndices(int[]? numbers, int target)
        {
            // Check that the array has at least two numbers
            if (numbers == null || numbers.Length < 2)
            {
                return new int[0];
            }

            for (int i = 0; i < numbers.Length; i++)
            {
                for (int j = i + 1; j < numbers.Length; j++)
                {
                    // Use double so the sum can hold large numbers
                    double sum = (double)numbers[i] + numbers[j];
                    if (sum == target)
                    {
                        return new int[] { i, j };
                    }
                }
            }

            return new int[0];
        }
    }
}
