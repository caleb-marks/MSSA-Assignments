namespace Caleb_Marks_Week_5_Challenge_Labs
{
    internal static class MissingNumber
    {
        // Read distinct values 0..n & display missing one
        public static void Run()
        {
            int count = Program.ReadCount();
            int[] nums = new int[count];
            HashSet<int> seenNumbers = new HashSet<int>();

            // Re-ask until value fits range & is new
            for (int i = 0; i < nums.Length; i++)
            {
                int number = Program.ReadInt("Enter number " + (i + 1) + " (0-" + count + "): ");
                while (number < 0 || number > count || seenNumbers.Contains(number))
                {
                    number = Program.ReadInt("Enter a new number from 0 to " + count + ": ");
                }

                seenNumbers.Add(number);
                nums[i] = number;
            }

            Console.WriteLine("Missing number: " + FindMissing(nums));
        }

        // Expected 0..n sum minus actual sum = missing value
        public static int FindMissing(int[] nums)
        {
            long n = nums.Length;
            long expectedSum = n * (n + 1) / 2;
            long actualSum = 0;
            foreach (int number in nums)
            {
                actualSum += number;
            }

            return (int)(expectedSum - actualSum);
        }
    }
}
