namespace Caleb_Marks_Assignment_6._1
{
    internal static class HouseProgram
    {
        // Build sample list, then add, display, or search houses
        public static void Run()
        {
            HouseLinkedList houses = new HouseLinkedList();
            houses.AddLast(101, "12 Oak St, Tacoma", "Ranch");
            houses.AddLast(205, "48 Pine Ave, Seattle", "Colonial");
            houses.AddLast(330, "7 Lake Rd, Olympia", "Victorian");

            bool keepRunning = true;
            while (keepRunning)
            {
                Console.WriteLine();
                Console.WriteLine("House Linked List (" + houses.Size + " houses)");
                Console.WriteLine("1. Add house");
                Console.WriteLine("2. Display all houses");
                Console.WriteLine("3. Search house by number");
                Console.WriteLine("4. Back to main menu");
                int option = Program.ReadInt("Select an option (1-4): ");

                // Run selected house action
                switch (option)
                {
                    case 1:
                        AddHouse(houses);
                        break;

                    case 2:
                        houses.Display();
                        break;

                    case 3:
                        SearchHouse(houses);
                        break;

                    case 4:
                        keepRunning = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Enter a number from 1 to 4.");
                        break;
                }
            }
        }

        // Read new house details & reject duplicate numbers
        private static void AddHouse(HouseLinkedList houses)
        {
            int houseNumber = ReadHouseNumber();
            if (houses.Search(houseNumber) != null)
            {
                Console.WriteLine("House #" + houseNumber + " already exists.");
                return;
            }

            string houseAddress = Program.ReadText("Enter brief address: ");
            string houseType = Program.ReadText("Enter house type (Ranch, Colonial, etc.): ");
            houses.AddLast(houseNumber, houseAddress, houseType);
            Console.WriteLine("House #" + houseNumber + " added.");
        }

        // Find matching house number & show details
        private static void SearchHouse(HouseLinkedList houses)
        {
            int houseNumber = ReadHouseNumber();
            HouseNode? house = houses.Search(houseNumber);
            if (house == null)
            {
                Console.WriteLine("House #" + houseNumber + " not found.");
            }
            else
            {
                Console.WriteLine("House found:");
                HouseLinkedList.PrintHouse(house);
            }
        }

        // Read positive house number
        private static int ReadHouseNumber()
        {
            int houseNumber = Program.ReadInt("Enter house number: ");
            while (houseNumber <= 0)
            {
                houseNumber = Program.ReadInt("House number must be positive. Try again: ");
            }

            return houseNumber;
        }
    }
}
