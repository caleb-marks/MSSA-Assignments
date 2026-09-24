namespace Caleb_Marks_Assignment_3._2
{
    // Send the total and average back to Main
    internal static class TotalAverage
    {
        // Ask for four numbers and send the total and average back to Main
        public static void Run(out double total, out double average)
        {
            bool valid;
            do
            {
                double num1 = Program.ReadDouble("Number 1 of 4: ");
                double num2 = Program.ReadDouble("Number 2 of 4: ");
                double num3 = Program.ReadDouble("Number 3 of 4: ");
                double num4 = Program.ReadDouble("Number 4 of 4: ");
                Calculate(out total, out average, num1, num2, num3, num4);
                valid = Program.IsFinite(total) && Program.IsFinite(average);
                if (!valid)
                {
                    Console.WriteLine("The calculation is too large. Enter four numbers again.");
                }
            }
            while (!valid);
        }

        // Use params to pass the numbers separately and out to send both results back
        public static void Calculate(out double total, out double average, params double[] numbers)
        {
            total = 0;
            average = 0;
            foreach (double number in numbers)
            {
                total = total + number;
            }

            // Only divide if there is at least one number
            if (numbers.Length > 0)
            {
                average = total / numbers.Length;
            }
        }
    }
}
