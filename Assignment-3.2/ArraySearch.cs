namespace Caleb_Marks_Assignment_3._2
{
    // Find where a number first appears in the array
    internal static class ArraySearch
    {
        // Ask for the numbers and which one to find
        public static void Run()
        {
            int count = Program.ReadInt("How many numbers? (0 or more): ");
            while (count < 0)
            {
                count = Program.ReadInt("Enter a count of 0 or more: ");
            }

            // Store each number in the array
            int[] numbers = new int[count];
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = Program.ReadInt("Number " + (i + 1) + " of " + count + ": ");
            }
            Console.WriteLine();
            int target = Program.ReadInt("Number to find: ");
            Console.WriteLine();
            Console.WriteLine("Index: " + Search(numbers, target));
            Console.WriteLine("Indexes start at 0. -1 means the number was not found.");
        }

        // Return the first matching index, or -1 if the number is missing
        public static int Search(int[] numbers, int target)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] == target)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
