namespace Caleb_Marks_Assignment_6._1
{
    // One house: data part & link
    internal class HouseNode
    {
        public int HouseNumber { get; set; }
        public string HouseAddress { get; set; }
        public string HouseType { get; set; }
        public HouseNode? Next { get; set; }

        // Store house details; new node links nowhere yet
        public HouseNode(int houseNumber, string houseAddress, string houseType)
        {
            HouseNumber = houseNumber;
            HouseAddress = houseAddress;
            HouseType = houseType;
            Next = null;
        }
    }
}
