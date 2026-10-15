namespace Caleb_Marks_Assignment_5._1
{
    internal static class ContainsDuplicate
    {
        // Read array & display duplicate result
        public static void Run()
        {
            int count = Program.ReadInt("How many numbers? ");
            while (count <= 0)
            {
                count = Program.ReadInt("Enter a number greater than 0: ");
            }

            int[] nums = new int[count];
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = Program.ReadInt("Enter number " + (i + 1) + ": ");
            }

            Console.WriteLine("Contains duplicate: " + HasDuplicate(nums));
        }

        // Return true once repeat appears
        public static bool HasDuplicate(int[] nums)
        {
            HashSet<int> seenNumbers = new HashSet<int>();

            // Track seen values
            foreach (int number in nums)
            {
                if (!seenNumbers.Add(number))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
