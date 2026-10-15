// Caleb Marks
namespace Assignment_1._2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Call the methods for each part so the computer knows what to execute
            Part1();
            Part2();
            Part3();
        }
        // Method to check if two numbers are equal
        static void Part1()
        {
            // Tells the compuer to display the text 
            Console.Write("Input 1st number: ");
            // Stores the input from the user into a variable called num1,
            // prompts the user then converts the text into a whole nuumber using int.Parse
            int num1 = int.Parse(Console.ReadLine()!);
            // Tells the compuer to display the text
            Console.Write("Input 2nd number: ");
            // Stores the input from the user into a variable called num2,
            // prompts the user then converts the text into a whole nuumber using int.Parse
            int num2 = int.Parse(Console.ReadLine()!);
            // Checks if the two numbers are equal and displays the result
            if (num1 == num2)
            {
                // Displays the result if the two numbers are equal
                Console.WriteLine($"{num1} and {num2} are equal\n");
            }
            else
            {
                // Displays the result if the two numbers are not equal
                Console.WriteLine($"{num1} and {num2} are not equal\n");
            }
        }
        // Method to display the first 10 natural numbers and their sum
        static void Part2()
        {
            // Variable to store the sum of the numbers
            int sum = 0;
            // Displays the first 10 natural numbers
            Console.WriteLine("The first 10 natural number is:");
            // Loop to iterate through the first 10 natural numbers
            for (int i = 1; i <= 10; i++)
            {
                // Displays the current number in the loop
                Console.Write(i + " ");
                // Adds the current number to the sum variable
                sum += i;
            }
            // Displays the sum of the first 10 natural numbers
            Console.WriteLine($"\nThe Sum is: {sum}\n");
        }
        // Method to perform math based on user input
        static void Part3()
        {
            // Variable to store the user's choice to continue or not
            char choice = 'y';
            while (choice == 'y' || choice == 'Y')
            {
                // Displays the menu of operations to the user
                Console.WriteLine("Calculator Menu \n1. Add\n2. Subtract\n3. Multiply\n4. Divide");
                // Prompts the user to enter their choice of operation
                Console.Write("Select an option (1-4): ");
                int operation = int.Parse(Console.ReadLine()!);
                // Checks if the user's choice is valid (between 1 and 4)
                if (operation >= 1 && operation <= 4)
                {
                    // Prompts the user to enter the first number
                    Console.Write("Enter the first number: ");
                    int num1 = int.Parse(Console.ReadLine()!);
                    // Prompts the user to enter the second number
                    Console.Write("Enter the second number: ");
                    // Stores the input from the user into a variable called num2,
                    int num2 = int.Parse(Console.ReadLine()!);
                    // Performs the operation based on the user's choice and displays the result
                    switch (operation)
                    {
                        // Calls the Addition method if the user chose option 1
                        case 1:
                            Console.WriteLine($"Result: {Addition(num1, num2)}");
                            break;
                        // Calls the Subtraction method if the user chose option 2
                        case 2:
                            Console.WriteLine($"Result: {Subtraction(num1, num2)}");
                            break;
                        // Calls the Multiplication method if the user chose option 3
                        case 3:
                            Console.WriteLine($"Result: {Multiplication(num1, num2)}");
                            break;
                        // Calls the Division method if the user chose option 4
                        case 4:
                            // Checks if the second number is zero to avoid division by zero error
                            if (num2 == 0)
                            {
                                // Displays an error message if the user tries to divide by zero
                                Console.WriteLine("Cannot divide by zero.");
                            }
                            // Calls the Division method if the second number is not zero
                            else
                            {
                                // Displays the result of the division operation
                                Console.WriteLine($"Result: {Division(num1, num2)}");
                            }
                            break;
                        // Displays an error message if no case matches
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                // Displays an error message if the user's choice is invalid
                else
                {
                    // Displays an error message if the user's choice is invalid
                    Console.WriteLine("Invalid choice.");
                }
                // Prompts the user to continue or exit the program
                Console.Write("\nDo you want to continue? (y/n): ");
                // Stores the user's choice to continue or exit the program
                choice = char.Parse(Console.ReadLine()!);
            }
        }
        // Method to perform addition of two numbers
        static int Addition(int num1, int num2)
        {
            // Returns the sum of the two numbers
            return num1 + num2;
        }
        // Method to perform subtraction of two numbers
        static int Subtraction(int num1, int num2)
        {
            // Returns the difference of the two numbers
            return num1 - num2;
        }
        // Method to perform multiplication of two numbers
        static int Multiplication(int num1, int num2)
        {
            // Returns the product of the two numbers
            return num1 * num2;
        }
        // Method to perform division of two numbers
        static double Division(int num1, int num2)
        {
            // Returns the quotient of the two numbers as a double to handle decimal results
            return (double)num1 / num2;
        }
    }
}
