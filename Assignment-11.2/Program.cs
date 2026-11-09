// Caleb Marks

namespace Caleb_Marks_Assignment_11._2
{
    internal class Program
    {
        // Show assignment menu & run selected problem
        static void Main(string[] args)
        {
            int choice = 0;
            while (choice != 3)
            {
                Console.WriteLine("\nAssignment 11.2");
                Console.WriteLine("1. Find maximum stock profit");
                Console.WriteLine("2. Reverse linked list");
                Console.WriteLine("3. Exit");
                choice = ReadInt("Select an option (1-3): ");
                Console.WriteLine();

                // Run selected problem or exit
                switch (choice)
                {
                    case 1:
                        RunStockProfit();
                        break;

                    case 2:
                        RunReverseList();
                        break;

                    case 3:
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Enter a number from 1 to 3.");
                        break;
                }
            }
        }

        // Read daily prices & show best profit
        static void RunStockProfit()
        {
            int dayCount = ReadInt("Enter number of price days: ");

            // Require one or more days
            while (dayCount < 1)
            {
                dayCount = ReadInt("Enter a count greater than zero: ");
            }

            // Fill prices array & reject negative prices
            int[] prices = new int[dayCount];
            for (int i = 0; i < prices.Length; i++)
            {
                prices[i] = ReadInt("Enter price for day " + (i + 1) + ": ");
                while (prices[i] < 0)
                {
                    prices[i] = ReadInt("Price cannot be negative. Enter price for day " + (i + 1) + ": ");
                }
            }

            Console.WriteLine("Maximum profit: " + MaxProfit(prices));
        }

        // Return best profit: one buy, later sale
        static int MaxProfit(int[] prices)
        {
            int lowestPrice = prices[0];
            int maxProfit = 0;

            // Track lowest price so far & best sale profit
            for (int i = 1; i < prices.Length; i++)
            {
                if (prices[i] < lowestPrice)
                {
                    lowestPrice = prices[i];
                }
                else if (prices[i] - lowestPrice > maxProfit)
                {
                    maxProfit = prices[i] - lowestPrice;
                }
            }

            return maxProfit;
        }

        // Build user's list, reverse it & show both
        static void RunReverseList()
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

            Console.Write("Original list: ");
            list.Display();
            list.Reverse();
            Console.Write("Reversed list: ");
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
