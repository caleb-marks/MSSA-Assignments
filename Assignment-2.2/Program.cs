// caleb marks 

namespace Caleb_Marks_Assignment_2._2
{
    // main program
    internal class Program
    {
        // Program entry point
        static void Main(string[] args)
        {
            // Display a welcome meessage to user 
            Console.WriteLine("Welcome to the shape area calculator!");

            // Start an infinite loop to allow the user to calculate Circle or Square areas until they choose to exit
            while (true)
            { 
                Console.WriteLine(" 1. Circle \n 2. Square \n 3. Exit");

                string shapeChoice = Console.ReadLine() ?? "";

                switch (shapeChoice)
                {
                    case "1":
                        CalculateCircleArea(); break;

                    case "2":
                        CalculateSquareArea(); break;

                    case "3":

                        Console.WriteLine("See you later!");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                Console.WriteLine( "\nTell me what is next");

                }
                static void CalculateCircleArea()
                {
                    Console.WriteLine("Enter any number for the circle's radius:");

                    // Read the radius input from the user and convert it to a double

                    double radius = Double.Parse(Console.ReadLine());

                    // Create a Circle object and calculate the area
                    Circle circle = new Circle { Radius = radius };

                    // Calculate and display the area of the circle

                    Console.WriteLine($"The area of the circle is: {circle.CalculateArea()}");
                        }

            static void CalculateSquareArea()
                {
                    Console.WriteLine("Enter any number for the square's Side:");

                    Console.WriteLine();

                    double Side = Double.Parse(Console.ReadLine());

                    Square square = new Square { Side = Side };

                    // Create a Square object and calculate the area
                    Console.WriteLine($"The area of the square is: {square.CalculateArea()}");
                }
            }
        }
    }
