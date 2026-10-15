namespace Caleb_Marks_Assignment_6._1
{
    // Single linked list holding house nodes
    internal class HouseLinkedList
    {
        private HouseNode? head;
        private HouseNode? tail;
        private int size;

        public int Size { get { return size; } }

        // Start empty list
        public HouseLinkedList()
        {
            head = null;
            tail = null;
            size = 0;
        }

        // Check whether list has no houses
        public bool IsEmpty()
        {
            return size == 0;
        }

        // Append house; it becomes new tail
        public void AddLast(int houseNumber, string houseAddress, string houseType)
        {
            HouseNode newNode = new HouseNode(houseNumber, houseAddress, houseType);
            if (tail == null)
            {
                // Empty list: head & tail share only node
                head = newNode;
            }
            else
            {
                // Link old tail first, then move tail
                tail.Next = newNode;
            }

            tail = newNode;
            size++;
        }

        // Walk list & print every house
        public void Display()
        {
            if (IsEmpty())
            {
                Console.WriteLine("List is empty.");
                return;
            }

            HouseNode? temp = head;
            while (temp != null)
            {
                PrintHouse(temp);
                temp = temp.Next;
            }
        }

        // Linear search: compare each house number
        public HouseNode? Search(int houseNumber)
        {
            HouseNode? temp = head;
            while (temp != null)
            {
                if (temp.HouseNumber == houseNumber)
                {
                    return temp;
                }

                temp = temp.Next;
            }

            return null;
        }

        // Print one house's details
        public static void PrintHouse(HouseNode house)
        {
            Console.WriteLine("House #" + house.HouseNumber + " | " + house.HouseAddress + " | " + house.HouseType);
        }
    }
}
