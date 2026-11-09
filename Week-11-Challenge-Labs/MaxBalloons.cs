namespace Caleb_Marks_Week_11_Challenge_Labs
{
    internal static class MaxBalloons
    {
        // Read text & display balloon count
        public static void Run()
        {
            Console.Write("Enter text: ");
            string text = Program.ReadInput();
            Console.WriteLine("Maximum instances: " + CountInstances(text));
        }

        // Count balloon letters & find most instances
        public static int CountInstances(string text)
        {
            int bCount = 0;
            int aCount = 0;
            int lCount = 0;
            int oCount = 0;
            int nCount = 0;

            foreach (char letter in text)
            {
                if (letter == 'b')
                {
                    bCount++;
                }
                else if (letter == 'a')
                {
                    aCount++;
                }
                else if (letter == 'l')
                {
                    lCount++;
                }
                else if (letter == 'o')
                {
                    oCount++;
                }
                else if (letter == 'n')
                {
                    nCount++;
                }
            }

            // Each balloon needs two l letters & two o letters
            int[] limits = { bCount, aCount, lCount / 2, oCount / 2, nCount };

            // Smallest limit sets result
            int result = limits[0];
            foreach (int limit in limits)
            {
                if (limit < result)
                {
                    result = limit;
                }
            }

            return result;
        }
    }
}
