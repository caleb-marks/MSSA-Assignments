using Caleb_Marks_Assignment_10._3.Models;
using Microsoft.EntityFrameworkCore;

namespace Caleb_Marks_Assignment_10._3.Data
{
    // Database session holding Cars table
    public class CarContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }

        // Connect local SQL Server; Initial Catalog names database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=CCAD23CarInventory;Integrated Security=True;Encrypt=False;Trust Server Certificate=True;MultipleActiveResultSets=True");
        }

        // Seed starting cars; sample VINs & prices
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().HasData(
                new Car { CarId = 1, Vin = "1FTFW1E50MFA12345", Make = "Ford", Model = "F-150", Year = 2021, Price = 38500 },
                new Car { CarId = 2, Vin = "DEMO2017RAV4SE001", Make = "Toyota", Model = "RAV4 SE Hybrid", Year = 2017, Price = 18500 },
                new Car { CarId = 3, Vin = "1G1FB1RX3N0109532", Make = "Chevrolet", Model = "Camaro", Year = 2022, Price = 31200 });
        }
    }
}
