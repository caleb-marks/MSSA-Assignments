namespace Caleb_Marks_Assignment_7._3
{
    // One binary search tree node: value & two child links
    internal class TreeNode
    {
        public int Value { get; set; }
        public TreeNode? Left { get; set; }
        public TreeNode? Right { get; set; }

        // Set value & start both links empty
        public TreeNode(int value)
        {
            Value = value;
            Left = null;
            Right = null;
        }
    }
}
