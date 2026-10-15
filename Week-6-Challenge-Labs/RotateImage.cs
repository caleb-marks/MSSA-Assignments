namespace Caleb_Marks_Week_6_Challenge_Labs
{
    internal static class RotateImage
    {
        // Read square matrix, rotate, & display results
        public static void Run()
        {
            int size = Program.ReadInt("Enter the number of rows and columns: ");

            // Reject empty or negative dimensions
            while (size <= 0)
            {
                size = Program.ReadInt("Enter a number greater than 0: ");
            }

            int[,] matrix = new int[size, size];

            // Fill each row
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = Program.ReadInt($"Row {i + 1}, column {j + 1}: ");
                }
            }

            Console.WriteLine("Original matrix:");
            Display(matrix);
            Rotate(matrix);
            Console.WriteLine("Rotated matrix (90 degrees clockwise):");
            Display(matrix);
        }

        // Rotate same matrix; O(n squared) time, O(1) extra space
        public static void Rotate(int[,] matrix)
        {
            int size = matrix.GetLength(0);

            // Transpose: swap row & column positions once
            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    int temp = matrix[i, j];
                    matrix[i, j] = matrix[j, i];
                    matrix[j, i] = temp;
                }
            }

            // Reverse each row: swap opposite columns
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size / 2; j++)
                {
                    int temp = matrix[i, j];
                    matrix[i, j] = matrix[i, size - 1 - j];
                    matrix[i, size - 1 - j] = temp;
                }
            }
        }

        // Print matrix rows
        public static void Display(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }

                Console.WriteLine();
            }
        }
    }
}
