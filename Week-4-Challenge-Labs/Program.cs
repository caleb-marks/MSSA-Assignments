// Caleb Marks

namespace Caleb_Marks_Week_4_Challenge_Labs
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
                Console.WriteLine("1. Check whether a number contains 3");
                Console.WriteLine("2. Multiply or add two numbers");
                Console.WriteLine("3. Reverse your text");
                Console.WriteLine("4. Exit");
                int option = ReadInt("Select an option (1-4): ");

                // Route selected option
                switch (option)
                {
                    case 1:
                        ContainsThree.Run();
                        break;

                    case 2:
                        Divisibility.Run();
                        break;

                    case 3:
                        StringReversal.Run();
                        break;

                    case 4:
                        keepRunning = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Enter a number from 1 to 4.");
                        break;
                }
            }
        }

        // Read valid whole number
        internal static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            int number;

            // Retry invalid input
            while (!Int32.TryParse(Console.ReadLine(), out number))
            {
                Console.Write("That is not a whole number. Try again: ");
            }

            return number;
        }
    }
}
