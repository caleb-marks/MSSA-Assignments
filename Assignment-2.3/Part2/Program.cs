// Caleb Marks

namespace Caleb_Marks_Assignment_2._3_Part2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: TipCalculator <bill total> <tip percentage>");
                return;
            }

            double billAmount;
            if (!Double.TryParse(args[0], out billAmount) || billAmount < 0)
            {
                Console.WriteLine("The bill total must be a valid nonnegative number.");
                return;
            }

            double tipPercentage;
            if (!Double.TryParse(args[1], out tipPercentage) || tipPercentage < 0)
            {
                Console.WriteLine("The tip percentage must be a valid nonnegative number.");
                return;
            }

            double tipRate = tipPercentage / 100;
            double tipAmount = billAmount * tipRate;
            double grandTotal = billAmount + tipAmount;

            Console.WriteLine();
            Console.WriteLine("***************************************");
            Console.WriteLine("Bill Amount   :          " + billAmount.ToString("c"));
            Console.WriteLine("Tip Amount    :          " + tipAmount.ToString("c") + " (" + tipRate.ToString("p1") + ")");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Grand Total:      " + grandTotal.ToString("c"));
        }
    }
}
