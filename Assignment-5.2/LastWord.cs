namespace Caleb_Marks_Assignment_5._2
{
    internal static class LastWord
    {
        // Read sentence & display last word length
        public static void Run()
        {
            Console.Write("Enter sentence: ");
            string sentence = Console.ReadLine() ?? "";
            int length = LengthOfLastWord(sentence);
            Console.WriteLine("Length last word: " + length);
        }

        // Measure trailing word ignoring spaces
        public static int LengthOfLastWord(string sentence)
        {
            int index = sentence.Length - 1;

            // Skip trailing blanks then count letters
            while (index >= 0 && sentence[index] == ' ')
            {
                index--;
            }

            int length = 0;
            while (index >= 0 && sentence[index] != ' ')
            {
                length++;
                index--;
            }

            return length;
        }
    }
}
