// Caleb Marks

namespace Caleb_Marks_Assignment_2._4
{
    // Show the assignment menu
    internal class Program
    {
        // Start the program here
        static void Main(string[] args)
        {
            // Repeat the menu until the user chooses no
            char choice = 'y';
            while (choice == 'y' || choice == 'Y')
            {
                Console.WriteLine("Assignment 2.4");
                Console.WriteLine("1. Sum of array elements");
                Console.WriteLine("2. Largest of three numbers");
                Console.WriteLine("3. Quadrant of a point");
                int option = ReadInt("Select an option (1-3): ");

                // Run the option chosen by the user
                switch (option)
                {
                    case 1:
                        SumArray.Run();
                        break;
                    case 2:
                        LargestOfThree.Run();
                        break;
                    case 3:
                        Quadrant.Run();
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
