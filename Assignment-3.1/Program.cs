// Caleb Marks

namespace Caleb_Marks_Assignment_3._1
{
    // Show the assignment menu
    internal class Program
    {
        // Start the program here
        static void Main(string[] args)
        {
            // Greet the user
            Console.WriteLine("Greetings!");
            Console.WriteLine();

            // Repeat the menu until the user chooses Exit
            bool keepRunning = true;
            while (keepRunning)
            {
                Console.WriteLine("Assignment 3.1");
                Console.WriteLine("1. Show even numbers from 2 to 98");
                Console.WriteLine("2. Check a leap year");
                Console.WriteLine("3. Count spaces in your text");
                Console.WriteLine("4. Change the first pair of 1s next to each other to 0s");
                Console.WriteLine("5. Exit");
                Console.Write("Enter a menu number (1-5): ");
                int option;

                // Keep asking until the user chooses a menu number
                while (!Int32.TryParse(Console.ReadLine(), out option) || option < 1 || option > 5)
                {
                    Console.Write("Enter a menu number from 1 to 5: ");
                }

                // Run the option chosen by the user
                switch (option)
                {
                    case 1:
                        EvenNumbers.Run();
                        break;

                    case 2:
                        LeapYear.Run();
                        break;

                    case 3:
                        SpaceCount.Run();
                        break;

                    case 4:
                        ReplaceOnes.Run();
                        break;

                    case 5:
                        keepRunning = false;
                        break;

                    default:
                        Console.WriteLine("Enter a number from 1 to 5.");
                        break;
                }

                // Add space before showing the menu again
                if (keepRunning)
                {
                    Console.WriteLine();
                }
            }
        }

        // Read a valid whole number from the user
        internal static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            int number;

            while (!Int32.TryParse(Console.ReadLine(), out number))
            {
                Console.WriteLine("Enter a number without decimals (e.g., 7).");
                Console.Write(prompt);
            }

            return number;
        }
    }
}
