namespace Caleb_Marks_Assignment_2._4
{
    // Add the numbers in an array
    internal static class SumArray
    {
        // Ask for the numbers and display the sum
        public static void Run()
        {
            int count = Program.ReadInt("Input the number of elements to be stored in the array :");
            // Require at least one number
            while (count < 1)
            {
                count = Program.ReadInt("Enter a whole number greater than 0: ");
            }

            int[] values = new int[count];
            Console.WriteLine("Input " + count + " elements in the array :");

            // Use long so adding large integers does not overflow
            long sum = 0;
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = Program.ReadInt("element - " + i + " : ");
                sum = sum + values[i];
            }

            Console.WriteLine("Sum of all elements stored in the array is : " + sum);
        }
    }
}
