namespace Caleb_Marks_Assignment_4._3
{
    internal static class ElectricityBill
    {
        // Calculate & print customer bill
        public static void Run()
        {
            // Read customer details & reject negative units
            string customerId = Program.ReadText("Enter customer ID (e.g. 1001): ");
            string customerName = Program.ReadText("Enter customer name (e.g. James): ");
            int units = Program.ReadInt("Enter units consumed (whole number): ");
            while (units < 0)
            {
                units = Program.ReadInt("Units cannot be negative. Enter 0 or more: ");
            }

            // Every consumed unit uses this rate
            decimal rate;
            if (units < 200)
            {
                rate = 1.20m;
            }
            else if (units < 400)
            {
                rate = 1.50m;
            }
            else if (units < 600)
            {
                rate = 1.80m;
            }
            else
            {
                rate = 2.00m;
            }

            decimal amount = units * rate;
            decimal surcharge = 0m;
            // Bills exceeding $400 receive 15% surcharge
            if (amount > 400m)
            {
                surcharge = amount * 0.15m;
            }

            // Show customer details, charges & final total
            Console.WriteLine();
            Console.WriteLine("Customer IDNO :" + customerId);
            Console.WriteLine("Customer Name :" + customerName);
            Console.WriteLine("unit Consumed :" + units);
            Console.WriteLine("Amount Charges @$ " + rate.ToString("F2") + " per unit : " + amount.ToString("F2"));
            Console.WriteLine("Surcharge Amount : " + surcharge.ToString("F2"));
            Console.WriteLine("Net Amount Paid By the Customer : " + (amount + surcharge).ToString("F2"));
        }
    }
}
