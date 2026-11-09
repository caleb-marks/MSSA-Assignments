using System.Text;
using Caleb_Marks_OU_Season_Analyzer.Controls;
using Caleb_Marks_OU_Season_Analyzer.Data;
using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Logic;

// Repeatable application checks
internal static partial class SelfTest
{
    private static StringBuilder report = new StringBuilder();
    private static int passedCount;
    private static int checkCount;

    // Sample schedule includes invalid date
    private const string ScheduleJson = """
        {"events":[
          {"id":"1","date":null,"week":{"number":7},
           "competitions":[{"status":{"type":{"completed":false,"state":"pre"}},"competitors":[]}]},
          {"id":"401","date":"2026-10-10T19:30Z","timeValid":true,"week":{"number":7},
           "competitions":[{"neutralSite":true,"status":{"type":{"completed":false,"state":"pre"}},
             "competitors":[{"homeAway":"home","team":{"id":"251","location":"Texas"}},
                            {"homeAway":"away","team":{"id":"201","location":"Oklahoma"}}]}]}
        ]}
        """;

    // Run all checks
    public static SelfTestResult Run()
    {
        report = new StringBuilder();
        passedCount = 0;
        checkCount = 0;
        CheckForecast();
        CheckWhatIfStack();
        CheckSortAndSearch();
        CheckLinkedList();
        CheckQueue();
        CheckCsv();
        CheckSchedule();
        CheckColors();
        CheckHistory();
        CheckClips();
        CheckStory();
        return new SelfTestResult(report.ToString(), passedCount, checkCount);
    }

    // Record check outcome
    private static void Check(string name, bool passed, string detail)
    {
        checkCount++;
        if (passed)
        {
            passedCount++;
        }
        report.AppendLine((passed ? "PASS  " : "FAIL  ") + name + " (" + detail + ")");
    }

    // Run isolated check
    private static void Check(string name, Func<(bool Passed, string Detail)> test)
    {
        try
        {
            (bool passed, string detail) = test();
            Check(name, passed, detail);
        }
        catch (Exception error)
        {
            Check(name, false, error.GetType().Name + ": " + error.Message);
        }
    }

    // Create scheduled sample game
    private static Game NewGame(string opponent, int month, int day, double winPct)
    {
        Game game = new Game();
        game.Season = 2026;
        game.Opponent = opponent;
        game.Date = new DateTime(2026, month, day);
        game.Status = Game.StatusScheduled;
        if (winPct > 0)
        {
            game.WinPct = winPct;
        }
        return game;
    }

    // Create completed sample game
    private static Game FinalGame(string opponent, int month, int day, int ouScore, int oppScore)
    {
        Game game = NewGame(opponent, month, day, 0);
        game.Status = Game.StatusFinal;
        game.OUScore = ouScore;
        game.OppScore = oppScore;
        return game;
    }

    // Create complete sample box score
    private static Game StatGame(int week, int ouYards)
    {
        Game game = FinalGame("Team " + week, 9, 5, 21, 14);
        game.Week = week;
        game.Date = new DateTime(2026, 9, 5).AddDays(7 * week);
        game.OUPlays = 100;
        game.OUYards = ouYards;
        game.OppPlays = 100;
        game.OppYards = 500;
        game.OU3rdAtt = 10;
        game.OU3rdMade = 5;
        game.OppRushAtt = 30;
        game.OppRushYds = 120;
        game.OURushAtt = 30;
        game.OURushYds = 150;
        game.OUPassYds = 250;
        return game;
    }

    // Check forecast probabilities
    private static void CheckForecast()
    {
        List<Game> left = new List<Game>
        {
            NewGame("Texas", 10, 10, 19.9), NewGame("Kentucky", 10, 17, 70.1), NewGame("Mississippi State", 10, 24, 41.5),
            NewGame("South Carolina", 10, 31, 70.0), NewGame("Florida", 11, 7, 26.8), NewGame("Ole Miss", 11, 14, 55.5),
            NewGame("Texas A&M", 11, 21, 52.9), NewGame("Missouri", 11, 28, 56.3)
        };
        double[] totals = Recursion.FinalWins(Forecast.Chances(left, new WhatIfStack()), 2);
        int most = Forecast.MostLikely(totals);
        double bowl = Forecast.BowlChance(totals);
        Check("Forecast most likely", most == 6 && Math.Abs(totals[6] - 0.293) < 0.0006,
            most + " wins, " + (totals[6] * 100).ToString("0.0") + "%");
        Check("Forecast bowl chance", Math.Abs(bowl - 0.628) < 0.0006, (bowl * 100).ToString("0.0") + "%");
        // Check linear call count
        Check("Recursion call count", Recursion.Calls == left.Count + 1, Recursion.Calls + " calls for " + left.Count + " games (n + 1)");

        // Check seven-win probability
        double[] lastSeven = new double[] { 0.9, 0.8, 0.7, 0.6, 0.5, 0.5, 0.5 };
        double playoff = Forecast.PlayoffChance(Recursion.FinalWins(lastSeven, 3), 12);
        double product = 0.9 * 0.8 * 0.7 * 0.6 * 0.5 * 0.5 * 0.5;
        Check("Forecast playoff chance", Math.Abs(playoff - product) < 0.000001,
            (playoff * 100).ToString("0.00") + "% = 7 game chances multiplied (10-2 or better)");
    }

    // Check prediction undo
    private static void CheckWhatIfStack()
    {
        Game texas = NewGame("Texas", 10, 10, 19.9);
        List<Game> left = new List<Game> { texas, NewGame("Kentucky", 10, 17, 70.1) };
        WhatIfStack whatIfs = new WhatIfStack();

        whatIfs.Push(texas, true);
        double win = Forecast.Chances(left, whatIfs)[0];
        whatIfs.Push(texas, false);
        double loss = Forecast.Chances(left, whatIfs)[0];
        whatIfs.Undo();
        double afterUndo = Forecast.Chances(left, whatIfs)[0];
        Check("What-if push", win == 1 && loss == 0, "win pick 100%, newer loss pick 0%");
        Check("What-if undo", afterUndo == 1 && whatIfs.Count == 1, "undo brings back win pick, 1 pick left");
    }

    // Check sorting & searching
    private static void CheckSortAndSearch()
    {
        // Equal keys retain original order
        List<string> items = new List<string> { "5a", "2b", "9c", "2d", "5e", "1f" };
        MergeSort.Sort(items, (x, y) => x[0].CompareTo(y[0]));
        string sorted = string.Join(" ", items);
        Check("Merge sort keeps ties in order", sorted == "1f 2b 2d 5a 5e 9c", sorted);

        Game[] byName = BinarySearch.SortByName(new List<Game>
        {
            NewGame("Texas", 10, 10, 0), NewGame("Georgia", 9, 26, 0), NewGame("Missouri", 11, 28, 0),
            NewGame("Mississippi State", 10, 24, 0), NewGame("Michigan", 9, 12, 0), NewGame("UTEP", 9, 4, 0)
        });
        List<Game> geo = BinarySearch.Find(byName, "geo");
        Check("Binary search \"geo\"", geo.Count == 1 && geo[0].Opponent == "Georgia", "Georgia");
        Check("Binary search \"miss\"", BinarySearch.Find(byName, "miss").Count == 2, "Mississippi State + Missouri");
        Check("Binary search \"zzz\"", BinarySearch.Find(byName, "zzz").Count == 0, "no match");
    }

    // Check linked-list operations
    private static void CheckLinkedList()
    {
        GameList list = new GameList();
        Game utep = FinalGame("UTEP", 9, 4, 51, 0);
        Game michigan = FinalGame("Michigan", 9, 12, 10, 17);
        Game texas = NewGame("Texas", 10, 10, 19.9);
        list.AddLast(utep);
        list.AddLast(michigan);
        list.AddLast(texas);

        GameNode? middle = list.Find(michigan);
        bool links = middle != null && middle.Prev != null && middle.Next != null
            && middle.Prev.Data == utep && middle.Next.Data == texas && middle.Prev.Prev == null && middle.Next.Next == null;
        Check("Linked list add & find", list.Size == 3 && middle != null && list.Find(NewGame("Nobody", 9, 1, 0)) == null,
            "3 games, missing game = null");
        Check("Linked list prev/next", links, "Michigan sits between UTEP & Texas");
        Check("Linked list position", middle != null && list.PositionOf(middle) == 2, "Michigan is 2 of 3");
        GameNode? last = list.LastFinal();
        Check("Opens newest finished game", last != null && last.Data == michigan, "Michigan");
    }

    // Check queue behavior
    private static void CheckQueue()
    {
        Game done = NewGame("Texas", 10, 10, 19.9);
        done.Status = Game.StatusFinal;
        Game next = NewGame("Kentucky", 10, 17, 70.1);
        Queue<Game> upcoming = GameQueue.Upcoming(new List<Game> { done, next }, 2026);
        Check("Queue upcoming games", upcoming.Count == 1 && upcoming.Peek() == next, upcoming.Count + " game left, Kentucky first");

        // Expected averages: 1.80, then 0.19
        List<Game> games = new List<Game> { StatGame(1, 837), StatGame(2, 513), StatGame(3, 691), StatGame(4, 352) };
        List<TrendPoint> points = GameQueue.ThreeGameAverages(games, 2026);
        double first = 0;
        double second = 0;
        if (points.Count == 4 && points[2].YppAvg3.HasValue && points[3].YppAvg3.HasValue)
        {
            first = points[2].YppAvg3.GetValueOrDefault();
            second = points[3].YppAvg3.GetValueOrDefault();
        }
        bool right = Math.Abs(first - 1.80) < 0.01 && Math.Abs(second - 0.19) < 0.01;
        Check("Queue 3-game averages", right, first.ToString("0.00") + " then " + second.ToString("0.00"));
    }

    // Check CSV round trip
    private static void CheckCsv()
    {
        Check("CSV round trip & bad line", () =>
        {
            string path = Path.Combine(Path.GetTempPath(), "ou-selftest-" + Guid.NewGuid().ToString("N") + ".csv");
            try
            {
                GameCsv.Save(path, new List<Game> { NewGame("Texas", 10, 10, 19.9) });
                File.AppendAllText(path, "2026,7,2026-10-17,only,five\n");
                List<string> problems = new List<string>();
                List<Game> loaded = GameCsv.Load(path, problems);
                bool right = loaded.Count == 1 && loaded[0].Opponent == "Texas" && loaded[0].WinPct == 19.9
                    && problems.Count == 1 && problems[0].Contains("skipped");
                return (right, loaded.Count + " game loaded, " + problems.Count + " problem message");
            }
            finally
            {
                File.Delete(path);
            }
        });
    }

    // Check schedule parsing
    private static void CheckSchedule()
    {
        Check("ESPN schedule parse", () =>
        {
            List<string> skipped = new List<string>();
            List<Game> read = EspnClient.ParseSchedule(ScheduleJson, 2026, 2, skipped);
            bool right = read.Count == 1 && read[0].Opponent == "Texas" && read[0].Site == "Neutral"
                && skipped.Count == 1 && skipped[0] == "1";
            return (right, read.Count + " game read, " + skipped.Count + " skipped");
        });
    }

    // Check team color contrast
    private static void CheckColors()
    {
        Color crimson = Team.Oklahoma.DrawColor;
        Color accent = Palette.ReadableAccent(crimson, Team.Oklahoma.DrawAltColor);
        double contrast = Palette.Contrast(crimson, Color.White);
        Check("OU color contrast", accent.ToArgb() == crimson.ToArgb() && contrast >= 4.5, contrast.ToString("0.0") + ":1 vs white");
    }
}
