namespace Caleb_Marks_Assignment_7._2
{
    internal static class MergeSort
    {
        // Read numbers, sort, & display results
        public static void Run()
        {
            int count = Program.ReadInt("How many numbers? ");
            while (count <= 0)
            {
                count = Program.ReadInt("Enter a count greater than zero: ");
            }

            int[] nums = new int[count];
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = Program.ReadInt("Enter number " + (i + 1) + ": ");
            }

            Console.Write("Unsorted: ");
            Print(nums);
            Divide(nums, 0, nums.Length - 1);
            Console.Write("Sorted:   ");
            Print(nums);
        }

        // Split range, sort each half, then merge; divide & conquer
        public static void Divide(int[] nums, int left, int right)
        {
            // Keep splitting while range holds 2+ values
            if (left < right)
            {
                int mid = (left + right) / 2;
                Divide(nums, left, mid);
                Divide(nums, mid + 1, right);
                Merge(nums, left, mid, right);
            }
        }

        // Merge both sorted halves; equal values keep order (stable)
        private static void Merge(int[] nums, int left, int mid, int right)
        {
            int i = left;
            int j = mid + 1;
            int k = left;
            int[] temp = new int[right + 1];

            // Pick smaller front value while both halves have values
            while (i <= mid && j <= right)
            {
                if (nums[i] <= nums[j])
                {
                    temp[k] = nums[i];
                    i++;
                }
                else
                {
                    temp[k] = nums[j];
                    j++;
                }

                k++;
            }

            // Copy leftover left values
            while (i <= mid)
            {
                temp[k] = nums[i];
                i++;
                k++;
            }

            // Copy leftover right values
            while (j <= right)
            {
                temp[k] = nums[j];
                j++;
                k++;
            }

            // Copy merged values back, original array
            for (int x = left; x <= right; x++)
            {
                nums[x] = temp[x];
            }
        }

        // Print numbers, format [5, 3, 4, 1, 2]
        private static void Print(int[] nums)
        {
            Console.WriteLine("[" + string.Join(", ", nums) + "]");
        }
    }
}
