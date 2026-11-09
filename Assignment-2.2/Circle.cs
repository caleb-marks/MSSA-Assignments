namespace Caleb_Marks_Assignment_2._2
{
    // This class represents a circle shape and inherits from the Shape class
    internal class Circle : Shape
    {
        public double Radius { get; set; }
        // Override the CalculateArea method to compute the area of the circle
        public override double CalculateArea()
        {
            // Area of a circle is calculated using the formula: π * radius^2
            return Math.PI * Radius * Radius;
        }   
    }
}
