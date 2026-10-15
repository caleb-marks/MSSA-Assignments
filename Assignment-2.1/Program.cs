// caleb marks

namespace Caleb_Marks_Assignment_2._1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // track whether the menu should keep running
            bool keepRunning = true;

            // repeat the menu until the user chooses exit
            while (keepRunning)
            {
                Console.WriteLine("Maths Menu");
                Console.WriteLine("1. Add two integers");
                Console.WriteLine("2. Add three decimals");
                Console.WriteLine("3. Multiply two floats");
                Console.WriteLine("4. Multiply three floats");
                Console.WriteLine("5. Exit");

                int choice = ReadInt("Select an option (1-5): ");

                // run the option chosen by the user
                switch (choice)
                {
                    case 1:
                        int addIntNum1 = ReadInt("Enter the first integer: ");
                        int addIntNum2 = ReadInt("Enter the second integer: ");
                        // call the overload that adds two integers
                        int intSum = Maths.Add(addIntNum1, addIntNum2);
                        Console.WriteLine("Result: " + intSum);
                        break;

                    case 2:
                        decimal addDecimalNum1 = ReadDecimal("Enter the first decimal: ");
                        decimal addDecimalNum2 = ReadDecimal("Enter the second decimal: ");
                        decimal addDecimalNum3 = ReadDecimal("Enter the third decimal: ");
                        // call the overload that adds three decimals
                        decimal decimalSum = Maths.Add(addDecimalNum1, addDecimalNum2, addDecimalNum3);
                        Console.WriteLine("Result: " + decimalSum);
                        break;

                    case 3:
                        float multiplyFloatNum1 = ReadFloat("Enter the first float: ");
                        float multiplyFloatNum2 = ReadFloat("Enter the second float: ");
                        // call the overload that multiplies two floats
                        float twoNumberProduct = Maths.Multiply(multiplyFloatNum1, multiplyFloatNum2);
                        Console.WriteLine("Result: " + twoNumberProduct);
                        break;

                    case 4:
                        float multiplyThreeNum1 = ReadFloat("Enter the first float: ");
                        float multiplyThreeNum2 = ReadFloat("Enter the second float: ");
                        float multiplyThreeNum3 = ReadFloat("Enter the third float: ");
                        // call the overload that multiplies three floats
                        float threeNumberProduct = Maths.Multiply(multiplyThreeNum1, multiplyThreeNum2, multiplyThreeNum3);
                        Console.WriteLine("Result: " + threeNumberProduct);
                        break;

                    case 5:
                        // stop the menu loop
                        keepRunning = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Enter a number from 1 to 5.");
                        break;
                }

                // add space before showing the menu again
                if (keepRunning)
                {
                    Console.WriteLine();
                }
            }
        }

        // read a valid whole number from the user
        static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            int number;

            // keep asking until the user enters a whole number
            while (!Int32.TryParse(Console.ReadLine(), out number))
            {
                Console.Write("Invalid input. Enter a whole number: ");
            }

            return number;
        }

        // read a valid decimal number from the user
        static decimal ReadDecimal(string prompt)
        {
            Console.Write(prompt);
            decimal number;

            // keep asking until the user enters a decimal number
            while (!Decimal.TryParse(Console.ReadLine(), out number))
            {
                Console.Write("Invalid input. Enter a decimal number: ");
            }

            return number;
        }

        // read a valid float number from the user
        static float ReadFloat(string prompt)
        {
            Console.Write(prompt);
            float number;

            // keep asking until the user enters a float number
            while (!Single.TryParse(Console.ReadLine(), out number))
            {
                Console.Write("Invalid input. Enter a number: ");
            }

            return number;
        }
    }
}
