namespace Caleb_Marks_Assignment_3._1
{
    // Build the even-number string
    internal static class EvenNumbers
    {
        // Print the string returned by the method
        public static void Run()
        {
            Console.WriteLine(ReturnEvenNumbers());
        }

        // Return the even numbers strictly between zero and one hundred
        public static string ReturnEvenNumbers()
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            for (int i = 2; i < 100; i = i + 2)
            {
                // Put a space before every number except the first
                if (i > 2)
                {
                    result.Append(" ");
                }
                result.Append(i);
            }

            return result.ToString();
        }
    }
}
