namespace Caleb_Marks_Assignment_6._3
{
    // Linked-list caller queue: join rear, leave front (FIFO)
    internal class CallerQueue
    {
        private CallerNode? front;
        private CallerNode? rear;
        private int size;

        public int Size { get { return size; } }

        // Start empty queue
        public CallerQueue()
        {
            front = null;
            rear = null;
            size = 0;
        }

        // Check whether no callers are waiting
        public bool IsEmpty()
        {
            return size == 0;
        }

        // Enqueue: new caller becomes rear (AddLast); O(1)
        public void Enqueue(int ticketNumber, string callerName, string phoneNumber)
        {
            CallerNode newNode = new CallerNode(ticketNumber, callerName, phoneNumber);
            if (rear == null)
            {
                // Empty queue: front & rear share only node
                front = newNode;
            }
            else
            {
                // Link old rear first, then move rear
                rear.Next = newNode;
            }

            rear = newNode;
            size++;
        }

        // Remove & return front caller (RemoveFirst); O(1)
        public CallerNode Dequeue()
        {
            if (front == null)
            {
                throw new InvalidOperationException("No callers in queue.");
            }

            CallerNode caller = front;
            front = front.Next;
            size--;
            if (front == null)
            {
                // Last caller left, so reset rear too
                rear = null;
            }

            // Clear served caller's link
            caller.Next = null;
            return caller;
        }

        // Return front caller, keep queue unchanged; O(1)
        public CallerNode Peek()
        {
            if (front == null)
            {
                throw new InvalidOperationException("No callers in queue.");
            }

            return front;
        }

        // Print every waiting caller, front first; O(n)
        public void Display()
        {
            if (IsEmpty())
            {
                Console.WriteLine("No callers in queue.");
                return;
            }

            int position = 1;
            CallerNode? temp = front;
            while (temp != null)
            {
                Console.Write(position + ". ");
                PrintCaller(temp);
                temp = temp.Next;
                position++;
            }
        }

        // Print one caller's details
        public static void PrintCaller(CallerNode caller)
        {
            Console.WriteLine("Ticket #" + caller.TicketNumber + " | " + caller.CallerName + " | " + caller.PhoneNumber);
        }
    }
}
