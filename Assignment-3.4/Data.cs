namespace Caleb_Marks_Assignment_3._4
{
    // Keep the coffee records together while the application is open
    internal class Data
    {
        public static List<Coffee> Coffees = new List<Coffee>
        {
            new Coffee { Id = 1, Name = "House Coffee", Price = 2.50m, SizeMl = 350, Roast = "Medium", IsDecaf = false },
            new Coffee { Id = 2, Name = "Decaf Coffee", Price = 2.75m, SizeMl = 350, Roast = "Dark", IsDecaf = true }
        };
    }
}
