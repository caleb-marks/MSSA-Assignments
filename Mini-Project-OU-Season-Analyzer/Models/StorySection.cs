namespace Caleb_Marks_OU_Season_Analyzer.Models;

// One labeled statistic
internal sealed record StoryStat(string Big, string Label);

// One story section
internal sealed class StorySection
{
    public string Clip = "";
    public string Kicker = "";
    public string Headline = "";
    public List<StoryStat> Stats = new List<StoryStat>();
    // Clip explanation
    public string Note = "";
    // Featured players
    public List<PlayerStar> Players = new List<PlayerStar>();
    // Use defensive player categories
    public bool Defense;
    // Optional chart
    public StoryChart? Chart;
    // Optional verdict badge
    public Finish? Badge;
    // Enable what-if controls
    public bool WhatIf;
}
