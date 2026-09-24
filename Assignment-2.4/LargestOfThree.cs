namespace Caleb_Marks_Assignment_2._4
{
    // Compare three numbers
    internal static class LargestOfThree
    {
        // Ask for three numbers and find the largest
        public static void Run()
        {
            int num1 = Program.ReadInt("Input the 1st number :");
            int num2 = Program.ReadInt("Input the 2nd number :");
            int num3 = Program.ReadInt("Input the 3rd number :");

            if (num1 > num2 && num1 > num3)
            {
                Console.WriteLine("The 1st Number is the greatest among three");
            }
            else if (num2 > num1 && num2 > num3)
            {
                Console.WriteLine("The 2nd Number is the greatest among three");
            }
            else if (num3 > num1 && num3 > num2)
            {
                Console.WriteLine("The 3rd Number is the greatest among three");
            }
            // Handle a tie for the largest number
            else
            {
                Console.WriteLine("There is a tie for the greatest number");
            }
        }
    }
}
