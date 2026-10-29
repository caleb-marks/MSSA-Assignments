// Caleb Marks

using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace Caleb_Marks_Assignment_10._1
{
    internal class Program
    {
        // Show assignment menu & handle choices
        static void Main(string[] args)
        {
            int choice = 0;
            while (choice != 3)
            {
                Console.WriteLine("\nAssignment 10.1");
                Console.WriteLine("1. Save and restore students");
                Console.WriteLine("2. Generate parentheses");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option (1-3): ");
                if (!Int32.TryParse(Console.ReadLine(), out choice))
                {
                    choice = 0;
                }
                switch (choice)
                {
                    case 1:
                        SaveAndRestore();
                        break;
                    case 2:
                        RunParentheses();
                        break;
                    case 3:
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Enter an option from 1 to 3.");
                        break;
                }
            }
        }

        // Save student records & display restored records
        static void SaveAndRestore()
        {
            List<Student> students = new List<Student>();
            students.Add(new Student { Id = 1, Name = "Baker Mayfield", Grade = "A" });
            students.Add(new Student { Id = 2, Name = "CeeDee Lamb", Grade = "B" });
            students.Add(new Student { Id = 3, Name = "Jalen Hurts", Grade = "A" });
            string jsonPath = "students.json";
            string xmlPath = "students.xml";

            try
            {
                // Save & restore JSON records
                using (FileStream stream = new FileStream(jsonPath, FileMode.Create, FileAccess.Write))
                {
                    JsonSerializer.Serialize(stream, students);
                }
                List<Student>? jsonStudents;
                using (FileStream stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
                {
                    jsonStudents = JsonSerializer.Deserialize<List<Student>>(stream);
                }

                // Save & restore XML records
                XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
                using (FileStream stream = new FileStream(xmlPath, FileMode.Create, FileAccess.Write))
                {
                    serializer.Serialize(stream, students);
                }
                List<Student>? xmlStudents;
                using (FileStream stream = new FileStream(xmlPath, FileMode.Open, FileAccess.Read))
                {
                    xmlStudents = serializer.Deserialize(stream) as List<Student>;
                }

                Console.WriteLine("\nRestored from JSON:");
                ShowStudents(jsonStudents);
                Console.WriteLine("\nRestored from XML:");
                ShowStudents(xmlStudents);
                Console.WriteLine("\nSaved students.json and students.xml in the current folder.");
            }
            catch (Exception error)
            {
                Console.WriteLine("Could not save or restore the students.");
                Console.WriteLine(error.Message);
            }
        }

        // Display restored student records
        static void ShowStudents(List<Student>? students)
        {
            if (students == null)
            {
                Console.WriteLine("No student records were restored.");
                return;
            }
            foreach (Student student in students)
            {
                Console.WriteLine("ID: " + student.Id + " | Name: " + student.Name + " | Grade: " + student.Grade);
            }
        }

        // Request pairs & show valid combinations
        static void RunParentheses()
        {
            int pairs;
            Console.Write("Enter the number of pairs (0 or greater): ");
            while (!Int32.TryParse(Console.ReadLine(), out pairs) || pairs < 0)
            {
                Console.Write("Enter a whole number of 0 or greater: ");
            }
            List<string> combinations = new List<string>();
            GenerateParentheses(pairs, 0, 0, "", combinations);
            Console.WriteLine("Valid combinations:");
            foreach (string combination in combinations)
            {
                Console.WriteLine(combination);
            }
        }

        // Build valid combinations recursively
        static void GenerateParentheses(int pairs, int opened, int closed, string current, List<string> combinations)
        {
            if (closed == pairs)
            {
                combinations.Add(current);
                return;
            }
            // Add opening parentheses while pairs remain
            if (opened < pairs)
            {
                GenerateParentheses(pairs, opened + 1, closed, current + "(", combinations);
            }
            // Close unmatched opening parentheses
            if (closed < opened)
            {
                GenerateParentheses(pairs, opened, closed + 1, current + ")", combinations);
            }
        }
    }
}
