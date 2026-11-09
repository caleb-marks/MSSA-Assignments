// Caleb Marks

namespace Caleb_Marks_Assignment_5._1
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
                Console.WriteLine("Assignment 5.1");
                Console.WriteLine("1. Check palindrome number");
                Console.WriteLine("2. Sum digits");
                Console.WriteLine("3. Check for duplicates");
                Console.WriteLine("4. Exit");
                int option = ReadInt("Select an option (1-4): ");
                Console.WriteLine();

                // Run selected problem or exit
                switch (option)
                {
                    case 1:
                        PalindromeNumber.Run();
                        break;

                    case 2:
                        DigitSum.Run();
                        break;

                    case 3:
                        ContainsDuplicate.Run();
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
