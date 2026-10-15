namespace Caleb_Marks_Assignment_5._1
{
    internal static class DigitSum
    {
        // Read number & display digit sum
        public static void Run()
        {
            int number = Program.ReadInt("Enter a number: ");
            int sum = SumDigits(number);
            Console.WriteLine("The sum of the digits of the number " + number + " is : " + sum);
        }

        // Add each digit
        public static int SumDigits(int number)
        {
            long remainingNumber = number;
            if (remainingNumber < 0)
            {
                remainingNumber = -remainingNumber;
            }

            int sum = 0;

            // Add digits individually
            while (remainingNumber > 0)
            {
                sum = sum + (int)(remainingNumber % 10);
                remainingNumber = remainingNumber / 10;
            }

            return sum;
        }
    }
}
