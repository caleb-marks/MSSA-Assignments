namespace Caleb_Marks_Assignment_2._2
{
    // This is an abstract class that represents a shape. It serves as a base class for specific shape types 
    internal abstract class Shape
    {
        // Properties for the shape's ID, name, and color
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Color { get; set; }

        // Abstract method to calculate the area of the shape. Each derived class must implement this method.
        public abstract double CalculateArea();
    }
}
