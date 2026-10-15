namespace Caleb_Marks_Assignment_6._2
{
    internal static class ProductExceptSelf
    {
        // Read array, compute products, show input & output
        public static void Run()
        {
            int count = Program.ReadInt("Input number of elements: ");
            while (count < 2)
            {
                count = Program.ReadInt("Need at least 2 elements. Try again: ");
            }

            int[] nums = new int[count];
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = Program.ReadInt("element - [" + i + "] : ");
            }

            Console.WriteLine("Input:  " + Format(nums));
            Console.WriteLine("Output: " + Format(Solve(nums)));
        }

        // answer[i] = product left of i * product right of i; two passes, O(n), no division
        public static int[] Solve(int[] nums)
        {
            int[] answer = new int[nums.Length];

            // Pass 1: store product of everything left of i
            int left = 1;
            for (int i = 0; i < nums.Length; i++)
            {
                answer[i] = left;
                left *= nums[i];
            }

            // Pass 2: multiply in product of everything right of i
            int right = 1;
            for (int i = nums.Length - 1; i >= 0; i--)
            {
                answer[i] *= right;
                right *= nums[i];
            }

            return answer;
        }

        // Format array like [24,12,8,6]
        private static string Format(int[] nums)
        {
            return "[" + string.Join(",", nums) + "]";
        }
    }
}
