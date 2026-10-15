namespace Caleb_Marks_Week_2_Challenge_Labs
{
    // Display a number triangle
    internal static class NumberTriangle
    {
        // Use the number as the starting row width
        public static void Run()
        {
            int number = Program.ReadInt("Enter a number: ");

            // Print the number one fewer time on each row
            for (int row = number; row >= 1; row--)
            {
                for (int i = 0; i < row; i++)
                {
                    Console.Write(number);
                }

                Console.WriteLine();
            }
        }
    }
}
