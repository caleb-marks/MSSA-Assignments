namespace Caleb_Marks_Assignment_4._3
{
    internal static class ArrayCounts
    {
        // Count repeated numbers or show unrepeated numbers
        public static void Run(bool uniqueOnly)
        {
            // Require positive array size
            int count = Program.ReadInt("How many numbers will you enter? (1 or more): ");
            while (count <= 0)
            {
                count = Program.ReadInt("Count must be at least 1. Try again: ");
            }

            int[] numbers = new int[count];
            // Dictionary pairs each number & its occurrence count
            Dictionary<int, int> frequencies = new Dictionary<int, int>();
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = Program.ReadInt("Enter number " + (i + 1) + " of " + count + ": ");
                // Increase existing count or start new count
                if (frequencies.ContainsKey(numbers[i]))
                {
                    frequencies[numbers[i]]++;
                }
                else
                {
                    frequencies.Add(numbers[i], 1);
                }
            }

            Console.WriteLine();
            if (uniqueOnly)
            {
                Console.WriteLine("The unique elements found in the array are :");
                bool found = false;
                // Print only numbers appearing once
                foreach (int number in numbers)
                {
                    if (frequencies[number] == 1)
                    {
                        Console.WriteLine(number);
                        found = true;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("No unique elements found.");
                }
            }
            else
            {
                Console.WriteLine("Frequency of all elements of array :");
                // Print each number & its count
                foreach (int number in frequencies.Keys)
                {
                    Console.WriteLine(number + " occurs " + frequencies[number] + " times");
                }
            }
        }
    }
}
