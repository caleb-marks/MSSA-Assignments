// Caleb Marks

namespace Caleb_Marks_Assignment_12._2
{
    internal class Program
    {
        // Show assignment menu & run selected option
        static void Main(string[] args)
        {
            int choice = 0;
            while (choice != 2)
            {
                Console.WriteLine("\nAssignment 12.2");
                Console.WriteLine("1. Remove linked list elements");
                Console.WriteLine("2. Exit");
                choice = ReadInt("Select an option (1-2): ");
                Console.WriteLine();

                // Run problem or exit
                switch (choice)
                {
                    case 1:
                        RunRemoveElements();
                        break;

                    case 2:
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Enter a number from 1 to 2.");
                        break;
                }
            }
        }

        // Build user's list, remove chosen value & show both lists
        static void RunRemoveElements()
        {
            int nodeCount = ReadInt("Enter number of values: ");

            // Allow empty list but reject negative count
            while (nodeCount < 0)
            {
                nodeCount = ReadInt("Enter zero or more: ");
            }

            // Add each value last
            SinglyLinkedList list = new SinglyLinkedList();
            for (int i = 0; i < nodeCount; i++)
            {
                list.AddLast(ReadInt("Enter value " + (i + 1) + ": "));
            }

            int removeValue = ReadInt("Enter value to remove: ");

            Console.Write("Original list: ");
            list.Display();
            list.RemoveAll(removeValue);
            Console.Write("Output: ");
            list.Display();
        }

        // Read whole numbers & retry invalid input
        static int ReadInt(string prompt)
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
