namespace Caleb_Marks_Assignment_3._4
{
    // A coffee inherits the beverage properties and adds its own details
    internal class Coffee : Beverage
    {
        public string Roast { get; set; } = string.Empty;
        public bool IsDecaf { get; set; }
    }
}
