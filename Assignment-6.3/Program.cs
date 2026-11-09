// Caleb Marks

namespace Caleb_Marks_Assignment_6._3
{
    internal class Program
    {
        // Load sample callers, demo queue, then let user manage it
        static void Main(string[] args)
        {
            Console.WriteLine("Greetings!");
            Console.WriteLine("Assignment 6.3 - Customer Call Queue");

            CallerQueue queue = new CallerQueue();
            int nextTicket = 1;

            // Demo: enqueue three callers, iterate, then dequeue one
            Console.WriteLine();
            Console.WriteLine("Demo: enqueueing 3 callers...");
            queue.Enqueue(nextTicket++, "Mike Johnson", "555-4821");
            queue.Enqueue(nextTicket++, "Sarah Davis", "555-7310");
            queue.Enqueue(nextTicket++, "Chris Lee", "555-2954");
            Console.WriteLine("Callers waiting: " + queue.Size);
            queue.Display();
            Console.WriteLine("Dequeue (serve first caller):");
            CallerQueue.PrintCaller(queue.Dequeue());
            Console.WriteLine("Callers still waiting: " + queue.Size);
            queue.Display();

            // Menu loop: run chosen option, repeat
            bool keepRunning = true;
            while (keepRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Call Queue (" + queue.Size + " waiting)");
                Console.WriteLine("1. Add caller (enqueue)");
                Console.WriteLine("2. Serve next caller (dequeue)");
                Console.WriteLine("3. Show next caller (peek)");
                Console.WriteLine("4. Show all waiting callers");
                Console.WriteLine("5. Exit");
                int option = ReadInt("Select an option (1-5): ");

                // Queue methods throw when empty; show message & keep menu running
                try
                {
                    switch (option)
                    {
                        case 1:
                            string name = ReadText("Enter caller name: ");
                            string phone = ReadText("Enter phone number: ");
                            queue.Enqueue(nextTicket, name, phone);
                            Console.WriteLine(name + " added with ticket #" + nextTicket + ".");
                            nextTicket++;
                            break;

                        case 2:
                            // Dequeue first so empty-queue message prints alone
                            CallerNode served = queue.Dequeue();
                            Console.Write("Now serving: ");
                            CallerQueue.PrintCaller(served);
                            break;

                        case 3:
                            CallerNode next = queue.Peek();
                            Console.Write("Next caller: ");
                            CallerQueue.PrintCaller(next);
                            break;

                        case 4:
                            queue.Display();
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
                catch (InvalidOperationException error)
                {
                    Console.WriteLine(error.Message);
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
