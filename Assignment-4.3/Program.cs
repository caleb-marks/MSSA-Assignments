// Caleb Marks

namespace Caleb_Marks_Assignment_4._3
{
    internal class Program
    {
        // Show assignment choices & repeat menu
        static void Main(string[] args)
        {
            Console.WriteLine("Greetings!");
            int option;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Assignment 4.3");
                Console.WriteLine("1. Calculate an electricity bill");
                Console.WriteLine("2. Count how often each number appears");
                Console.WriteLine("3. Show numbers that appear only once");
                Console.WriteLine("4. Exit");
                option = ReadInt("Select an option (1-4): ");
                Console.WriteLine();

                // Run selected problem or exit
                switch (option)
                {
                    case 1:
                        ElectricityBill.Run();
                        break;

                    case 2:
                        ArrayCounts.Run(false);
                        break;

                    case 3:
                        ArrayCounts.Run(true);
                        break;

                    case 4:
                        Console.WriteLine("See ya later!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Select a number from 1 to 4.");
                        break;
                }
            }
            while (option != 4);
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

        // Reject blank text & trim extra spaces
        internal static string ReadText(string prompt)
        {
            Console.Write(prompt);
            string text = Console.ReadLine() ?? "";
            while (String.IsNullOrWhiteSpace(text))
            {
                Console.Write("Enter a value that is not blank: ");
                text = Console.ReadLine() ?? "";
            }

            return text.Trim();
        }
    }
}
