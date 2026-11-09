namespace Caleb_Marks_Assignment_4._1
{
    // Keep the phone book while the application is open
    internal class Data
    {
        // Treat uppercase and lowercase versions of a name as the same key
        public static Dictionary<string, Person> People = new Dictionary<string, Person>(StringComparer.OrdinalIgnoreCase);
    }
}
