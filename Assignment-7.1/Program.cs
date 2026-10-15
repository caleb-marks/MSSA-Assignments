// Caleb Marks

namespace Caleb_Marks_Assignment_7._1
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
                Console.WriteLine("Assignment 7.1");
                Console.WriteLine("1. Sort exam scores (selection sort)");
                Console.WriteLine("2. Sort exam scores (insertion sort, practice)");
                Console.WriteLine("3. Merge strings alternately");
                Console.WriteLine("4. Exit");
                int option = ReadInt("Select an option (1-4): ");
                Console.WriteLine();

                // Run selected problem or exit
                switch (option)
                {
                    case 1:
                        SelectionSort.Run();
                        break;

                    case 2:
                        InsertionSort.Run();
                        break;

                    case 3:
                        MergeStrings.Run();
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

        // Read non-blank text & retry empty input
        internal static string ReadText(string prompt)
        {
            Console.Write(prompt);
            string text = (Console.ReadLine() ?? "").Trim();
            while (text == "")
            {
                Console.Write("Invalid input. Enter some text: ");
                text = (Console.ReadLine() ?? "").Trim();
            }

            return text;
        }
    }
}
