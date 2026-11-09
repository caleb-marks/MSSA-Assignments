namespace Caleb_Marks_Assignment_7._3
{
    // Binary search tree: smaller values left, larger values right
    internal class BinarySearchTree
    {
        // Starting point; kept private so client code cannot change it
        private TreeNode? root;

        // Let client code read root only
        public TreeNode? Root
        {
            get
            {
                return root;
            }
        }

        // Start empty tree
        public BinarySearchTree()
        {
            root = null;
        }

        // Add value; return false when value already exists
        public bool Add(int value)
        {
            if (root == null)
            {
                root = new TreeNode(value);
                return true;
            }

            return AddNode(root, value);
        }

        // Go left or right recursively, then attach new node once empty spot found
        private bool AddNode(TreeNode node, int value)
        {
            if (value == node.Value)
            {
                return false;
            }

            if (value < node.Value)
            {
                if (node.Left == null)
                {
                    node.Left = new TreeNode(value);
                    return true;
                }

                return AddNode(node.Left, value);
            }
            else
            {
                if (node.Right == null)
                {
                    node.Right = new TreeNode(value);
                    return true;
                }

                return AddNode(node.Right, value);
            }
        }

        // Find node holding value; return its subtree or null
        public TreeNode? Search(int value)
        {
            return SearchNode(root, value);
        }

        // Walk left or right; stop once match or empty link found
        private TreeNode? SearchNode(TreeNode? node, int value)
        {
            if (node == null || node.Value == value)
            {
                return node;
            }

            if (value < node.Value)
            {
                return SearchNode(node.Left, value);
            }
            else
            {
                return SearchNode(node.Right, value);
            }
        }

        // Print all three traversal orders, starting given node
        public void Display(TreeNode? node)
        {
            Console.Write("Pre-order:  ");
            PreOrder(node);
            Console.WriteLine();
            Console.Write("In-order:   ");
            InOrder(node);
            Console.WriteLine();
            Console.Write("Post-order: ");
            PostOrder(node);
            Console.WriteLine();
        }

        // Root first, then left subtree, then right subtree
        public void PreOrder(TreeNode? tempRoot)
        {
            if (tempRoot != null)
            {
                Console.Write(tempRoot.Value + " ");
                PreOrder(tempRoot.Left);
                PreOrder(tempRoot.Right);
            }
        }

        // Left subtree, then root, then right subtree
        public void InOrder(TreeNode? tempRoot)
        {
            if (tempRoot != null)
            {
                InOrder(tempRoot.Left);
                Console.Write(tempRoot.Value + " ");
                InOrder(tempRoot.Right);
            }
        }

        // Left subtree, then right subtree, then root
        public void PostOrder(TreeNode? tempRoot)
        {
            if (tempRoot != null)
            {
                PostOrder(tempRoot.Left);
                PostOrder(tempRoot.Right);
                Console.Write(tempRoot.Value + " ");
            }
        }
    }
}
