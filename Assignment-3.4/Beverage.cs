namespace Caleb_Marks_Assignment_3._4
{
    // Hold properties that different beverages share
    internal abstract class Beverage
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int SizeMl { get; set; }
    }
}
