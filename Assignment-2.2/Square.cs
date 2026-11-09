namespace Caleb_Marks_Assignment_2._2
{
    internal class Square : Shape
    {
        // Property for the length of the square's side
        public double Side { get; set; }
        // Override the CalculateArea method to compute the area of the square
        public override double CalculateArea()
        {
            // Area of a square is calculated using the formula: side * side
            return Side * Side;
        }
    }
}
