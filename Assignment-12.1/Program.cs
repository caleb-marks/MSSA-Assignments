// Caleb Marks

namespace Caleb_Marks_Assignment_12._1
{
    internal class Program
    {
        // Show assignment menu & run selected problem
        static void Main(string[] args)
        {
            int choice = 0;
            while (choice != 3)
            {
                Console.WriteLine("\nAssignment 12.1");
                Console.WriteLine("1. Check ransom note");
                Console.WriteLine("2. Check linked list palindrome");
                Console.WriteLine("3. Exit");
                choice = ReadInt("Select an option (1-3): ");
                Console.WriteLine();

                // Run selected problem or exit
                switch (choice)
                {
                    case 1:
                        RunRansomNote();
                        break;

                    case 2:
                        RunPalindromeList();
                        break;

                    case 3:
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Enter a number from 1 to 3.");
                        break;
                }
            }
        }

        // Read both strings & show result
        static void RunRansomNote()
        {
            Console.Write("Enter ransom note: ");
            string ransomNote = Console.ReadLine() ?? "";
            Console.Write("Enter magazine: ");
            string magazine = Console.ReadLine() ?? "";

            Console.WriteLine("Output: " + CanConstruct(ransomNote, magazine));
        }

        // Return true when magazine letters can build ransom note
        static bool CanConstruct(string ransomNote, string magazine)
        {
            Dictionary<char, int> letterCounts = new Dictionary<char, int>();

            // Count each magazine letter
            for (int i = 0; i < magazine.Length; i++)
            {
                if (letterCounts.ContainsKey(magazine[i]))
                {
                    letterCounts[magazine[i]]++;
                }
                else
                {
                    letterCounts.Add(magazine[i], 1);
                }
            }

            // Spend one magazine letter each time ransom note needs it
            for (int i = 0; i < ransomNote.Length; i++)
            {
                // Missing or used up letter: note cannot be built
                if (!letterCounts.ContainsKey(ransomNote[i]) || letterCounts[ransomNote[i]] == 0)
                {
                    return false;
                }

                letterCounts[ransomNote[i]]--;
            }

            return true;
        }

        // Build user's list & show palindrome result
        static void RunPalindromeList()
        {
            int nodeCount = ReadInt("Enter number of values: ");

            // Require one or more values
            while (nodeCount < 1)
            {
                nodeCount = ReadInt("Enter a count greater than zero: ");
            }

            // Add each value last
            SinglyLinkedList list = new SinglyLinkedList();
            for (int i = 0; i < nodeCount; i++)
            {
                list.AddLast(ReadInt("Enter value " + (i + 1) + ": "));
            }

            Console.Write("Linked list: ");
            list.Display();
            Console.WriteLine("Output: " + IsPalindrome(list.Head));
        }

        // Return true when list reads same forward & backward
        static bool IsPalindrome(Node? head)
        {
            int count = 0;
            Node? current = head;

            // Count nodes
            while (current != null)
            {
                count++;
                current = current.Next;
            }

            // Find node starting second half
            Node? secondHalf = head;
            for (int i = 0; i < count / 2; i++)
            {
                if (secondHalf != null)
                {
                    secondHalf = secondHalf.Next;
                }
            }

            // Reverse second half so last node comes first
            Node? reversedHead = Reverse(secondHalf);
            Node? left = head;
            Node? right = reversedHead;
            bool isPalindrome = true;

            // Compare first half & reversed second half
            while (left != null && right != null)
            {
                if (left.Data != right.Data)
                {
                    isPalindrome = false;
                }

                left = left.Next;
                right = right.Next;
            }

            // Reverse second half again so list keeps original order
            Reverse(reversedHead);

            return isPalindrome;
        }

        // Reverse links starting here; return new first node
        static Node? Reverse(Node? start)
        {
            Node? previous = null;
            Node? current = start;

            // Flip each node's link backward
            while (current != null)
            {
                Node? nextNode = current.Next;
                current.Next = previous;
                previous = current;
                current = nextNode;
            }

            return previous;
        }

        // Read whole numbers & retry invalid input
        static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            int number;
            while (!Int32.TryParse(Console.ReadLine(), out number))
            {
                Console.Write("Invalid input. Enter a whole number: ");
            }

            return number;
        }
    }
}
