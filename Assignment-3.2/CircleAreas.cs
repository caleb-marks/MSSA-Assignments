namespace Caleb_Marks_Assignment_3._2
{
    // Add and subtract the areas of two circles
    internal static class CircleAreas
    {
        // Ask for both circle sizes and show the area sum and difference
        public static void Run()
        {
            Circle c1;
            Circle c2;
            double totalArea;
            do
            {
                c1 = ReadCircle("First circle radius (0 or more): ");
                c2 = ReadCircle("Second circle radius (0 or more): ");
                totalArea = c1 + c2;
                if (!Program.IsFinite(totalArea))
                {
                    Console.WriteLine("The combined area is too large. Enter smaller radii.");
                }
            }
            while (!Program.IsFinite(totalArea));

            double areaDifference = c1 - c2;
            Console.WriteLine();
            Console.WriteLine("First circle area: " + c1.Area);
            Console.WriteLine("Second circle area: " + c2.Area);
            Console.WriteLine("Total area: " + totalArea);
            Console.WriteLine("Area difference (first - second): " + areaDifference);
        }

        // Keep asking if the radius is negative or the area is too big
        private static Circle ReadCircle(string prompt)
        {
            Circle circle = new Circle();
            circle.Radius = Program.ReadDouble(prompt);
            while (circle.Radius < 0 || !Program.IsFinite(circle.Area))
            {
                circle.Radius = Program.ReadDouble("Radius must be 0 or more and small enough to calculate the area. Try again: ");
            }

            return circle;
        }
    }
}
