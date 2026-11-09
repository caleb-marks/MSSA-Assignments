namespace Caleb_Marks_Assignment_6._3
{
    // Waiting caller's details & next-caller link
    internal class CallerNode
    {
        public int TicketNumber { get; set; }
        public string CallerName { get; set; }
        public string PhoneNumber { get; set; }
        public CallerNode? Next { get; set; }

        // Store caller details; Next starts null
        public CallerNode(int ticketNumber, string callerName, string phoneNumber)
        {
            TicketNumber = ticketNumber;
            CallerName = callerName;
            PhoneNumber = phoneNumber;
            Next = null;
        }
    }
}
