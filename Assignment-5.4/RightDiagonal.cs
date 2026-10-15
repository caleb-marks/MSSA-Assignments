namespace Caleb_Marks_Assignment_5._4
{
    internal static class RightDiagonal
    {
        // Read square matrix & display diagonal sum
        public static void Run()
        {
            int size = Program.ReadInt("Input the size of the square matrix: ");
            while (size <= 0)
            {
                size = Program.ReadInt("Enter a size greater than 0: ");
            }

            int[,] matrix = new int[size, size];
            Console.WriteLine("Input elements in the matrix:");
            for (int row = 0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    matrix[row, column] = Program.ReadInt("element - [" + row + "],[" + column + "]: ");
                }
            }

            Console.WriteLine("The matrix is :");
            for (int row = 0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    if (column > 0)
                    {
                        Console.Write(" ");
                    }

                    Console.Write(matrix[row, column]);
                }

                Console.WriteLine();
            }

            Console.WriteLine("Addition of the right Diagonal elements is :" + SumRightDiagonal(matrix));
        }

        // Add matching row & column positions
        public static long SumRightDiagonal(int[,] matrix)
        {
            long sum = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                sum += matrix[i, i];
            }

            return sum;
        }
    }
}
