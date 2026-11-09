// Caleb Marks

namespace Caleb_Marks_Assignment_7._4
{
    internal class Program
    {
        // Set spot counts, then let user park cars & exit
        static void Main(string[] args)
        {
            Console.WriteLine("Greetings!");
            Console.WriteLine("Assignment 7.4 - Design Parking System");
            Console.WriteLine();

            int big = ReadSpotCount("Enter big spots: ");
            int medium = ReadSpotCount("Enter medium spots: ");
            int small = ReadSpotCount("Enter small spots: ");
            ParkingSystem parkingSystem = new ParkingSystem(big, medium, small);

            bool keepRunning = true;
            while (keepRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Assignment 7.4");
                Console.WriteLine("1. Park car");
                Console.WriteLine("2. Show open spots");
                Console.WriteLine("3. Exit");
                int option = ReadInt("Select an option (1-3): ");
                Console.WriteLine();

                // Run selected parking action or exit
                switch (option)
                {
                    case 1:
                        ParkCar(parkingSystem);
                        break;

                    case 2:
                        ShowOpenSpots(parkingSystem);
                        break;

                    case 3:
                        keepRunning = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Enter a number from 1 to 3.");
                        break;
                }
            }
        }

        // Ask car type, try parking & show result
        private static void ParkCar(ParkingSystem parkingSystem)
        {
            Console.WriteLine("Car types: 1 = big, 2 = medium, 3 = small");
            int carType = ReadInt("Enter car type (1-3): ");

            // Keep asking while type not 1, 2 or 3
            while (carType < 1 || carType > 3)
            {
                carType = ReadInt("Invalid car type. Enter 1, 2 or 3: ");
            }

            bool parked = parkingSystem.AddCar(carType);
            if (parked)
            {
                Console.WriteLine(GetSizeName(carType) + " car parked.");
            }
            else
            {
                Console.WriteLine(GetSizeName(carType) + " spots full. Car not parked.");
            }
        }

        // Print open spot counts, each size
        private static void ShowOpenSpots(ParkingSystem parkingSystem)
        {
            for (int carType = 1; carType <= 3; carType++)
            {
                Console.WriteLine(GetSizeName(carType) + " spots open: " + parkingSystem.GetOpenSpots(carType));
            }
        }

        // Return size name matching car type number
        private static string GetSizeName(int carType)
        {
            if (carType == 1)
            {
                return "Big";
            }
            else if (carType == 2)
            {
                return "Medium";
            }
            else
            {
                return "Small";
            }
        }

        // Read spot count & retry negative numbers
        private static int ReadSpotCount(string prompt)
        {
            int number = ReadInt(prompt);
            while (number < 0)
            {
                number = ReadInt("Spot count cannot be negative. Enter 0 or more: ");
            }

            return number;
        }

        // Read whole numbers & retry invalid input
        internal static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            int number;
            while (!Int32.TryParse(Console.ReadLine(), out number))
            {
                Console.Write("Invalid input. Enter a whole number: ");
            }

            return number;
        }
    }
}
