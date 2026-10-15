namespace Caleb_Marks_Assignment_6._1
{
    internal static class MoveZeroes
    {
        // Read array, move zeroes, show input & output
        public static void Run()
        {
            int count = Program.ReadInt("Input number of elements: ");
            while (count <= 0)
            {
                count = Program.ReadInt("Count must be positive. Try again: ");
            }

            int[] nums = new int[count];
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = Program.ReadInt("element - [" + i + "] : ");
            }

            Console.Write("Input:  ");
            Print(nums);
            Move(nums);
            Console.Write("Output: ");
            Print(nums);
        }

        // Shift non-zeroes left keeping order, then zero rest; same array, O(n)
        public static void Move(int[] nums)
        {
            int writeIndex = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] != 0)
                {
                    nums[writeIndex] = nums[i];
                    writeIndex++;
                }
            }

            // Zero remaining slots
            for (int i = writeIndex; i < nums.Length; i++)
            {
                nums[i] = 0;
            }
        }

        // Print array, format [1,3,12,0,0]
        private static void Print(int[] nums)
        {
            Console.WriteLine("[" + string.Join(",", nums) + "]");
        }
    }
}
