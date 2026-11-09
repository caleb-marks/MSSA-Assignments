namespace Caleb_Marks_Assignment_3._2
{
    // Add the numbers in matching spots in two matrices
    internal static class MatrixAddition
    {
        // Ask for two matrices and show them and their sum
        public static void Run()
        {
            int size = Program.ReadInt("Rows and columns in each square matrix (1-4): ");
            while (size < 1 || size > 4)
            {
                size = Program.ReadInt("Enter a size from 1 to 4: ");
            }

            Console.WriteLine();
            Console.WriteLine("Enter whole numbers for the first matrix:");
            int[,] first = ReadMatrix(size);
            Console.WriteLine();
            Console.WriteLine("Enter whole numbers for the second matrix:");
            int[,] second = ReadMatrix(size);

            Console.WriteLine();
            Console.WriteLine("First matrix:");
            MatrixDisplay.PrintMatrix(first);
            Console.WriteLine();
            Console.WriteLine("Second matrix:");
            MatrixDisplay.PrintMatrix(second);
            Console.WriteLine();
            Console.WriteLine("Sum:");
            for (int i = 0; i < size; i++)
            {
                Console.Write("|");
                for (int j = 0; j < size; j++)
                {
                    // Use double before adding in case the sum is too big for int
                    double cellSum = (double)first[i, j] + second[i, j];
                    Console.Write($"{cellSum,12} |");
                }
                Console.WriteLine();
            }
        }

        // Ask for each number by its row and column
        public static int[,] ReadMatrix(int size)
        {
            int[,] numbers = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    numbers[i, j] = Program.ReadInt($"Row {i + 1}, column {j + 1}: ");
                }
            }

            return numbers;
        }
    }
}
