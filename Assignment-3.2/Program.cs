// Caleb Marks

namespace Caleb_Marks_Assignment_3._2
{
    // Show the assignment menu
    internal class Program
    {
        // Start the program here
        static void Main(string[] args)
        {
            Console.WriteLine("Greetings!");
            Console.WriteLine();

            // Repeat the menu until the user picks Exit
            int option;
            do
            {
                Console.WriteLine("Assignment 3.2");
                Console.WriteLine("--------------");
                Console.WriteLine("1. Show an example matrix");
                Console.WriteLine("2. Add two matrices");
                Console.WriteLine("3. Add and subtract circle areas");
                Console.WriteLine("4. Find the total and average");
                Console.WriteLine("5. Find a number in an array");
                Console.WriteLine("6. Exit");
                Console.WriteLine();
                option = ReadInt("Choose an option (1-6): ");
                Console.WriteLine();

                // Run the option the user picked
                switch (option)
                {
                    case 1:
                        MatrixDisplay.Run();
                        break;
                    case 2:
                        MatrixAddition.Run();
                        break;
                    case 3:
                        CircleAreas.Run();
                        break;
                    case 4:
                        double total;
                        double average;
                        TotalAverage.Run(out total, out average);
                        Console.WriteLine();
                        Console.WriteLine("Total: " + total);
                        Console.WriteLine("Average: " + average);
                        break;
                    case 5:
                        ArraySearch.Run();
                        break;
                    case 6:
                        Console.WriteLine("See ya later!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Choose a number from 1 to 6.");
                        break;
                }

                Console.WriteLine();
            }
            while (option != 6);
        }

        // Ask for a whole number and check the input
        internal static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            int number;

            while (!Int32.TryParse(Console.ReadLine(), out number))
            {
                Console.Write("Enter a whole number: ");
            }

            return number;
        }

        // Ask for a number and reject NaN (not a number) and infinity
        internal static double ReadDouble(string prompt)
        {
            Console.Write(prompt);
            double number;
            while (!Double.TryParse(Console.ReadLine(), out number) || !IsFinite(number))
            {
                Console.Write("Enter a valid number within range: ");
            }

            return number;
        }

        // Check that the number is within the range double can store
        internal static bool IsFinite(double number)
        {
            return number >= Double.MinValue && number <= Double.MaxValue;
        }
    }
}
