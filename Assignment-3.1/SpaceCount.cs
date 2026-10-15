namespace Caleb_Marks_Assignment_3._1
{
    // Count literal spaces without trimming the input
    internal static class SpaceCount
    {
        // Read the complete string, including leading and trailing spaces
        public static void Run()
        {
            Console.WriteLine("Type some text, then press Enter to count its spaces.");
            Console.Write("Your text (e.g., Boomer Sooner): ");
            string text = Console.ReadLine() ?? "";
            Console.WriteLine("\"" + text + "\" contains " + CountSpaces(text) + " spaces");
        }

        // Count each space character, but not tabs
        public static int CountSpaces(string text)
        {
            int count = 0;
            foreach (char character in text)
            {
                if (character == ' ')
                {
                    count = count + 1;
                }
            }

            return count;
        }
    }
}
