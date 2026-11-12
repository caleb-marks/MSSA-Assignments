namespace Caleb_Marks_Assignment_12._2
{
    // Store integer nodes linked one direction
    internal class SinglyLinkedList
    {
        private Node? head;
        private Node? tail;

        // Return first node; read-only
        public Node? Head
        {
            get
            {
                return head;
            }
        }

        // Add new node last
        public void AddLast(int value)
        {
            Node newNode = new Node(value);

            // Empty list: new node becomes head & tail
            if (tail == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                tail = newNode;
            }
        }

        // Remove every node holding value; return new head
        public Node? RemoveAll(int value)
        {
            // Advance head while it matches
            while (head != null && head.Data == value)
            {
                head = head.Next;
            }

            Node? current = head;

            // Unlink next node when it matches; otherwise move forward
            while (current != null && current.Next != null)
            {
                if (current.Next.Data == value)
                {
                    current.Next = current.Next.Next;
                }
                else
                {
                    current = current.Next;
                }
            }

            // Last node left becomes tail
            tail = current;
            return head;
        }

        // Show bracketed values
        public void Display()
        {
            Console.Write("[");
            Node? current = head;

            // Walk list & separate values
            while (current != null)
            {
                Console.Write(current.Data);
                if (current.Next != null)
                {
                    Console.Write(",");
                }
                current = current.Next;
            }
            Console.WriteLine("]");
        }
    }
}
