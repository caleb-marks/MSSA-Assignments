namespace Caleb_Marks_Assignment_12._2
{
    // Hold one integer & link next node
    internal class Node
    {
        public int Data { get; set; }
        public Node? Next { get; set; }

        // Create unlinked node holding value
        public Node(int value)
        {
            Data = value;
            Next = null;
        }
    }
}
