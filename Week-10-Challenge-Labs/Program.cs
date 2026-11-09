// Caleb Marks

namespace Caleb_Marks_Week_10_Challenge_Labs
{
    internal class Program
    {
        // Start menu & exit cleanly when input ends
        static void Main(string[] args)
        {
            try
            {
                RunMenu();
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine();
                Console.WriteLine("Goodbye!");
            }
        }

        // Repeat menu; Exit selection stops loop
        static void RunMenu()
        {
            Console.WriteLine("Greetings!");
            bool keepRunning = true;
            while (keepRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Week 10 Challenge Labs");
                Console.WriteLine("1. Find the added letter");
                Console.WriteLine("2. Merge two sorted arrays");
                Console.WriteLine("3. Exit");
                int option = ReadInt("Select an option (1-3): ");
                Console.WriteLine();

                // Route selected option
                switch (option)
                {
                    case 1:
                        FindTheDifference.Run();
                        break;

                    case 2:
                        MergeSortedArray.Run();
                        break;

                    case 3:
                        keepRunning = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Enter a number from 1 to 3.");
                        break;
                }
            }
        }

        // Read whole numbers & retry invalid input
        internal static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            int number;
            while (!Int32.TryParse(ReadInput(), out number))
            {
                Console.Write("Invalid input. Enter a whole number: ");
            }

            return number;
        }

        // Read one line; distinguish empty text & ended input
        internal static string ReadInput()
        {
            string? input = Console.ReadLine();
            if (input == null)
            {
                throw new EndOfStreamException();
            }

            return input;
        }
    }
}
