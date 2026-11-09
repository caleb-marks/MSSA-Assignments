namespace Caleb_Marks_Assignment_3._1
{
    // Check the leap-year rules
    internal static class LeapYear
    {
        // Ask for a year and show the Boolean result
        public static void Run()
        {
            int year = Program.ReadInt("Enter a year (e.g., 2017): ");
            Console.WriteLine("Leap year: " + IfYearIsLeap(year));
        }

        // Century years must divide evenly by 400; other years by 4
        public static bool IfYearIsLeap(int year)
        {
            return year % 400 == 0 || (year % 4 == 0 && year % 100 != 0);
        }
    }
}
