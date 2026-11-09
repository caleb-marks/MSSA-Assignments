# OU Season Analyzer

A Windows desktop app that tells the University of Oklahoma (OU) football season as one story you scroll through. Each section shows one headline and the numbers that prove it. The data comes from ESPN's web API.

I built it as my mini-project at the Microsoft Software & Systems Academy (MSSA), October 2026.

**Built with:** C#, .NET 10, WinForms, HttpClient, async/await, LINQ

## What it does

- Downloads OU's games, player leaders, and national ranks from ESPN, then saves them to CSV files. No database.
- Forecasts the chance of each final win total for the season.
- Finds past OU seasons that started like this one.
- Lets you pick a win or a loss for each game left. Ctrl+Z undoes the newest pick.

## The problem I am most proud of

The forecast must give the chance of every final record. The simple way tries every win-or-loss path, so each new game doubles the work: O(2^n) for n games.

My version keeps one array of win totals and updates it once for each game. That makes the work O(n^2), with n + 1 recursive calls. The code is in [Logic/Recursion.cs](Logic/Recursion.cs).

## Where to look

| Topic | File |
|---|---|
| Forecast (recursion) | [Logic/Recursion.cs](Logic/Recursion.cs), [Logic/Forecast.cs](Logic/Forecast.cs) |
| My own linked list | [Models/GameList.cs](Models/GameList.cs) |
| Merge sort | [Logic/MergeSort.cs](Logic/MergeSort.cs) |
| Binary search | [Logic/BinarySearch.cs](Logic/BinarySearch.cs) |
| Stack (what-if undo) | [Logic/WhatIfStack.cs](Logic/WhatIfStack.cs) |
| Queue (next game, 3-game window) | [Logic/GameQueue.cs](Logic/GameQueue.cs) |
| ESPN download | [Data/EspnClient.cs](Data/EspnClient.cs) |
| CSV load and save | [Data/GameCsv.cs](Data/GameCsv.cs) |
| Self-tests | [Logic/SelfTest.cs](Logic/SelfTest.cs) |

## Tests

The app has a self-test mode with 31 checks. They cover the forecast, merge sort, binary search, the linked list, the queues, a CSV round trip with a bad line, and ESPN JSON parsing.

```
dotnet run --project Caleb_Marks_OU_Season_Analyzer.csproj -- --selftest
```

It writes `selftest.txt`. Exit code 0 means every check passed.

## AI code review

I had OpenAI Codex and Claude Code review the code, then fixed 19 findings from those reviews. Two examples: a crash when ESPN sent a game with no date, and saved stats that blank data could overwrite.

## Run it

You need Windows and the .NET 10 SDK.

Open `Caleb_Marks_OU_Season_Analyzer.slnx` in Visual Studio and press F5, or run:

```
dotnet run --project Caleb_Marks_OU_Season_Analyzer.csproj
```

Select **Update from ESPN** (or press F5 in the app) to download the newest games.
