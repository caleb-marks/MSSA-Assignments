namespace Caleb_Marks_Assignment_3._2
{
    // Show the array in rows and columns
    internal static class MatrixDisplay
    {
        // Print the assignment example
        public static void Run()
        {
            int[,] numbers = { { 2, 3, 4 }, { 1, 4, 6 } };
            Console.WriteLine("Example matrix:");
            PrintMatrix(numbers);
        }

        // Give each number 12 spaces so the columns line up
        public static void PrintMatrix(int[,] numbers)
        {
            for (int i = 0; i < numbers.GetLength(0); i++)
            {
                Console.Write("|");
                for (int j = 0; j < numbers.GetLength(1); j++)
                {
                    Console.Write($"{numbers[i, j],12} |");
                }
                Console.WriteLine();
            }
        }
    }
}
