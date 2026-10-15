// Caleb Marks

namespace Caleb_Marks_Assignment_7._3
{
    internal class Program
    {
        // Build sample BST, then let user add or search values
        static void Main(string[] args)
        {
            Console.WriteLine("Greetings!");

            BinarySearchTree tree = new BinarySearchTree();
            tree.Add(5);
            tree.Add(2);
            tree.Add(8);
            tree.Add(1);
            tree.Add(3);

            bool keepRunning = true;
            while (keepRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Assignment 7.3");
                Console.WriteLine("1. Add value");
                Console.WriteLine("2. Search value");
                Console.WriteLine("3. Display tree");
                Console.WriteLine("4. Exit");
                int option = ReadInt("Select an option (1-4): ");
                Console.WriteLine();

                // Run selected tree action or exit
                switch (option)
                {
                    case 1:
                        int newValue = ReadInt("Enter value to add: ");
                        if (tree.Add(newValue))
                        {
                            Console.WriteLine("Added " + newValue + ".");
                        }
                        else
                        {
                            Console.WriteLine(newValue + " already exists. No duplicates allowed.");
                        }
                        break;

                    case 2:
                        SearchValue(tree);
                        break;

                    case 3:
                        tree.Display(tree.Root);
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

        // Find value & show subtree rooted there, or report missing
        private static void SearchValue(BinarySearchTree tree)
        {
            int value = ReadInt("Enter value to search: ");
            TreeNode? subtree = tree.Search(value);
            if (subtree == null)
            {
                Console.WriteLine("Value " + value + " not found.");
                return;
            }

            Console.WriteLine("Found " + subtree.Value + ". Its subtree:");
            tree.Display(subtree);
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
