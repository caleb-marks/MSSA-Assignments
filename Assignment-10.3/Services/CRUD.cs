using Caleb_Marks_Assignment_10._3.Data;
using Caleb_Marks_Assignment_10._3.Models;

namespace Caleb_Marks_Assignment_10._3.Services
{
    // Create, read, update, & delete cars
    public class CRUD
    {
        // Add car & save database
        public void AddCar(Car car)
        {
            Records.Context.Cars.Add(car);
            Records.Context.SaveChanges();
        }

        // Return every car
        public List<Car> GetCars()
        {
            return Records.Context.Cars.ToList();
        }

        // Remove matching car when found
        public void DeleteCar(int id)
        {
            Car? car = Records.Context.Cars.Find(id);
            if (car != null)
            {
                Records.Context.Cars.Remove(car);
                Records.Context.SaveChanges();
            }
        }

        // Copy changed fields; existing car keeps its ID
        public void UpdateCar(int id, Car changedCar)
        {
            Car? existingCar = Records.Context.Cars.Find(id);
            if (existingCar != null)
            {
                existingCar.Vin = changedCar.Vin;
                existingCar.Make = changedCar.Make;
                existingCar.Model = changedCar.Model;
                existingCar.Year = changedCar.Year;
                existingCar.Price = changedCar.Price;
                Records.Context.SaveChanges();
            }
        }

        // Find one car; null when missing
        public Car? GetCar(int id)
        {
            return Records.Context.Cars.Find(id);
        }
    }
}
