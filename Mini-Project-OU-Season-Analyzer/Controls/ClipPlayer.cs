using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;

namespace Caleb_Marks_OU_Season_Analyzer.Controls;

// Looping highlight player
// Missing video uses poster
// Poster covers startup
// Background swaps keep scrolling responsive
internal class ClipPlayer : Panel
{
    // Enable live playback
    public static bool Allowed = false;

    // Shared VLC instance
    private static LibVLC? vlc;
    private static bool vlcFailed = false;

    private readonly VideoView video = new VideoView();
    private readonly PictureBox poster = new PictureBox();
    private readonly Label caption = new Label();
    private readonly Label note = new Label();
    private readonly Label credit = new Label();
    private readonly InkButton backButton = new InkButton();
    private readonly InkButton nextButton = new InkButton();
    private MediaPlayer? player;
    // Serialize clip changes
    private Task swap = Task.CompletedTask;
    private int playId;
    // Latest started request identifier
    private volatile int startedId = -1;
    private volatile bool posterUp;
    private List<ClipInfo> clips = new List<ClipInfo>();
    private int index = 0;

    // Current clip name
    public string ClipName { get; private set; } = "";

    // Check visible playback
    public bool IsPlaying => player != null && video.Visible && player.IsPlaying;

    // Clip explanation
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string Note
    {
        get => note.Text;
        set
        {
            note.Text = value;
            note.Visible = value.Length > 0;
            PerformLayout();
        }
    }

    // Build player controls
    public ClipPlayer()
    {
        Padding = new Padding(12, 0, 0, 0);
        video.AccessibleName = "Highlight video";
        poster.AccessibleName = "Highlight poster";
        poster.SizeMode = PictureBoxSizeMode.Zoom;
        poster.BackColor = Color.Black;
        video.BackColor = Color.Black;
        caption.Font = Palette.BodyBold;
        caption.AutoSize = false;
        note.Font = Palette.Body;
        note.AutoSize = false;
        note.Visible = false;
        credit.Font = Palette.Small;
        credit.AutoSize = false;
        backButton.Text = "◀ Back";
        nextButton.Text = "Next ▶";
        backButton.AccessibleName = "Previous highlight";
        nextButton.AccessibleName = "Next highlight";
        backButton.Click += (sender, e) => ChangeClip(-1);
        nextButton.Click += (sender, e) => ChangeClip(1);
        Controls.Add(video);
        Controls.Add(poster);
        Controls.Add(caption);
        Controls.Add(note);
        Controls.Add(credit);
        Controls.Add(backButton);
        Controls.Add(nextButton);
    }

    // Show section clips
    public void ShowClips(List<ClipInfo> stepClips)
    {
        clips = stepClips;
        index = 0;
        Visible = clips.Count > 0;
        backButton.Visible = clips.Count > 1;
        nextButton.Visible = clips.Count > 1;
        PlayCurrent();
    }

    // Cycle selected clip
    private void ChangeClip(int step)
    {
        if (clips.Count == 0)
        {
            return;
        }
        index = (index + step + clips.Count) % clips.Count;
        PlayCurrent();
    }

    // Start selected clip
    private void PlayCurrent()
    {
        if (clips.Count == 0)
        {
            Stop();
            return;
        }
        ClipInfo clip = clips[index];
        ClipName = clip.Name;
        caption.Text = clip.Caption;
        credit.Text = clip.Credit + (clips.Count > 1 ? $"   ·   {index + 1} of {clips.Count}" : "");
        poster.Image?.Dispose();
        poster.Image = ClipLibrary.Poster(clip);
        poster.Visible = true;
        posterUp = true;
        poster.BringToFront();
        string? path = ClipLibrary.VideoPath(clip);
        LibVLC? core = Vlc();
        if (path == null || core == null)
        {
            Halt();
            video.Visible = false;
            PerformLayout();
            return;
        }
        if (player == null)
        {
            player = new MediaPlayer(core);
            player.Mute = true;
            // Playback progress triggers poster removal
            player.TimeChanged += (sender, e) =>
            {
                if (e.Time > 0 && posterUp)
                {
                    FirstFrame(startedId);
                }
            };
            video.MediaPlayer = player;
        }
        video.Visible = true;
        int id = ++playId;
        MediaPlayer target = player;
        swap = swap.ContinueWith(_ =>
        {
            // Skip obsolete requests
            if (id != playId)
            {
                return;
            }
            using Media media = new Media(core, path, FromType.FromPath, ":input-repeat=65535", ":no-audio");
            startedId = id;
            target.Play(media);
        }, TaskScheduler.Default);
        PerformLayout();
    }

    // Reveal current playback
    // Ignore older requests
    private void FirstFrame(int id)
    {
        if (id != playId)
        {
            return;
        }
        if (!IsHandleCreated || IsDisposed)
        {
            return;
        }
        try
        {
            BeginInvoke(async () =>
            {
                await Task.Delay(60);
                if (id == playId && video.Visible && player != null && player.IsPlaying)
                {
                    poster.Visible = false;
                    posterUp = false;
                }
            });
        }
        catch (InvalidOperationException)
        {
            // Ignore closed window
        }
    }

    // Stop playback asynchronously
    private void Halt()
    {
        playId++;
        MediaPlayer? target = player;
        if (target != null)
        {
            swap = swap.ContinueWith(_ => target.Stop(), TaskScheduler.Default);
        }
    }

    // Queue player disposal
    // Earlier operations finish first
    public void Stop()
    {
        ClipName = "";
        playId++;
        if (player != null)
        {
            MediaPlayer old = player;
            player = null;
            video.MediaPlayer = null;
            swap = swap.ContinueWith(_ =>
            {
                old.Stop();
                old.Dispose();
            }, TaskScheduler.Default);
        }
    }

    // Initialize shared VLC
    private static LibVLC? Vlc()
    {
        if (!Allowed || vlcFailed)
        {
            return null;
        }
        if (vlc == null)
        {
            try
            {
                Core.Initialize();
                // C7: LibVLCSharp playback
                vlc = new LibVLC("--quiet", "--no-osd", "--no-video-title-show");
            }
            catch (Exception)
            {
                vlcFailed = true;
            }
        }
        return vlc;
    }

    // Arrange player controls
    protected override void OnLayout(LayoutEventArgs levent)
    {
        base.OnLayout(levent);
        int x = Padding.Left;
        int width = Math.Max(16, ClientSize.Width - x);
        int videoHeight = width * 9 / 16;
        int gap = LogicalToDeviceUnits(6);
        video.SetBounds(x, 0, width, videoHeight);
        poster.SetBounds(x, 0, width, videoHeight);
        int lineHeight = LogicalToDeviceUnits(26);
        caption.SetBounds(x, videoHeight + gap, width, lineHeight);
        int creditHeight = LogicalToDeviceUnits(22);
        int noteHeight = note.Visible ? TextRenderer.MeasureText(note.Text, note.Font, new Size(width, int.MaxValue),
            TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl).Height + gap : 0;
        note.SetBounds(x, caption.Bottom, width, noteHeight);
        credit.SetBounds(x, note.Visible ? note.Bottom : caption.Bottom, width, creditHeight);
        int buttonWidth = LogicalToDeviceUnits(100);
        int buttonHeight = LogicalToDeviceUnits(34);
        backButton.SetBounds(x, credit.Bottom + gap, buttonWidth, buttonHeight);
        nextButton.SetBounds(x + width - buttonWidth, credit.Bottom + gap, buttonWidth, buttonHeight);
    }

    // Release player resources
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Stop();
            poster.Image?.Dispose();
        }
        base.Dispose(disposing);
    }
}
