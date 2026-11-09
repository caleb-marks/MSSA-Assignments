namespace Caleb_Marks_Week_10_Challenge_Labs
{
    internal static class MergeSortedArray
    {
        // Read both arrays, merge, & display results
        public static void Run()
        {
            int m = ReadCount("Enter m (number of values in nums1): ");
            int n = ReadCount("Enter n (number of values in nums2): ");

            // Require one or more values
            while (m + n == 0)
            {
                Console.WriteLine("m and n cannot both be 0.");
                m = ReadCount("Enter m: ");
                n = ReadCount("Enter n: ");
            }

            // nums1 size equals m + n; final n zeros leave nums2 room
            int[] nums1 = new int[m + n];
            Console.WriteLine("Enter nums1 values in non-decreasing order.");
            ReadSorted(nums1, m);
            int[] nums2 = new int[n];
            Console.WriteLine("Enter nums2 values in non-decreasing order.");
            ReadSorted(nums2, n);

            Console.Write("nums1 before merge: ");
            Display(nums1);
            Console.Write("nums2: ");
            Display(nums2);
            Merge(nums1, m, nums2, n);
            Console.Write("nums1 after merge: ");
            Display(nums1);
        }

        // Fill nums1 backward; O(m + n) time, O(1) extra space
        public static void Merge(int[] nums1, int m, int[] nums2, int n)
        {
            int i = m - 1;
            int j = n - 1;
            int write = m + n - 1;

            // Place larger remaining value last while nums2 has values
            while (j >= 0)
            {
                if (i >= 0 && nums1[i] > nums2[j])
                {
                    nums1[write] = nums1[i];
                    i--;
                }
                else
                {
                    nums1[write] = nums2[j];
                    j--;
                }
                write--;
            }
        }

        // Read count & keep it 0-200
        static int ReadCount(string prompt)
        {
            int count = Program.ReadInt(prompt);
            while (count < 0 || count > 200)
            {
                count = Program.ReadInt("Enter a number from 0 to 200: ");
            }

            return count;
        }

        // Read first count values; reject decreasing values
        static void ReadSorted(int[] values, int count)
        {
            for (int i = 0; i < count; i++)
            {
                values[i] = Program.ReadInt($"Value {i + 1}: ");
                while (i > 0 && values[i] < values[i - 1])
                {
                    values[i] = Program.ReadInt("Values must not decrease. Enter " + values[i - 1] + " or greater: ");
                }
            }
        }

        // Print array values
        static void Display(int[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Console.Write(values[i] + " ");
            }
            Console.WriteLine();
        }
    }
}
