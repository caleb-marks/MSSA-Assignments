namespace Caleb_Marks_Week_4_Challenge_Labs
{
    // Reverse character arrays
    internal static class StringReversal
    {
        // Read text, reverse characters, & display result
        public static void Run()
        {
            Console.Write("Enter text to reverse: ");
            char[] s = (Console.ReadLine() ?? "").ToCharArray();
            ReverseString(s);
            Console.Write("Reversed text: ");
            Console.WriteLine(s);
        }

        // Reverse supplied array directly
        public static void ReverseString(char[] s)
        {
            // Swap matching end positions once
            for (int i = 0; i < s.Length / 2; i++)
            {
                int opposite = s.Length - 1 - i;
                char temp = s[i];
                s[i] = s[opposite];
                s[opposite] = temp;
            }
        }
    }
}
