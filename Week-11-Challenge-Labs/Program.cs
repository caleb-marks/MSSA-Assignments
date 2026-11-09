// Caleb Marks

namespace Caleb_Marks_Week_11_Challenge_Labs
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
            bool keepRunning = true;
            while (keepRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Week 11 Challenge Labs");
                Console.WriteLine("1. Sort colors");
                Console.WriteLine("2. Count balloon instances");
                Console.WriteLine("3. Exit");
                int option = ReadInt("Select an option (1-3): ");
                Console.WriteLine();

                // Route selected option
                switch (option)
                {
                    case 1:
                        SortColors.Run();
                        break;

                    case 2:
                        MaxBalloons.Run();
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

        // Read one line; stop when input ends
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
