// Caleb Marks

namespace Caleb_Marks_Week_5_Challenge_Labs
{
    // Run challenge menu
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
                Console.WriteLine("Week 5 Challenge Labs");
                Console.WriteLine("1. Find the single number");
                Console.WriteLine("2. Find the missing number");
                Console.WriteLine("3. Exit");
                int option = ReadInt("Select an option (1-3): ");
                Console.WriteLine();

                // Route selected option
                switch (option)
                {
                    case 1:
                        SingleNumber.Run();
                        break;

                    case 2:
                        MissingNumber.Run();
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

        // Read positive array size
        internal static int ReadCount()
        {
            int count = ReadInt("How many numbers? ");
            while (count <= 0)
            {
                count = ReadInt("Enter a number greater than 0: ");
            }

            return count;
        }
    }
}
