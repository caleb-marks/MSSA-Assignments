namespace Caleb_Marks_OU_Season_Analyzer.Models;

// Doubly linked game node
// Next = newer; Prev = older
// C4: doubly linked node
internal class GameNode
{
    public Game Data;
    public GameNode? Next;
    public GameNode? Prev;

    // Store game
    public GameNode(Game game)
    {
        Data = game;
    }
}

// Chronological doubly linked list
internal class GameList
{
    // Newest completed game
    private GameNode? lastFinal;

    // List endpoints & count
    public GameNode? Head;
    public GameNode? Tail;
    public int Size;

    // O(1): check empty list
    public bool IsEmpty()
    {
        return Size == 0;
    }

    // O(1): append game
    // Tail avoids traversal
    // C4: constant-time append
    public void AddLast(Game game)
    {
        GameNode newNode = new GameNode(game);

        // Initialize empty list
        if (IsEmpty())
        {
            Head = newNode;
            Tail = newNode;
        }
        else
        {
            // Connect neighboring nodes
            Tail!.Next = newNode;
            newNode.Prev = Tail;
            Tail = newNode;
        }

        // Track newest completed game
        if (game.IsFinal)
        {
            lastFinal = newNode;
        }
        Size++;
    }

    // O(n): find game node
    // Missing game returns null
    public GameNode? Find(Game game)
    {
        GameNode? node = Head;
        while (node != null)
        {
            // Match game reference
            if (node.Data == game)
            {
                return node;
            }
            node = node.Next;
        }
        return null;
    }

    // O(1): latest result or head
    public GameNode? LastFinal()
    {
        if (lastFinal != null)
        {
            return lastFinal;
        }
        return Head;
    }

    // O(n): find one-based position
    // Missing node returns zero
    public int PositionOf(GameNode node)
    {
        int position = 1;
        GameNode? walk = Head;
        while (walk != null)
        {
            if (walk == node)
            {
                return position;
            }
            position++;
            walk = walk.Next;
        }
        return 0;
    }
}
