using System.Windows.Forms.Automation;
using Caleb_Marks_OU_Season_Analyzer.Controls;
using Caleb_Marks_OU_Season_Analyzer.Data;
using Caleb_Marks_OU_Season_Analyzer.Logic;
using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer;

// Scrolling season story
// One section pairs highlights & statistics
// Final section wraps
// C1: presentation layer
public sealed class StoryForm : Form, IPaperForm, IMessageFilter
{
    private const int WM_MOUSEWHEEL = 0x020A;
    // Throttle wheel gestures
    private const int WheelPauseMs = 450;

    private readonly ClipPlayer clip = new ClipPlayer();
    private readonly PictureBox logo = new PictureBox();
    private readonly Label title = new Label();
    private readonly Label seasonLine = new Label();
    private readonly InkButton updateButton = new InkButton();
    private readonly Label status = new Label();
    private readonly StoryPanel panel = new StoryPanel();
    private readonly FlowLayoutPanel picks = new FlowLayoutPanel();
    private readonly InkButton undoButton = new InkButton();
    private readonly InkButton upButton = new InkButton();
    private readonly InkButton downButton = new InkButton();
    private readonly Label counter = new Label();
    private readonly Label hint = new Label();

    private List<Game> games = new List<Game>();
    private readonly WhatIfStack whatIfs = new WhatIfStack();
    private StoryFacts facts = null!;
    private List<StorySection> sections = new List<StorySection>();
    private int index;
    private DateTime lastWheel = DateTime.MinValue;

    // Initialize story window
    public StoryForm(bool offline = false)
    {
        Text = "Oklahoma Sooners Season Analyzer";
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(1100, 700);
        AutoScaleMode = AutoScaleMode.Dpi;
        DoubleBuffered = true;
        KeyPreview = true;
        Font = Palette.Body;
        Team team = TeamStore.Current;
        Palette.UseTeam(team.DrawColor, team.DrawAltColor);
        BackColor = Palette.Page;
        ForeColor = Palette.Ink;

        logo.SizeMode = PictureBoxSizeMode.Zoom;
        logo.BackColor = Color.Transparent;
        logo.AccessibleName = $"{team.FullName} logo";
        title.Text = $"{team.FullName} ({team.Short})";
        title.Font = new Font("Segoe UI Semibold", 18f);
        seasonLine.Font = new Font("Segoe UI", 12f);
        updateButton.Text = "Update from ESPN";
        updateButton.AccessibleDescription = "Download the newest games, stars and national ranks (F5)";
        updateButton.Click += async (sender, e) => await UpdateAsync();
        status.Font = Palette.Small;
        status.ForeColor = Palette.InkSoft;
        status.TextAlign = ContentAlignment.TopRight;
        status.LiveSetting = AutomationLiveSetting.Polite;

        picks.BackColor = Color.Transparent;
        picks.FlowDirection = FlowDirection.LeftToRight;
        picks.WrapContents = true;
        picks.AccessibleName = "What-if picks";
        undoButton.Text = "Undo (Ctrl+Z)";
        undoButton.Click += (sender, e) => Undo();

        upButton.Text = "▲";
        upButton.AccessibleName = "Previous section";
        upButton.Click += (sender, e) => Go(-1);
        downButton.Text = "▼";
        downButton.AccessibleName = "Next section";
        downButton.Primary = true;
        downButton.Click += (sender, e) => Go(1);
        counter.Font = Palette.SmallBold;
        counter.TextAlign = ContentAlignment.MiddleCenter;
        hint.Font = Palette.Small;
        hint.ForeColor = Palette.InkSoft;
        hint.Text = "Scroll for the next fact  ▼";
        hint.TextAlign = ContentAlignment.MiddleCenter;

        clip.BackColor = Color.Transparent;
        panel.HostPicks(picks);
        Controls.AddRange(new Control[] { logo, title, seasonLine, updateButton, status, clip, panel,
            upButton, downButton, counter, hint });
        foreach (Label label in Controls.OfType<Label>().Concat(clip.Controls.OfType<Label>()))
        {
            label.BackColor = Color.Transparent;
            label.AutoSize = false;
            label.UseMnemonic = false;
            if (label.ForeColor == SystemColors.ControlText)
            {
                label.ForeColor = Palette.Ink;
            }
        }

        Load += (sender, e) => Reload(null);
        Shown += (sender, e) => Application.AddMessageFilter(this);
        FormClosing += (sender, e) =>
        {
            Application.RemoveMessageFilter(this);
            clip.Stop();
        };
        Resize += (sender, e) => LayoutSection();
    }

    // Scale logical pixels
    private int Px(int pixels) => pixels * DeviceDpi / 96;

    // Data

    // Reload saved games
    private void Reload(string? message)
    {
        string keep = sections.Count > 0 ? sections[index].Clip : "";
        List<string> problems = new List<string>();
        games = GameCsv.Load(GameCsv.DefaultPath, problems);
        games.Sort((a, b) => a.Date.CompareTo(b.Date));
        whatIfs.DropFinished(GameQueue.Upcoming(games, games.Count == 0 ? 0 : games.Max(g => g.Season)));
        logo.Image = TeamLogo.Get(TeamStore.Current.Id, TeamStore.Current.Location, TeamStore.Current.DrawColor);
        Rebuild();
        index = Math.Max(0, sections.FindIndex(s => s.Clip == keep));
        status.Text = message ?? (problems.Count > 0 ? string.Join(" ", problems.Take(2))
            : File.Exists(GameCsv.DefaultPath) ? $"ESPN data as of {File.GetLastWriteTime(GameCsv.DefaultPath):MMM d, h:mm tt}" : "No ESPN data saved yet");
        ShowSection(true);
    }

    // Recalculate story facts
    private void Rebuild()
    {
        facts = new StoryFacts(TeamStore.Current, games, whatIfs);
        sections = facts.Build();
        seasonLine.Text = facts.HeaderLine;
    }

    // Navigation

    // Cycle story section
    // C2: wrap section index
    private void Go(int step)
    {
        if (sections.Count == 0)
        {
            return;
        }
        index = (index + step + sections.Count) % sections.Count;
        ShowSection(true);
    }

    // Handle window-wide scrolling
    // C2: handle wheel navigation
    public bool PreFilterMessage(ref Message m)
    {
        if (m.Msg != WM_MOUSEWHEEL || ActiveForm != this)
        {
            return false;
        }
        int delta = (short)((long)m.WParam >> 16);
        if ((DateTime.Now - lastWheel).TotalMilliseconds >= WheelPauseMs && delta != 0)
        {
            Go(delta < 0 ? 1 : -1);
        }
        lastWheel = DateTime.Now;
        return true;
    }

    // Handle keyboard shortcuts
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Down:
            case Keys.PageDown:
                Go(1);
                return true;
            case Keys.Space when !(ActiveControl is Button):
                Go(1);
                return true;
            case Keys.Up:
            case Keys.PageUp:
                Go(-1);
                return true;
            case Keys.Home:
                index = 0;
                ShowSection(true);
                return true;
            case Keys.End:
                index = sections.Count - 1;
                ShowSection(true);
                return true;
            case Keys.Control | Keys.Z:
                Undo();
                return true;
            case Keys.F5:
                _ = UpdateAsync();
                return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    // Display current section
    private void ShowSection(bool newClip)
    {
        if (sections.Count == 0)
        {
            panel.Show(new StorySection { Kicker = "No data", Headline = "No games saved yet. Press Update from ESPN (F5)." }, false);
            return;
        }
        StorySection section = sections[index];
        BuildPicks(section.WhatIf);
        counter.Text = $"{index + 1} of {sections.Count}";
        hint.Text = index == sections.Count - 1 ? "Scroll to start over  ↺" : "Scroll for the next fact  ▼";
        LayoutSection();
        panel.Show(section, newClip);
        if (newClip)
        {
            ClipInfo? info = ClipLibrary.All.Find(c => c.Name == section.Clip);
            clip.ShowClips(info == null ? new List<ClipInfo>() : new List<ClipInfo> { info });
        }
        clip.Note = section.Note;
    }

    // What-if predictions

    // Build prediction buttons
    private void BuildPicks(bool show)
    {
        picks.Visible = show;
        foreach (Control old in picks.Controls.Cast<Control>().ToList())
        {
            picks.Controls.Remove(old);
            if (old != undoButton)
            {
                old.Dispose();
            }
        }
        if (!show)
        {
            return;
        }
        Dictionary<Game, bool> chosen = whatIfs.NewestPicks();
        foreach (Game game in facts.Left)
        {
            string pick = chosen.TryGetValue(game, out bool win) ? (win ? "W" : "L") : $"{game.WinPct:0}%";
            InkButton button = new InkButton { Text = $"{game.ShortName}: {pick}", Primary = chosen.ContainsKey(game) && chosen[game] };
            button.AccessibleDescription = "Click to pick a win, click again for a loss";
            button.Click += (sender, e) => Pick(game);
            picks.Controls.Add(button);
        }
        undoButton.Enabled = whatIfs.Count > 0;
        picks.Controls.Add(undoButton);
    }

    // Toggle game prediction
    private void Pick(Game game)
    {
        bool win = !(whatIfs.NewestPicks().TryGetValue(game, out bool pickedWin) && pickedWin);
        whatIfs.Push(game, win);
        RefreshPicks($"What-if: {game.Opponent} as a {(win ? "win" : "loss")}. Ctrl+Z undoes it.");
    }

    // Undo newest prediction
    private void Undo()
    {
        WhatIf? removed = whatIfs.Undo();
        RefreshPicks(removed == null ? "Nothing to undo." : $"Undid {removed.Game.Opponent} {(removed.Win ? "win" : "loss")}.");
    }

    // Refresh forecast display
    private void RefreshPicks(string message)
    {
        string keep = sections[index].Clip;
        Rebuild();
        index = Math.Max(0, sections.FindIndex(s => s.Clip == keep));
        status.Text = message;
        ShowSection(false);
    }

    // ESPN updates

    // Update saved team data
    private async Task UpdateAsync()
    {
        if (!updateButton.Enabled)
        {
            return;
        }
        updateButton.Enabled = false;
        UseWaitCursor = true;
        string message;
        try
        {
            Team team = TeamStore.Current;
            UpdateResult result = await EspnClient.UpdateCsvAsync(TeamStore.GamesPath(team), team.Id,
                new Progress<string>(text => status.Text = text));
            List<Game> saved = GameCsv.Load(TeamStore.GamesPath(team), new List<string>());
            string extras = await ExtrasClient.UpdateAsync(team, saved, new Progress<string>(text => status.Text = text));
            message = result.Message + (extras.Length > 0 ? " " + extras : "");
        }
        catch (Exception error)
        {
            message = $"Update stopped, so the app kept your saved games. ({error.Message})";
        }
        finally
        {
            updateButton.Enabled = true;
            UseWaitCursor = false;
        }
        Reload(message);
    }

    // Layout

    // Arrange story columns
    private void LayoutSection()
    {
        if (ClientSize.Width < 200 || ClientSize.Height < 200)
        {
            return;
        }
        int m = Px(32);
        int w = ClientSize.Width;
        int h = ClientSize.Height;

        // Position header controls
        logo.SetBounds(m, Px(16), Px(64), Px(64));
        title.SetBounds(logo.Right + Px(14), Px(14), Px(560), Px(38));
        seasonLine.SetBounds(title.Left, title.Bottom, Px(560), Px(28));
        updateButton.SetBounds(w - m - Px(190), Px(20), Px(190), Px(38));
        status.SetBounds(w - m - Px(600), updateButton.Bottom + Px(4), Px(600), Px(22));
        int top = Px(100);
        int bottom = h - Px(24);

        // Position navigation controls
        int arrowWidth = Px(56);
        int arrowX = w - m - arrowWidth;
        int middle = (top + bottom) / 2;
        upButton.SetBounds(arrowX, middle - Px(70), arrowWidth, Px(48));
        counter.SetBounds(arrowX - Px(20), middle - Px(18), arrowWidth + Px(40), Px(36));
        downButton.SetBounds(arrowX, middle + Px(22), arrowWidth, Px(48));

        // Fit video column
        int contentWidth = arrowX - Px(24) - m;
        int clipWidth = contentWidth * 55 / 100;
        int extra = Px(112);
        clipWidth = Math.Min(clipWidth, (bottom - top - extra - Px(36)) * 16 / 9);
        int clipHeight = clipWidth * 9 / 16 + extra;
        int clipTop = top + Math.Max(0, (bottom - Px(36) - top - clipHeight) / 2);
        clip.SetBounds(m - Px(12), clipTop, clipWidth + Px(12), clipHeight);
        hint.SetBounds(m, clip.Bottom + Px(4), clipWidth, Px(28));

        // Position story panel
        int x = m + clipWidth + Px(40);
        panel.SetBounds(x, top, Math.Max(1, arrowX - Px(24) - x), bottom - top);
    }

    // Screenshot capture

    // Save rendered section images
    // Include intermediate animation frame
    public static int Shoot(string folder)
    {
        Directory.CreateDirectory(folder);
        using StoryForm form = new StoryForm();
        form.WindowState = FormWindowState.Normal;
        form.StartPosition = FormStartPosition.Manual;
        form.ShowInTaskbar = false;
        form.Show();
        // Match screen working area
        form.Bounds = Screen.FromControl(form).WorkingArea;
        Application.DoEvents();
        int saved = 0;
        form.panel.PaintTimes = new List<double>();
        for (int i = 0; i < form.sections.Count; i++)
        {
            form.index = i;
            form.ShowSection(true);
            if (i == 0)
            {
                // Capture entrance progress
                DateTime until = DateTime.Now.AddMilliseconds(Motion.Length);
                while (DateTime.Now < until)
                {
                    Application.DoEvents();
                }
                Save(form, Path.Combine(folder, "section-01-mid.png"));
            }
            DateTime end = DateTime.Now.AddMilliseconds(Motion.Build * 2);
            while (DateTime.Now < end)
            {
                Application.DoEvents();
            }
            Save(form, Path.Combine(folder, $"section-{i + 1:00}.png"));
            saved++;
        }
        List<double> times = form.panel.PaintTimes;
        times.Sort();
        File.WriteAllText(Path.Combine(folder, "frames.txt"), times.Count == 0 ? "no frames" :
            $"{times.Count} panel frames over {form.sections.Count} entrances: median {times[times.Count / 2]:0.0} ms, " +
            $"95th {times[times.Count * 95 / 100]:0.0} ms, max {times[^1]:0.0} ms (one frame = {Motion.Tick} ms)");
        // Write section metadata table
        System.Text.StringBuilder table = new System.Text.StringBuilder("| # | Section | Clip | Players beside it | Why the clip fits |\n|---|---|---|---|---|\n");
        for (int i = 0; i < form.sections.Count; i++)
        {
            StorySection s = form.sections[i];
            string caption = ClipLibrary.All.Find(c => c.Name == s.Clip)?.Caption ?? "";
            string players = s.Players.Count == 0 ? "none (no OU game on file)" : string.Join(", ", s.Players.Select(p => p.Name));
            table.Append($"| {i + 1} | {s.Kicker} | {caption} | {players} | {s.Note} |\n");
        }
        File.WriteAllText(Path.Combine(folder, "sections.md"), table.ToString());
        form.Close();
        return saved == form.sections.Count && saved > 0 ? 0 : 1;
    }

    // Save rendered window
    private static void Save(Form form, string path)
    {
        using Bitmap bitmap = new Bitmap(form.ClientSize.Width, form.ClientSize.Height);
        form.DrawToBitmap(bitmap, form.ClientRectangle);
        bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
    }

    // Paper background

    // Paint paper background
    protected override void OnPaintBackground(PaintEventArgs e) => Watercolor.PaintPage(e.Graphics, this, e.ClipRectangle);

    // Define text-safe regions
    Rectangle[] IPaperForm.QuietBands()
    {
        int x = clip.Right + Px(20);
        return new Rectangle[] { new Rectangle(0, 0, ClientSize.Width, Px(96)), new Rectangle(x, Px(96), Math.Max(1, ClientSize.Width - x), ClientSize.Height) };
    }
}
