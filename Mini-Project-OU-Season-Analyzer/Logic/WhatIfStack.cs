using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Logic;

// Last-in, first-out predictions
// Undo removes newest prediction
internal class WhatIfStack
{
    // Newest prediction stays top
    private Stack<WhatIf> picks = new Stack<WhatIf>();

    // Prediction count
    public int Count
    {
        get { return picks.Count; }
    }

    // Push newest prediction
    // C4: stack push
    public void Push(Game game, bool win)
    {
        picks.Push(new WhatIf(game, win));
    }

    // O(1): undo newest prediction
    // C4: stack pop
    public WhatIf? Undo()
    {
        if (picks.Count == 0)
        {
            return null;
        }
        return picks.Pop();
    }

    // Clear predictions
    public void Clear()
    {
        picks.Clear();
    }

    // O(w): list oldest predictions first
    // Newer predictions override older ones
    public List<WhatIf> OldestFirst()
    {
        // Reverse stack copy
        List<WhatIf> list = new List<WhatIf>(picks);
        list.Reverse();
        return list;
    }

    // O(w): collect newest game predictions
    public Dictionary<Game, bool> NewestPicks()
    {
        Dictionary<Game, bool> newest = new Dictionary<Game, bool>();

        // Keep first encountered prediction
        foreach (WhatIf pick in picks)
        {
            newest.TryAdd(pick.Game, pick.Win);
        }
        return newest;
    }

    // Remove completed-game predictions
    // Return removed count
    public int DropFinished(Queue<Game> upcoming)
    {
        // Index upcoming games
        Dictionary<string, Game> ahead = new Dictionary<string, Game>();
        foreach (Game game in upcoming)
        {
            ahead.TryAdd(game.EventId, game);
        }

        // Rebuild chronological stack
        Stack<WhatIf> kept = new Stack<WhatIf>();
        foreach (WhatIf pick in OldestFirst())
        {
            if (ahead.TryGetValue(pick.Game.EventId, out Game? stillAhead))
            {
                kept.Push(new WhatIf(stillAhead, pick.Win));
            }
        }
        int dropped = picks.Count - kept.Count;
        picks = kept;
        return dropped;
    }
}
