namespace Caleb_Marks_Week_2_Challenge_Labs
{
    // Create a student report card
    internal static class StudentMarks
    {
        // Ask for the student's details and display the results
        public static void Run()
        {
            Student student = new Student();
            student.RollNumber = Program.ReadInt("Input the Roll Number of the student : ");
            Console.Write("Input the Name of the Student : ");
            student.Name = Console.ReadLine() ?? "";
            Console.WriteLine("Enter one subject mark at a time, then press Enter.");
            student.Physics = ReadMark("Physics");
            student.Chemistry = ReadMark("Chemistry");
            student.ComputerApplication = ReadMark("Computer Application");

            // Calculate the total and percentage
            int total = student.Physics + student.Chemistry + student.ComputerApplication;
            double percentage = total / 3.0;

            // Choose the division from the percentage
            string division;
            if (percentage >= 60)
            {
                division = "First";
            }
            else if (percentage >= 50)
            {
                division = "Second";
            }
            else if (percentage >= 40)
            {
                division = "Third";
            }
            else
            {
                division = "Fail";
            }

            Console.WriteLine();
            Console.WriteLine("Roll No : " + student.RollNumber);
            Console.WriteLine("Name of Student : " + student.Name);
            Console.WriteLine("Marks in Physics : " + student.Physics);
            Console.WriteLine("Marks in Chemistry : " + student.Chemistry);
            Console.WriteLine("Marks in Computer Application : " + student.ComputerApplication);
            Console.WriteLine("Total Marks = " + total);
            Console.WriteLine("Percentage = " + percentage.ToString("F2"));
            Console.WriteLine("Division = " + division);
        }

        // Read a mark from 0 to 100
        static int ReadMark(string subject)
        {
            Console.Write(subject + " mark (0-100): ");
            int number;

            while (!Int32.TryParse(Console.ReadLine(), out number) || number < 0 || number > 100)
            {
                Console.Write("Enter one whole number from 0 to 100 for " + subject + ": ");
            }

            return number;
        }
    }
}
