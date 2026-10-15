using Caleb_Marks_OU_Season_Analyzer.Controls;
using Caleb_Marks_OU_Season_Analyzer.Data;
using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Logic;

// Saved-data story checks
// Check chart contrast
internal static partial class SelfTest
{
    // Check saved story content
    private static void CheckStory()
    {
        List<Game> saved = GameCsv.Load(GameCsv.DefaultPath, new List<string>());
        List<StorySection> sections = new StoryFacts(TeamStore.Current, saved, new WhatIfStack()).Build();

        Check("S1 every story section has a clip & a why-line naming the clip's game (opponent or year)", () =>
        {
            List<string> bad = new List<string>();
            foreach (StorySection s in sections)
            {
                ClipInfo? clip = ClipLibrary.All.Find(c => c.Name == s.Clip);
                bool names = clip != null && s.Note.Length > 0
                    && (s.Note.Contains(clip.Opponent) || s.Note.Contains(clip.Date.Year.ToString()));
                if (!names)
                {
                    bad.Add(s.Clip);
                }
            }
            return (sections.Count > 0 && bad.Count == 0, bad.Count == 0 ? $"{sections.Count} of {sections.Count} sections" : "no match: " + string.Join(", ", bad));
        });

        Check("S2 rail players come from the clip's own game or the season leaders, & each has a photo", () =>
        {
            List<string> bad = new List<string>();
            int shown = 0;
            foreach (StorySection s in sections)
            {
                ClipInfo? clip = ClipLibrary.All.Find(c => c.Name == s.Clip);
                Game? game = clip == null ? null : saved.Find(g => g.IsFinal && g.Date.Date == clip.Date.Date && g.Opponent == clip.Opponent);
                foreach (PlayerStar p in s.Players)
                {
                    shown++;
                    bool fromClip = p.EventId.Length == 0 || (game != null && p.EventId == game.EventId);
                    if (!fromClip || !File.Exists(ExtrasStore.PhotoPath(p.AthleteId)))
                    {
                        bad.Add($"{s.Clip}: {p.Name}");
                    }
                }
            }
            return (shown > 0 && bad.Count == 0, bad.Count == 0 ? $"{shown} player cards, all w/ photos" : "bad: " + string.Join(", ", bad));
        });

        Check("S3 chart text 4.5:1 & bars, dots, outlines 3:1 on the lightest & darkest page", () =>
        {
            List<Color> pages = new List<Color> { Palette.Page };
            Color[] blooms = Palette.BloomColors;
            for (int i = 0; i < blooms.Length; i++)
            {
                pages.Add(Palette.DarkestPage(blooms[i], Palette.BloomAlpha(i)));
            }
            double text = pages.Min(p => Math.Min(Palette.Contrast(Palette.Ink, p), Palette.Contrast(Palette.InkSoft, p)));
            double accent = pages.Min(p => Palette.Contrast(Palette.Accent, p));
            double slate = pages.Min(p => Palette.Contrast(Palette.Slate, p));
            double edge = pages.Min(p => Palette.Contrast(Palette.ChartEdge, p));
            bool good = text >= 4.5 && Math.Min(accent, Math.Min(slate, edge)) >= 3.0;
            return (good, $"text {text:0.00}:1, accent {accent:0.00}:1, slate {slate:0.00}:1, outline {edge:0.00}:1");
        });

        Check("S4 every chart row prints its value & the screen-reader summary lists every row", () =>
        {
            int charts = 0;
            List<string> bad = new List<string>();
            foreach (StorySection s in sections.Where(s => s.Chart != null))
            {
                charts++;
                string said = s.Chart!.Describe();
                if (s.Chart.Bars.Count == 0 || s.Chart.Bars.Any(b => b.Text.Length == 0 || !said.Contains(b.Label + " " + b.Text)))
                {
                    bad.Add(s.Clip);
                }
            }
            return (charts > 0 && bad.Count == 0, bad.Count == 0 ? $"{charts} charts" : "bad: " + string.Join(", ", bad));
        });
    }
}
