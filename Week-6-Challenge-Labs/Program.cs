// Caleb Marks

namespace Caleb_Marks_Week_6_Challenge_Labs
{
    internal class Program
    {
        // Repeat menu; Exit selection stops loop
        static void Main(string[] args)
        {
            Console.WriteLine("Greetings!");
            bool keepRunning = true;
            while (keepRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Week 6 Challenge Labs");
                Console.WriteLine("1. Rotate a square matrix clockwise");
                Console.WriteLine("2. Exit");
                int option = ReadInt("Select an option (1-2): ");
                Console.WriteLine();

                // Route selected option
                switch (option)
                {
                    case 1:
                        RotateImage.Run();
                        break;

                    case 2:
                        keepRunning = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Enter a number from 1 to 2.");
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
