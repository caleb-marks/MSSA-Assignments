namespace Caleb_Marks_Assignment_6._2
{
    // Generic stack stored in fixed-size array; top marks last pushed item (LIFO)
    internal class StackArray<T>
    {
        private T[] data;
        private int top;

        public int Count { get { return top + 1; } }
        public int Capacity { get { return data.Length; } }

        // Create empty stack with fixed capacity
        public StackArray(int size)
        {
            data = new T[size];
            top = -1; // -1 = empty, not an index
        }

        // Check whether stack has no items
        public bool IsEmpty()
        {
            return top == -1;
        }

        // Check whether every array slot is used
        public bool IsFull()
        {
            return top == data.Length - 1;
        }

        // Add item on top; O(1)
        public void Push(T value)
        {
            if (IsFull())
            {
                throw new InvalidOperationException("Stack is full.");
            }

            data[++top] = value; // move top up first, then store
        }

        // Remove & return top item; O(1)
        public T Pop()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack is empty.");
            }

            T value = data[top];
            data[top] = default!; // clear slot so old item is not kept
            top--;
            return value;
        }

        // Return top item without removing it; O(1)
        public T Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack is empty.");
            }

            return data[top];
        }

        // Print active items top to bottom; O(n)
        public void Display()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Stack is empty.");
                return;
            }

            for (int i = top; i >= 0; i--)
            {
                if (i == top)
                {
                    Console.WriteLine(data[i] + "  <- top");
                }
                else
                {
                    Console.WriteLine(data[i]);
                }
            }
        }
    }
}
