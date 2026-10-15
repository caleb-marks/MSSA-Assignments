// Caleb Marks

namespace Caleb_Marks_Week_3_Challenge_Labs
{
    // Show the four challenge problems
    internal class Program
    {
        // Repeat the menu until the user chooses Exit
        static void Main(string[] args)
        {
            Console.WriteLine("Greetings!");
            bool keepRunning = true;
            while (keepRunning)
            {
                Console.WriteLine();
                Console.WriteLine("1. Check a palindrome");
                Console.WriteLine("2. Add the digits in your text");
                Console.WriteLine("3. Find two numbers that add to a target");
                Console.WriteLine("4. Remove AB and CD");
                Console.WriteLine("5. Exit");
                int option = ReadInt("Select an option (1-5): ");

                switch (option)
                {
                    case 1:
                        Palindrome.Run();
                        break;

                    case 2:
                        DigitSum.Run();
                        break;

                    case 3:
                        TwoSum.Run();
                        break;

                    case 4:
                        StringReduction.Run();
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

        // Keep asking until the user enters a whole number
        internal static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            int number;
            while (!Int32.TryParse(Console.ReadLine(), out number))
            {
                Console.Write("That is not a whole number. Try again: ");
            }

            return number;
        }
    }
}
