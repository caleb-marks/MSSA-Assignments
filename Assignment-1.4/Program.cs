// Caleb Marks

namespace Caleb_Marks_Assignment_1._4
{
    // Define the main program class
    internal class Program
    {
        // Start the program here
        static void Main(string[] args)
        {
            // Create the first point
            Point p1 = new Point();
            // Create the second point
            Point p2 = new Point();
            // Store each coordinate after it is validated
            int coordinate;

            // Ask for the first point's horizontal position
            Console.Write("Enter P1 X coordinate: ");
            // Keep asking until the user enters a whole number
            while (!Int32.TryParse(Console.ReadLine(), out coordinate))
            {
                // Explain what type of input is required
                Console.Write("Invalid input. Enter a whole number: ");
            }
            // Set the first point's horizontal position
            p1.X = coordinate;

            // Ask for the first point's vertical position
            Console.Write("Enter P1 Y coordinate: ");
            // Keep asking until the user enters a whole number
            while (!Int32.TryParse(Console.ReadLine(), out coordinate))
            {
                // Explain what type of input is required
                Console.Write("Invalid input. Enter a whole number: ");
            }
            // Set the first point's vertical position
            p1.Y = coordinate;

            // Ask for the second point's horizontal position
            Console.Write("Enter P2 X coordinate: ");
            // Keep asking until the user enters a whole number
            while (!Int32.TryParse(Console.ReadLine(), out coordinate))
            {
                // Explain what type of input is required
                Console.Write("Invalid input. Enter a whole number: ");
            }
            // Set the second point's horizontal position
            p2.X = coordinate;

            // Ask for the second point's vertical position
            Console.Write("Enter P2 Y coordinate: ");
            // Keep asking until the user enters a whole number
            while (!Int32.TryParse(Console.ReadLine(), out coordinate))
            {
                // Explain what type of input is required
                Console.Write("Invalid input. Enter a whole number: ");
            }
            // Set the second point's vertical position
            p2.Y = coordinate;

            // Check whether the second point is farther right
            if (p2.X > p1.X)
            {
                // Tell the user that the second point is right of the first
                Console.WriteLine("P2 is to the right of P1.");
            }
            // Check whether the second point is farther left
            else if (p2.X < p1.X)
            {
                // Tell the user that the second point is left of the first
                Console.WriteLine("P2 is to the left of P1.");
            }
            // Handle points with matching horizontal positions
            else
            {
                // Tell the user that both points have the same horizontal position
                Console.WriteLine("P2 is on the same axis as P1.");
            }

            // Create a student object
            Student student = new Student();

            // Ask for the student's identification number
            Console.Write("Enter student ID: ");
            // Store the identification number after it is validated
            int studentId;
            // Keep asking until the user enters a whole number
            while (!Int32.TryParse(Console.ReadLine(), out studentId))
            {
                // Explain what type of input is required
                Console.Write("Invalid input. Enter a whole number: ");
            }
            // Set the student's identification number
            student.StudentId = studentId;

            // Ask for the student's first name
            Console.Write("Enter student first name: ");
            // Set the student's first name
            student.StudentFname = Console.ReadLine() ?? "";

            // Ask for the student's last name
            Console.Write("Enter student last name: ");
            // Set the student's last name
            student.StudentLname = Console.ReadLine() ?? "";

            // Ask for the student's grade
            Console.Write("Enter student grade: ");
            // Store the grade after it is validated
            char studentGrade;
            // Keep asking until the user enters one character
            while (!Char.TryParse(Console.ReadLine(), out studentGrade))
            {
                // Explain what type of input is required
                Console.Write("Invalid input. Enter one character: ");
            }
            // Set the student's grade
            student.StudentGrade = studentGrade;

            // Separate the input from the displayed student data
            Console.WriteLine();
            // Display the student's identification number
            Console.WriteLine("Student ID: " + student.StudentId);
            // Display the student's first name
            Console.WriteLine("Student First Name: " + student.StudentFname);
            // Display the student's last name
            Console.WriteLine("Student Last Name: " + student.StudentLname);
            // Display the student's grade
            Console.WriteLine("Student Grade: " + student.StudentGrade);
        }
    }
}
