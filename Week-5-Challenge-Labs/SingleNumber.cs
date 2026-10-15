namespace Caleb_Marks_Week_5_Challenge_Labs
{
    internal static class SingleNumber
    {
        // Read array & display number that appears once
        public static void Run()
        {
            int count = Program.ReadCount();
            int[] nums = new int[count];
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = Program.ReadInt("Enter number " + (i + 1) + ": ");
            }

            int? single = FindSingle(nums);
            if (single == null)
            {
                Console.WriteLine("No number appears exactly once.");
            }
            else
            {
                Console.WriteLine("Single number: " + single);
            }
        }

        // Count each value, then return one seen only once
        public static int? FindSingle(int[] nums)
        {
            Dictionary<int, int> counts = new Dictionary<int, int>();

            // Tally how often each value appears
            foreach (int number in nums)
            {
                if (counts.ContainsKey(number))
                {
                    counts[number]++;
                }
                else
                {
                    counts[number] = 1;
                }
            }

            // Return first value seen once
            foreach (int number in nums)
            {
                if (counts[number] == 1)
                {
                    return number;
                }
            }

            return null;
        }
    }
}
