namespace Caleb_Marks_Assignment_6._2
{
    internal static class StackProgram
    {
        // Pick stack size, then push, pop, peek, or display
        public static void Run()
        {
            int size = Program.ReadInt("Enter stack capacity: ");
            while (size <= 0)
            {
                size = Program.ReadInt("Capacity must be positive. Try again: ");
            }

            StackArray<int> stack = new StackArray<int>(size);
            bool keepRunning = true;
            while (keepRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Stack (" + stack.Count + " of " + stack.Capacity + " used)");
                Console.WriteLine("1. Push");
                Console.WriteLine("2. Pop");
                Console.WriteLine("3. Peek");
                Console.WriteLine("4. Display");
                Console.WriteLine("5. Back to main menu");
                int option = Program.ReadInt("Select an option (1-5): ");

                // Stack methods throw on full/empty; show message & keep menu running
                try
                {
                    switch (option)
                    {
                        case 1:
                            int value = Program.ReadInt("Enter value to push: ");
                            stack.Push(value);
                            Console.WriteLine("Pushed " + value + ".");
                            break;

                        case 2:
                            Console.WriteLine("Popped " + stack.Pop() + ".");
                            break;

                        case 3:
                            Console.WriteLine("Top value is " + stack.Peek() + ".");
                            break;

                        case 4:
                            stack.Display();
                            break;

                        case 5:
                            keepRunning = false;
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Enter a number from 1 to 5.");
                            break;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
