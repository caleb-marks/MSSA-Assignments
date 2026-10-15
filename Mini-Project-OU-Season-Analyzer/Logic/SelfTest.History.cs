using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Logic;

// Historical comparison checks
internal static partial class SelfTest
{
    // Create sample historical game
    private static PastGame G(int season, int week, string oppId, int? points, int? oppPoints, int? rank)
    {
        return new PastGame(season, week, false, new DateTime(season, 9, 1).AddDays(week * 7), oppId, "Team " + oppId, "Home",
            points, oppPoints, "", rank, null);
    }

    // Build sample seasons
    private static List<PastGame> SampleHistory()
    {
        return new List<PastGame>
        {
            G(2023, 1, "10", 30, 10, 10), G(2023, 2, "11", 24, 20, 9), G(2023, 3, "251", 14, 21, 7), G(2023, 4, "12", null, null, 6),
            G(2022, 1, "20", 35, 0, 15), G(2022, 2, "21", 28, 27, 14), G(2022, 3, "251", 10, 24, 12), G(2022, 4, "22", 30, 3, 9),
            G(2021, 1, "251", 10, 13, 20), G(2021, 2, "31", 21, 14, 21), G(2021, 3, "32", 17, 10, 19), G(2021, 4, "33", 7, 20, 18),
            G(2020, 1, "40", 60, 0, 5), G(2020, 2, "41", 3, 30, 4), G(2020, 3, "251", 21, 20, 9), G(2020, 4, "42", 21, 17, 12),
            G(2019, 1, "50", 7, 3, null), G(2019, 2, "51", 9, 3, null)
        };
    }

    // Format season list
    private static string Years(List<SeasonEnd> seasons)
    {
        string years = "";
        foreach (SeasonEnd season in seasons)
        {
            years += (years.Length > 0 ? "," : "") + season.Season;
        }
        return years;
    }

    // Check historical verdicts
    private static void CheckHistory()
    {
        Check("History same start", () =>
        {
            List<Comp> comps = History.Build(SampleHistory(), 2023);
            string years = Years(comps[0].Matches);
            return (years == "2022,2021,2020" && comps[0].Now == "2-1 after 3 games", years + " / " + comps[0].Now + " (2019 too short)");
        });

        // Check postseason classifications
        bool flags = History.IsPlayoffLevel(2017, "ROSE BOWL GAME PRESENTED BY NORTHWESTERN MUTUAL - CFP SEMIFINAL")
            && !History.IsPlayoffLevel(2016, "ALLSTATE SUGAR BOWL")
            && History.IsPlayoffLevel(2006, "FIESTA BOWL AT GLENDALE AZ")
            && !History.IsPlayoffLevel(2012, "AT&T COTTON BOWL")
            && History.IsPlayoffLevel(2015, "")
            && !History.IsPlayoffLevel(1999, "")
            && new SeasonEnd(1, 10, 2, "", 0, true).Finish == Finish.LockedIn && new SeasonEnd(1, 12, 1, "").Finish == Finish.Cooked;
        Check("History playoff-level flag", flags, "CFP semifinal yes, 2016 Sugar no, 2006 Fiesta yes, 2012 Cotton no, blank 2015 yes (list), blank 1999 no");

        // Check tied vote
        List<SeasonEnd> pair = new List<SeasonEnd> { new SeasonEnd(1, 10, 2, "", 0, true), new SeasonEnd(2, 6, 6, "") };
        HistoryVerdict tied = History.Decide(new List<Comp> { new Comp("start", "a", "", pair, ""), new Comp("b", "b", "", pair, "") });
        Check("History tie -> Cooked", tied.Call == Finish.Cooked && tied.Matches == 2 && !tied.ShowOdds,
            tied.Call + ", " + tied.Matches + " seasons, no odds");

        // Check same-start voter selection
        List<SeasonEnd> five = new List<SeasonEnd>
        {
            new SeasonEnd(14, 12, 1, "", 0, true), new SeasonEnd(13, 11, 2, "", 0, true), new SeasonEnd(12, 11, 1, "", 0, true),
            new SeasonEnd(11, 9, 4, ""), new SeasonEnd(10, 7, 6, "")
        };
        List<SeasonEnd> six = new List<SeasonEnd>(five);
        six.Add(new SeasonEnd(15, 2, 10, ""));
        HistoryVerdict odds = History.Decide(new List<Comp>
        {
            new Comp("start", "Same start", "", five, ""), new Comp("margin", "margin", "", six, "")
        });
        Check("History same-start vote & odds",
            odds.Call == Finish.LockedIn && odds.Matches == 5 && odds.ShowOdds && odds.Percent(Finish.LockedIn) == 60,
            odds.Call + ", " + odds.Matches + " seasons, " + odds.Percent(Finish.LockedIn) + "% Locked in");
    }
}
