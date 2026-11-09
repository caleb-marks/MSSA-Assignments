namespace Caleb_Marks_Assignment_7._4
{
    // Parking lot: fixed open spots, each size counted separately
    internal class ParkingSystem
    {
        // Index 0 = big, 1 = medium, 2 = small
        private int[] openSpots = new int[3];

        // Set starting spot counts
        public ParkingSystem(int big, int medium, int small)
        {
            openSpots[0] = big;
            openSpots[1] = medium;
            openSpots[2] = small;
        }

        // Take spot when open; return true or false
        public bool AddCar(int carType)
        {
            if (carType < 1 || carType > 3)
            {
                return false;
            }

            int index = carType - 1;
            if (openSpots[index] > 0)
            {
                openSpots[index]--;
                return true;
            }

            return false;
        }

        // Return open spots, chosen size
        public int GetOpenSpots(int carType)
        {
            return openSpots[carType - 1];
        }
    }
}
