namespace Caleb_Marks_Assignment_3._2
{
    internal class Circle
    {
        public double Radius { get; set; }

        // Calculate the area from the radius
        public double Area
        {
            get
            {
                return Math.PI * Radius * Radius;
            }
        }

        // Add the areas of two circles
        public static double operator +(Circle c1, Circle c2)
        {
            return c1.Area + c2.Area;
        }

        // Subtract the second circle's area from the first
        public static double operator -(Circle c1, Circle c2)
        {
            return c1.Area - c2.Area;
        }
    }
}
