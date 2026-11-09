using Caleb_Marks_Assignment_3._3.Models;

namespace Caleb_Marks_Assignment_3._3
{
    // Both forms share this list until the app closes
    internal class Data
    {
        public static List<Student> Students = new List<Student>
        {
            new Student { StudId = 1, FirstName = "Jim",   LastName = "Derry",   Address = "418 Oak St, Tacoma WA",   MonthOfAdmission = MonthOfAdmission.September, Grade = 'A' },
            new Student { StudId = 2, FirstName = "Kelly", LastName = "Simpson", Address = "22 Birch Ave, Olympia WA", MonthOfAdmission = MonthOfAdmission.January,   Grade = 'B' },
            new Student { StudId = 3, FirstName = "Alice", LastName = "Johnson", Address = "907 Pine Rd, Seattle WA",  MonthOfAdmission = MonthOfAdmission.September, Grade = 'C' },
            new Student { StudId = 4, FirstName = "Bob",   LastName = "Brown",   Address = "15 Cedar Ln, Everett WA",  MonthOfAdmission = MonthOfAdmission.June,      Grade = 'B' }
        };
    }
}
