// Caleb Marks

namespace Caleb_Marks_Assignment_6._2
{
    internal class Program
    {
        // Show assignment choices & repeat menu
        static void Main(string[] args)
        {
            Console.WriteLine("Greetings!");
            bool keepRunning = true;
            while (keepRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Assignment 6.2");
                Console.WriteLine("1. Stack using an array");
                Console.WriteLine("2. Product of array except self");
                Console.WriteLine("3. Exit");
                int option = ReadInt("Select an option (1-3): ");
                Console.WriteLine();

                // Run selected problem or exit
                switch (option)
                {
                    case 1:
                        StackProgram.Run();
                        break;

                    case 2:
                        ProductExceptSelf.Run();
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
            while (!Int32.TryParse(Console.ReadLine(), out number))
            {
                Console.Write("Invalid input. Enter a whole number: ");
            }

            return number;
        }
    }
}
