namespace Caleb_Marks_Assignment_11._2
{
    // Store integer nodes linked one direction
    internal class SinglyLinkedList
    {
        private Node? head;
        private Node? tail;

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

        // Reverse links, reusing same nodes; return new head
        public Node? Reverse()
        {
            Node? previous = null;
            Node? current = head;

            // Old head becomes new tail
            tail = head;

            // Flip each node's link backward
            while (current != null)
            {
                Node? nextNode = current.Next;
                current.Next = previous;
                previous = current;
                current = nextNode;
            }

            // Last node visited becomes new head
            head = previous;
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
