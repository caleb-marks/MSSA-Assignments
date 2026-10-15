// Caleb Marks
namespace Caleb_Marks_Assignment_1._3
{
    // This program calculates the area of a triangle, square, or rectangle based on user input 
    internal class Program
    {
        // The Main method is the entry point like the front door of the house
        static void Main()
        {
            // Call the required assignment parts
            Part1();
            Part3();
        }
        // Part 1: like a room in the house 
        static void Part1()
        {
            // Display the area calculator menu and prompt the user for a choice
            Console.Write("Area Calculator\n1. Triangle\n2. Square\n3. Rectangle\nChoose: ");

            // Use a switch statement to handle the user's choice and call the appropriate method
            switch (Console.ReadLine())
            {
                case "1":
                    Triangle(Get("Base: "), Get("Height: "));
                    break;
                case "2":
                    Square(Get("Side: "));
                    break;
                case "3":
                    Rectangle(Get("Length: "), Get("Width: "));
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
        // Get method to the get the correct input from the user and return it as a double
        static double Get(string prompt)
        {
            // Prompt the user for input and read the value from the console
            Console.Write(prompt);
            return double.Parse(Console.ReadLine()!);
        }
        // Triangle method to calculate the area of a triangle
        static void Triangle(double b, double h)
        {
            Console.WriteLine($"Triangle area: {0.5 * b * h}");
        }
        // Square method to calculate the area of a square
        static void Square(double s)
        {
            // Calculate the area of a square and display the result
            Console.WriteLine($"Square area: {s * s}");
        }
        // Rectangle method to calculate the area of a rectangle
        static void Rectangle(double l, double w)
        {
            // Calculate the area of a rectangle and display the result
            Console.WriteLine($"Rectangle area: {l * w}");
        }

        // Part 3: read any number of values and display them in reverse
        static void Part3()
        {
            // Prompt the user for the number of elements to store in the array
            Console.Write("\nInput the number of elements to store in the array: ");
            // Read the number of elements from the console and parse it as an integer
            int n = int.Parse(Console.ReadLine()!);
            // Create an array to store the values
            int[] values = new int[n];
            // Prompt the user to input the values for the array
            Console.WriteLine($"Input {n} number of elements in the array:");
            // Loop through the array and read each value from the console
            for (int i = 0; i < values.Length; i++)
            {
                // Prompt the user for each element and read the value from the console
                Console.Write($"element - {i}: ");
                // Parse the input as an integer and store it in the array
                values[i] = int.Parse(Console.ReadLine()!);
            }
            // Display the values stored in the array
            Console.WriteLine("The values store into the array are:");
            // Loop through the array and print each value to the console
            for (int i = 0; i < values.Length; i++)
            {
                // Print each value followed by a space
                Console.Write(values[i] + " ");
            }
            // Print a new line for better formatting
            Console.WriteLine("\nThe values store into the array in reverse are:");
            // Loop through the array in reverse order and print each value to the console
            for (int i = values.Length - 1; i >= 0; i--)
            {
                // Print each value followed by a space
                Console.Write(values[i] + " ");
            }
            // Print a new line for formatting
            Console.WriteLine();
        }
    }
}
