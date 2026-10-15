namespace Caleb_Marks_Assignment_2._4
{
    // Find the location of a point
    internal static class Quadrant
    {
        // Ask for X and Y and display the quadrant
        public static void Run()
        {
            int x = Program.ReadInt("Input the value for X coordinate :");
            int y = Program.ReadInt("Input the value for Y coordinate :");

            // Check the origin and axes before the quadrants
            string place;
            if (x == 0 && y == 0)
            {
                place = "is the origin.";
            }
            else if (x == 0)
            {
                place = "lies on the Y axis.";
            }
            else if (y == 0)
            {
                place = "lies on the X axis.";
            }
            else if (x > 0 && y > 0)
            {
                place = "lies in the First quadrant.";
            }
            else if (x < 0 && y > 0)
            {
                place = "lies in the Second quadrant.";
            }
            else if (x < 0 && y < 0)
            {
                place = "lies in the Third quadrant.";
            }
            else
            {
                place = "lies in the Fourth quadrant.";
            }

            Console.WriteLine("The coordinate point (" + x + "," + y + ") " + place);
        }
    }
}
