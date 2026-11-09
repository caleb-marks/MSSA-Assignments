// Caleb Marks

namespace Caleb_Marks_Assignment_7._2
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
                Console.WriteLine("Assignment 7.2");
                Console.WriteLine("1. Merge sort");
                Console.WriteLine("2. Reverse vowels");
                Console.WriteLine("3. Valid anagram");
                Console.WriteLine("4. Exit");
                int option = ReadInt("Select an option (1-4): ");
                Console.WriteLine();

                // Run selected problem or exit
                switch (option)
                {
                    case 1:
                        MergeSort.Run();
                        break;

                    case 2:
                        ReverseVowels.Run();
                        break;

                    case 3:
                        ValidAnagram.Run();
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
            string text = Console.ReadLine() ?? "";
            while (text.Trim() == "")
            {
                Console.Write("Invalid input. Enter some text: ");
                text = Console.ReadLine() ?? "";
            }

            return text;
        }
    }
}
