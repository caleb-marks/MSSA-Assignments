namespace Caleb_Marks_Assignment_3._3.Models
{
    // Months available for student admission
    internal enum MonthOfAdmission
    {
        // Default value when no month is set
        None = 0,
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    internal class Student
    {
        public int StudId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public MonthOfAdmission MonthOfAdmission { get; set; }

        // Store the grade as one letter
        public char Grade { get; set; }
    }
}
