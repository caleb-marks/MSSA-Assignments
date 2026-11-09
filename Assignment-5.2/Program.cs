// Caleb Marks

namespace Caleb_Marks_Assignment_5._2
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
                Console.WriteLine("Assignment 5.2");
                Console.WriteLine("1. Length last word");
                Console.WriteLine("2. Print 1..n recursively");
                Console.WriteLine("3. Print n..1 recursively");
                Console.WriteLine("4. Check string palindrome recursively");
                Console.WriteLine("5. Exit");
                int option = ReadInt("Select an option (1-5): ");
                Console.WriteLine();

                // Run selected problem or exit
                switch (option)
                {
                    case 1:
                        LastWord.Run();
                        break;

                    case 2:
                        AscendingNumbers.Run();
                        break;

                    case 3:
                        DescendingNumbers.Run();
                        break;

                    case 4:
                        StringPalindrome.Run();
                        break;

                    case 5:
                        keepRunning = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Enter a number from 1 to 5.");
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
