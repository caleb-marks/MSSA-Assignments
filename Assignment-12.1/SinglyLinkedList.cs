namespace Caleb_Marks_Assignment_12._1
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
