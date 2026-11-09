using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Assignment1_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Personal Details 
            string name = "Caleb"; 
            int age = 28;
            string address = "24 Kobe Bryant Street";
            
            Console.WriteLine("Personal details");
            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"Age: {age}");
            Console.WriteLine($"Address: {address}");
            Console.WriteLine();

            // Addition 
            Console.WriteLine("Addition");
            Console.WriteLine("Enter the first number.");
            int num1 = Int32.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter the second number.");
            int num2 = Int32.Parse(Console.ReadLine()!);
            int sum = num1 + num2;

            Console.WriteLine($"Sum: {sum}");
            Console.WriteLine();

            // Division 
            Console.WriteLine("Division");
            Console.WriteLine("Enter the dividend.");
            int dividend = Int32.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter a non-zero divisor.");
            int divisor = Int32.Parse(Console.ReadLine()!);
            int quotient = dividend / divisor;
            int remainder = dividend % divisor;

            Console.WriteLine($"Quotient: {quotient}");
            Console.WriteLine($"Remainder: {remainder}");

        }
    }
}
