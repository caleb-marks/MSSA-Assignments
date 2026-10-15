namespace Caleb_Marks_OU_Season_Analyzer.Models;

// One predicted game result
internal class WhatIf
{
    public Game Game;
    public bool Win;

    // Set game prediction
    public WhatIf(Game game, bool win)
    {
        Game = game;
        Win = win;
    }
}
