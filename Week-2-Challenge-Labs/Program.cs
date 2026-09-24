// Caleb Marks

namespace Caleb_Marks_Week_2_Challenge_Labs
{
    // Show the challenge lab menu
    internal class Program
    {
        // Start the program here
        static void Main(string[] args)
        {
            // Repeat the menu until the user chooses no
            char choice = 'y';
            while (choice == 'y' || choice == 'Y')
            {
                Console.WriteLine("Week 2 Challenge Labs");
                Console.WriteLine("1. Temperature state");
                Console.WriteLine("2. User ID and password (3 attempts)");
                Console.WriteLine("3. Number triangle");
                Console.WriteLine("4. Student marks");
                int option = ReadInt("Select an option (1-4): ");

                // Run the option chosen by the user
                switch (option)
                {
                    case 1:
                        TemperatureState.Run();
                        break;
                    case 2:
                        LoginCheck.Run();
                        break;
                    case 3:
                        NumberTriangle.Run();
                        break;
                    case 4:
                        StudentMarks.Run();
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.Write("\nDo you want to continue? (y/n): ");
                // Keep asking until the user enters Y or N
                while (!Char.TryParse(Console.ReadLine(), out choice) ||
                    (choice != 'y' && choice != 'Y' && choice != 'n' && choice != 'N'))
                {
                    Console.Write("Invalid input. Enter y or n: ");
                }
                Console.WriteLine();
            }
        }

        // Read a valid whole number from the user
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
