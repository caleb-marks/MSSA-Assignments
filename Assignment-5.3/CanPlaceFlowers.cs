namespace Caleb_Marks_Assignment_5._3
{
    internal static class CanPlaceFlowers
    {
        // Read flowerbed & show planting result
        public static void Run()
        {
            int count = Program.ReadInt("How many plots? ");
            while (count < 0)
            {
                count = Program.ReadInt("Enter zero or more plots: ");
            }

            int[] flowerbed = new int[count];
            for (int i = 0; i < flowerbed.Length; i++)
            {
                int plot = Program.ReadInt("Plot " + (i + 1) + " (0 = empty, 1 = planted): ");
                while (plot != 0 && plot != 1)
                {
                    plot = Program.ReadInt("Enter 0 or 1: ");
                }

                flowerbed[i] = plot;
            }

            int newFlowers = Program.ReadInt("How many new flowers? ");
            while (newFlowers < 0)
            {
                newFlowers = Program.ReadInt("Enter zero or more flowers: ");
            }

            Console.WriteLine("Can place flowers: " + CanPlace(flowerbed, newFlowers));
        }

        // Count safe empty plots; flowerbed stays unchanged
        public static bool CanPlace(int[] flowerbed, int newFlowers)
        {
            if (newFlowers <= 0)
            {
                return true;
            }

            int planted = 0;
            bool previousPlanted = false;
            for (int i = 0; i < flowerbed.Length; i++)
            {
                if (flowerbed[i] == 1)
                {
                    previousPlanted = true;
                }
                else if (!previousPlanted && (i == flowerbed.Length - 1 || flowerbed[i + 1] == 0))
                {
                    planted++;
                    previousPlanted = true;
                    if (planted >= newFlowers)
                    {
                        return true;
                    }
                }
                else
                {
                    previousPlanted = false;
                }
            }

            return false;
        }
    }
}
