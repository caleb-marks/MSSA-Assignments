namespace Caleb_Marks_Week_11_Challenge_Labs
{
    internal static class SortColors
    {
        // Read colors, sort & display result
        public static void Run()
        {
            int count = Program.ReadInt("Enter number of colors (1-100): ");

            // Require 1-100 colors
            while (count < 1 || count > 100)
            {
                count = Program.ReadInt("Enter a count from 1 to 100: ");
            }

            // Fill array, accept only 0, 1, or 2
            int[] nums = new int[count];
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = Program.ReadInt("Enter color " + (i + 1) + " (0 red, 1 white, 2 blue): ");
                while (nums[i] < 0 || nums[i] > 2)
                {
                    nums[i] = Program.ReadInt("Color must be 0, 1, or 2. Enter color " + (i + 1) + ": ");
                }
            }

            Console.WriteLine("Original: " + FormatArray(nums));
            Sort(nums);
            Console.WriteLine("Sorted: " + FormatArray(nums));
        }

        // Sort same array, one pass, no library sort
        public static void Sort(int[] nums)
        {
            int low = 0;
            int current = 0;
            int high = nums.Length - 1;

            // Move red left, blue right, keep white middle
            while (current <= high)
            {
                if (nums[current] == 0)
                {
                    Swap(nums, low, current);
                    low++;
                    current++;
                }
                else if (nums[current] == 2)
                {
                    // Recheck swapped value
                    Swap(nums, current, high);
                    high--;
                }
                else
                {
                    current++;
                }
            }
        }

        // Swap two array values
        static void Swap(int[] nums, int first, int second)
        {
            int temp = nums[first];
            nums[first] = nums[second];
            nums[second] = temp;
        }

        // Format array values
        static string FormatArray(int[] nums)
        {
            string text = "[";
            for (int i = 0; i < nums.Length; i++)
            {
                text += nums[i];

                // Separate values, skip last
                if (i < nums.Length - 1)
                {
                    text += ",";
                }
            }

            return text + "]";
        }
    }
}
