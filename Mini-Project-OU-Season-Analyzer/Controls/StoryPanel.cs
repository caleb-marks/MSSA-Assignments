using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms.Automation;
using Caleb_Marks_OU_Season_Analyzer.Data;
using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Controls;

// Animated story content
// Double buffering reduces flicker
// Expose content using accessibility objects
internal sealed class StoryPanel : Control
{
    // Entrance stagger, milliseconds
    private const int Stagger = 60;
    // Entrance travel, logical pixels
    private const int Slide = 18;

    private readonly System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = Motion.Tick };
    private readonly List<Part> parts = new List<Part>();
    private readonly List<Card> cards = new List<Card>();
    private readonly Dictionary<string, Font> fonts = new Dictionary<string, Font>();
    private readonly Dictionary<string, Bitmap> photos = new Dictionary<string, Bitmap>();
    private StorySection? section;
    private Control? picks;
    private DateTime started = DateTime.MinValue;
    private float scale = 1f;
    private RectangleF badgeBox;
    private RectangleF chartBox;
    private RectangleF railBox;
    private string railTitle = "";
    private int chartDelay;
    private int last;

    // Optional paint timings
    public List<double>? PaintTimes;

    // Initialize story rendering
    public StoryPanel()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
            | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.Selectable, false);
        BackColor = Color.Transparent;
        AccessibleRole = AccessibleRole.Grouping;
        AccessibleName = "Story section";
        timer.Tick += (sender, e) =>
        {
            if (Elapsed >= last)
            {
                timer.Stop();
            }
            Invalidate();
        };
    }

    // Attach prediction controls
    public void HostPicks(Control control)
    {
        picks = control;
        Controls.Add(control);
    }

    // Display section
    public void Show(StorySection next, bool animate)
    {
        // Detect section change
        bool fresh = section == null || section.Clip != next.Clip || section.Kicker != next.Kicker;
        bool said = section != null && section.Headline == next.Headline;
        section = next;
        AccessibleName = next.Kicker;
        Relayout();
        started = animate && fresh && Motion.Enabled ? DateTime.Now : DateTime.MinValue;
        if (started != DateTime.MinValue)
        {
            timer.Start();
        }
        Invalidate();
        if ((fresh || !said) && IsHandleCreated)
        {
            AccessibilityObject.RaiseAutomationNotification(AutomationNotificationKind.Other,
                AutomationNotificationProcessing.ImportantMostRecent, next.Kicker + ". " + next.Headline);
        }
    }

    // Elapsed entrance time
    private int Elapsed => started == DateTime.MinValue ? int.MaxValue / 2 : (int)(DateTime.Now - started).TotalMilliseconds;

    // Calculate eased progress
    private float Progress(int delay, int length = Motion.Length) => Motion.EaseOut((Elapsed - delay) / (float)length);

    // Initialize layout
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        Relayout();
        Invalidate();
    }

    // Refresh resized layout
    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        Relayout();
    }

    // Layout

    // Fit section layout
    private void Relayout()
    {
        if (section == null || Width < 50 || Height < 50 || !IsHandleCreated)
        {
            return;
        }
        using Graphics g = CreateGraphics();
        for (scale = 1f; scale > 0.55f; scale -= 0.05f)
        {
            if (Place(g, 0, false) <= Height)
            {
                break;
            }
        }
        float used = Place(g, 0, false);
        Place(g, Math.Max(0, (Height - used) / 3), true);
    }

    // Measure or place section parts
    private float Place(Graphics g, float y, bool apply)
    {
        StorySection s = section!;
        float top = y;
        // Hide player rail when narrow
        bool rail = s.Players.Count > 0 && Width - Px(176 + 28) >= Px(360);
        float railWidth = rail ? Px(176) : 0;
        float width = Width - railWidth - (rail ? Px(28) : 0);
        int delay = 0;
        List<Part> placed = new List<Part>();

        // Measure text element
        Part Add(string text, Font font, Color color, float x, float partWidth, string speak, AccessibleRole role = AccessibleRole.StaticText)
        {
            SizeF size = g.MeasureString(text.Length == 0 ? " " : text, font, (int)Math.Max(1, partWidth), Part.Format);
            Part part = new Part(text, font, color, new RectangleF(x, y, partWidth, size.Height + 1), delay, speak, role);
            placed.Add(part);
            return part;
        }

        Part kicker = Add(s.Kicker.ToUpperInvariant(), Face("Segoe UI Semibold", 12f), Palette.Accent, 0, width, s.Kicker);
        y += kicker.Box.Height + Px(8);
        delay += Stagger;
        if (s.Badge.HasValue)
        {
            float size = Px(150) * scale;
            if (apply)
            {
                badgeBox = new RectangleF(0, y, width, size);
            }
            placed.Add(new Part("", Face("Segoe UI", 9f), Palette.Ink, new RectangleF(0, y, width, size), delay,
                $"{Badge.Word(s.Badge.Value)} badge: {Badge.Subline(s.Badge.Value)}", AccessibleRole.Graphic) { Badge = true });
            y += size + Px(16);
            delay += Stagger;
        }
        Part headline = Add(s.Headline, Face("Segoe UI Semibold", 26f), Palette.Ink, 0, width, s.Headline);
        y += headline.Box.Height + Px(20);
        delay += Stagger;

        // Align statistic columns
        Font bigFont = Face("Segoe UI Semibold", 30f);
        Font wordFont = Face("Segoe UI", 13f);
        float bigWidth = s.Stats.Count == 0 ? 0 : Math.Min(width / 2, s.Stats.Max(t => g.MeasureString(t.Big, bigFont, int.MaxValue, Part.Format).Width) + 2);
        foreach (StoryStat stat in s.Stats)
        {
            float wordsWidth = width - bigWidth - Px(16);
            float bigHeight = g.MeasureString(stat.Big, bigFont, (int)bigWidth, Part.Format).Height;
            float wordsHeight = g.MeasureString(stat.Label, wordFont, (int)Math.Max(1, wordsWidth), Part.Format).Height;
            float row = Math.Max(bigHeight, wordsHeight);
            Part big = Add(stat.Big, bigFont, Palette.Ink, 0, bigWidth, stat.Big + " " + stat.Label);
            big.Box.Height = row;
            big.Middle = true;
            Part words = Add(stat.Label, wordFont, Palette.InkSoft, bigWidth + Px(16), wordsWidth, "");
            words.Box.Height = row;
            words.Middle = true;
            y += row + Px(12);
            delay += Stagger;
        }

        // Position chart
        if (s.Chart != null && s.Chart.Bars.Count > 0)
        {
            y += Px(6);
            float height = ChartHeight(s.Chart);
            if (apply)
            {
                chartBox = new RectangleF(0, y, width, height);
                chartDelay = delay;
            }
            y += height + Px(12);
            delay += Stagger;
        }
        else if (apply)
        {
            chartBox = RectangleF.Empty;
        }

        // Arrange prediction buttons
        if (picks != null)
        {
            picks.Visible = s.WhatIf;
        }
        if (picks != null && s.WhatIf)
        {
            int buttonWidth = (int)(width - Px(12)) / 2;
            int buttonHeight = (int)Px(36);
            foreach (Control button in picks.Controls)
            {
                button.Size = new Size(buttonWidth - (int)Px(8), buttonHeight);
                button.Margin = new Padding(0, 0, (int)Px(8), (int)Px(8));
            }
            int rows = (picks.Controls.Count + 1) / 2;
            int height = rows * (buttonHeight + (int)Px(8));
            if (apply)
            {
                picks.SetBounds(0, (int)y, (int)width, height);
            }
            y += height;
        }

        // Position player rail
        float railBottom = top;
        List<Card> placedCards = new List<Card>();
        if (rail)
        {
            float x = Width - railWidth;
            float cardY = headline.Box.Top;
            bool season = s.Players[0].EventId.Length == 0;
            float titleHeight = g.MeasureString("Season leaders", Face("Segoe UI Semibold", 10f), (int)railWidth, Part.Format).Height;
            if (apply)
            {
                railTitle = season ? "Season leaders" : "Leaders in this clip's game";
                railBox = new RectangleF(x, cardY, railWidth, titleHeight);
            }
            cardY += titleHeight + Px(6);
            int cardDelay = Stagger * 2;
            foreach (PlayerStar star in s.Players)
            {
                float photo = Px(84) * Math.Max(scale, 0.75f);
                string line = StatLine(star);
                float nameHeight = g.MeasureString(star.Name, Face("Segoe UI Semibold", 10.5f), (int)railWidth, Part.Format).Height;
                float lineHeight = g.MeasureString(line, Face("Segoe UI", 9.5f), (int)railWidth, Part.Format).Height;
                float height = photo + Px(4) + nameHeight + lineHeight;
                // Cache prepared player photos
                Bitmap? face = apply ? Photo(star, (int)photo) : null;
                placedCards.Add(new Card(star, line, new RectangleF(x, cardY, railWidth, height), photo, cardDelay, face));
                cardY += height + Px(14);
                cardDelay += Stagger * 2;
            }
            railBottom = cardY;
        }

        if (apply)
        {
            parts.Clear();
            parts.AddRange(placed);
            cards.Clear();
            cards.AddRange(placedCards);
            last = Math.Max(delay + Motion.Build, Stagger * 2 * (cards.Count + 1) + Motion.Length);
        }
        return Math.Max(y, railBottom);
    }

    // Format player statistic
    private static string StatLine(PlayerStar star) => star.Label + " · " + star.Display;

    // Measure chart height
    private float ChartHeight(StoryChart chart)
    {
        float title = Face("Segoe UI Semibold", 10.5f).GetHeight() + Px(6);
        float rows = chart.Bars.Count * RowHeight;
        return title + rows + (chart.Kind == ChartKind.Rank ? Face("Segoe UI", 9f).GetHeight() + Px(2) : 0);
    }

    private float RowHeight => Math.Max(Face("Segoe UI", 10f).GetHeight() + Px(6), Px(26) * scale);

    // Painting

    // Paint paper background
    protected override void OnPaintBackground(PaintEventArgs e) => Watercolor.PaintPage(e.Graphics, this, e.ClipRectangle);

    // Render current frame
    protected override void OnPaint(PaintEventArgs e)
    {
        if (section == null)
        {
            return;
        }
        long start = System.Diagnostics.Stopwatch.GetTimestamp();
        DrawSection(e.Graphics, section);
        PaintTimes?.Add(System.Diagnostics.Stopwatch.GetElapsedTime(start).TotalMilliseconds);
    }

    // Draw animated section
    private void DrawSection(Graphics g, StorySection section)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        foreach (Part part in parts)
        {
            float p = Progress(part.Delay);
            if (p <= 0f)
            {
                continue;
            }
            float shift = (1 - p) * Px(Slide);
            if (part.Badge)
            {
                DrawBadge(g, section.Badge!.Value, new RectangleF(badgeBox.X, badgeBox.Y + shift, badgeBox.Width, badgeBox.Height), p);
                continue;
            }
            using SolidBrush brush = new SolidBrush(Color.FromArgb((int)(255 * p), part.Color));
            RectangleF box = new RectangleF(part.Box.X, part.Box.Y + shift, part.Box.Width, part.Box.Height + (part.Middle ? 0 : Px(4)));
            g.DrawString(part.Text, part.Font, brush, box, part.Middle ? Part.Middled : Part.Format);
        }
        if (section.Chart != null && !chartBox.IsEmpty)
        {
            DrawChart(g, section.Chart);
        }
        if (cards.Count > 0)
        {
            float p = Progress(Stagger);
            using SolidBrush title = new SolidBrush(Color.FromArgb((int)(255 * p), Palette.InkSoft));
            g.DrawString(railTitle, Face("Segoe UI Semibold", 10f), title, railBox, Part.Format);
            foreach (Card card in cards)
            {
                DrawCard(g, card);
            }
        }
    }

    // Draw verdict badge
    private static void DrawBadge(Graphics g, Finish finish, RectangleF box, float p)
    {
        float size = box.Height;
        GraphicsState state = g.Save();
        g.TranslateTransform(box.X + size / 2, box.Y + size / 2);
        g.ScaleTransform(0.85f + 0.15f * p, 0.85f + 0.15f * p);
        Badge.Draw(g, finish, new RectangleF(-size / 2, -size / 2, size, size));
        g.Restore(state);
        using Font wordFont = new Font("Segoe UI Semibold", Math.Max(8f, size * 0.30f), GraphicsUnit.Pixel);
        using Font subFont = new Font("Segoe UI", Math.Max(6f, size * 0.12f), GraphicsUnit.Pixel);
        float x = box.X + size + size / 6;
        SizeF word = g.MeasureString(Badge.Word(finish), wordFont);
        float y = box.Y + (size - word.Height - subFont.GetHeight()) / 2;
        using SolidBrush crimson = new SolidBrush(Color.FromArgb((int)(255 * p), Badge.Crimson));
        using SolidBrush ink = new SolidBrush(Color.FromArgb((int)(255 * p), Palette.Ink));
        g.DrawString(Badge.Word(finish), wordFont, crimson, x, y);
        g.DrawString(Badge.Subline(finish), subFont, ink, x + size / 40, y + word.Height * 0.92f);
    }

    // Charts

    // Draw labeled chart rows
    private void DrawChart(Graphics g, StoryChart chart)
    {
        float p = Progress(chartDelay);
        if (p <= 0f)
        {
            return;
        }
        int alpha = (int)(255 * p);
        Font titleFont = Face("Segoe UI Semibold", 10.5f);
        Font labelFont = Face("Segoe UI", 10f);
        Font boldFont = Face("Segoe UI Semibold", 10f);
        Font valueFont = Face("Segoe UI Semibold", 10f);
        using SolidBrush titleBrush = new SolidBrush(Color.FromArgb(alpha, Palette.InkSoft));
        using SolidBrush ink = new SolidBrush(Color.FromArgb(alpha, Palette.Ink));
        g.DrawString(chart.Title, titleFont, titleBrush, chartBox.X, chartBox.Y);

        float top = chartBox.Y + titleFont.GetHeight() + Px(6);
        float labelWidth = Math.Min(chartBox.Width * 0.38f, chart.Bars.Max(b => g.MeasureString(b.Label, boldFont).Width) + Px(8));
        float valueWidth = chart.Bars.Max(b => g.MeasureString(b.Text, valueFont).Width) + Px(6);
        float area = Math.Max(Px(40), chartBox.Width - labelWidth - (chart.Kind == ChartKind.Split ? valueWidth * 2 : valueWidth));
        float left = chartBox.X + labelWidth + (chart.Kind == ChartKind.Split ? valueWidth : 0);
        float rowHeight = RowHeight;
        float barHeight = rowHeight * 0.62f;
        double max = Math.Max(1e-9, chart.Bars.Max(b => Math.Abs(b.Value)));

        if (chart.Kind == ChartKind.Split)
        {
            // Draw zero baseline
            float center = left + area / 2;
            using Pen zero = new Pen(Color.FromArgb(alpha, Palette.ChartEdge), Px(1.5f));
            g.DrawLine(zero, center, top - Px(2), center, top + rowHeight * chart.Bars.Count);
        }
        if (chart.Kind == ChartKind.Rank)
        {
            using SolidBrush soft = new SolidBrush(Color.FromArgb(alpha, Palette.InkSoft));
            Font axisFont = Face("Segoe UI", 9f);
            float axisY = top + rowHeight * chart.Bars.Count + Px(2);
            g.DrawString("#1 (best)", axisFont, soft, left - Px(2), axisY);
            string end = $"#{chart.Of}";
            g.DrawString(end, axisFont, soft, left + area - g.MeasureString(end, axisFont).Width + Px(2), axisY);
        }

        for (int i = 0; i < chart.Bars.Count; i++)
        {
            ChartBar bar = chart.Bars[i];
            float y = top + i * rowHeight;
            float grow = Progress(chartDelay + Motion.Length / 2 + i * 40, Motion.Build - Motion.Length);
            RectangleF labelBox = new RectangleF(chartBox.X, y, labelWidth - Px(6), rowHeight);
            using (StringFormat right = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap })
            {
                g.DrawString(bar.Label, bar.Highlight ? boldFont : labelFont, ink, labelBox, right);
            }
            float midY = y + rowHeight / 2;
            using StringFormat middle = new StringFormat { LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap };
            switch (chart.Kind)
            {
                case ChartKind.Bars:
                {
                    float length = (float)(Math.Abs(bar.Value) / max) * area * grow;
                    RectangleF box = new RectangleF(left, midY - barHeight / 2, Math.Max(Px(2), length), barHeight);
                    FillBar(g, box, bar, alpha);
                    g.DrawString(bar.Text, valueFont, ink, new RectangleF(box.Right + Px(4), y, valueWidth + Px(40), rowHeight), middle);
                    break;
                }
                case ChartKind.Split:
                {
                    float center = left + area / 2;
                    float length = (float)(Math.Abs(bar.Value) / max) * (area / 2) * grow;
                    bool plus = bar.Value >= 0;
                    RectangleF box = new RectangleF(plus ? center : center - length, midY - barHeight / 2, Math.Max(Px(2), length), barHeight);
                    FillBar(g, box, bar, alpha);
                    float textWidth = g.MeasureString(bar.Text, valueFont).Width;
                    float textX = plus ? box.Right + Px(4) : box.Left - Px(4) - textWidth;
                    g.DrawString(bar.Text, valueFont, ink, new RectangleF(textX, y, textWidth + Px(4), rowHeight), middle);
                    break;
                }
                case ChartKind.Rank:
                {
                    using Pen track = new Pen(Color.FromArgb(alpha, Palette.ChartEdge), Px(1.5f));
                    g.DrawLine(track, left, midY, left + area, midY);
                    float share = chart.Of <= 1 ? 0 : (float)((bar.Value - 1) / (chart.Of - 1));
                    float x = left + area * share * grow;
                    float r = Px(7);
                    RectangleF dot = new RectangleF(x - r, midY - r, r * 2, r * 2);
                    FillBar(g, dot, bar, alpha, round: true);
                    float textWidth = g.MeasureString(bar.Text, valueFont).Width;
                    float textX = x + r + Px(4) + textWidth <= left + area ? x + r + Px(4) : x - r - Px(4) - textWidth;
                    RectangleF textBox = new RectangleF(textX, y - rowHeight * 0.38f, textWidth + Px(4), rowHeight);
                    g.DrawString(bar.Text, valueFont, ink, textBox, middle);
                    break;
                }
            }
        }
    }

    // Draw chart mark
    private static void FillBar(Graphics g, RectangleF box, ChartBar bar, int alpha, bool round = false)
    {
        using GraphicsPath shape = new GraphicsPath();
        if (round)
        {
            shape.AddEllipse(box);
        }
        else
        {
            shape.AddRectangle(box);
        }
        if (bar.Loss)
        {
            using SolidBrush pale = new SolidBrush(Color.FromArgb(alpha, Palette.Loss));
            using HatchBrush stripes = new HatchBrush(HatchStyle.WideUpwardDiagonal, Color.FromArgb(alpha * Palette.LossStripe.A / 255, Palette.LossStripe), Color.Transparent);
            using Pen edge = new Pen(Color.FromArgb(alpha, Palette.ChartEdge), 1.5f);
            g.FillPath(pale, shape);
            g.FillPath(stripes, shape);
            g.DrawPath(edge, shape);
            return;
        }
        using SolidBrush fill = new SolidBrush(Color.FromArgb(alpha, bar.Highlight ? Palette.Accent : Palette.Slate));
        g.FillPath(fill, shape);
    }

    // Players

    // Draw player card
    private void DrawCard(Graphics g, Card card)
    {
        float p = Progress(card.Delay);
        if (p <= 0f)
        {
            return;
        }
        int alpha = (int)(255 * p);
        float shift = (1 - p) * Px(Slide + 6);
        RectangleF circle = new RectangleF(card.Box.X + (card.Box.Width - card.Photo) / 2 + shift, card.Box.Y, card.Photo, card.Photo);
        Bitmap face = card.Face ?? Photo(card.Star, (int)card.Photo);
        using (ImageAttributes fade = new ImageAttributes())
        {
            ColorMatrix matrix = new ColorMatrix { Matrix33 = p };
            fade.SetColorMatrix(matrix);
            g.DrawImage(face, Rectangle.Round(circle), 0, 0, face.Width, face.Height, GraphicsUnit.Pixel, fade);
        }
        using Pen ring = new Pen(Color.FromArgb(alpha, Palette.Accent), Px(2));
        g.DrawEllipse(ring, circle);
        using StringFormat center = new StringFormat(Part.Format) { Alignment = StringAlignment.Center };
        using SolidBrush ink = new SolidBrush(Color.FromArgb(alpha, Palette.Ink));
        using SolidBrush soft = new SolidBrush(Color.FromArgb(alpha, Palette.InkSoft));
        Font nameFont = Face("Segoe UI Semibold", 10.5f);
        float y = circle.Bottom + Px(4);
        RectangleF nameBox = new RectangleF(card.Box.X + shift, y, card.Box.Width, card.Box.Bottom - y);
        g.DrawString(card.Star.Name, nameFont, ink, nameBox, center);
        y += g.MeasureString(card.Star.Name, nameFont, (int)card.Box.Width, Part.Format).Height;
        g.DrawString(card.Line, Face("Segoe UI", 9.5f), soft, new RectangleF(card.Box.X + shift, y, card.Box.Width, card.Box.Bottom - y + Px(4)), center);
    }

    // Cache circular headshot
    // Missing photo uses initials
    private Bitmap Photo(PlayerStar star, int size)
    {
        size = Math.Max(8, size);
        string path = ExtrasStore.PhotoPath(star.AthleteId);
        long stamp = File.Exists(path) ? File.GetLastWriteTimeUtc(path).Ticks : 0;
        string key = star.AthleteId + "@" + size + "@" + stamp;
        if (photos.TryGetValue(key, out Bitmap? cached))
        {
            return cached;
        }
        Bitmap face = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
        using (Graphics g = Graphics.FromImage(face))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            using GraphicsPath round = new GraphicsPath();
            round.AddEllipse(0, 0, size - 1, size - 1);
            using SolidBrush back = new SolidBrush(Palette.Backdrop);
            g.FillPath(back, round);
            g.SetClip(round);
            Image? picture = null;
            try
            {
                picture = File.Exists(path) ? Image.FromFile(path) : null;
            }
            catch (Exception error) when (error is OutOfMemoryException || error is IOException)
            {
                // Use initials fallback
            }
            if (picture != null)
            {
                // Crop portrait
                float zoom = Math.Max(size / (float)picture.Width, size / (float)picture.Height) * 1.12f;
                float w = picture.Width * zoom;
                float h = picture.Height * zoom;
                g.DrawImage(picture, (size - w) / 2, size * 0.04f, w, h);
                picture.Dispose();
            }
            else
            {
                string initials = string.Concat(star.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(w => w[0]));
                using Font font = new Font("Segoe UI Semibold", size * 0.32f, GraphicsUnit.Pixel);
                using SolidBrush ink = new SolidBrush(Palette.Ink);
                using StringFormat center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(initials, font, ink, new RectangleF(0, 0, size, size), center);
            }
        }
        photos[key] = face;
        return face;
    }

    // Helpers

    // Scale logical pixels
    private float Px(float pixels) => pixels * DeviceDpi / 96f;

    // Cache scaled font
    private Font Face(string family, float size)
    {
        float want = (float)Math.Round(size * scale, 1);
        string key = family + "|" + want;
        if (!fonts.TryGetValue(key, out Font? font))
        {
            font = new Font(family, want);
            fonts[key] = font;
        }
        return font;
    }

    // Release rendering resources
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            timer.Dispose();
            foreach (Font font in fonts.Values)
            {
                font.Dispose();
            }
            foreach (Bitmap photo in photos.Values)
            {
                photo.Dispose();
            }
        }
        base.Dispose(disposing);
    }

    // Screen readers

    // Create accessible panel
    protected override AccessibleObject CreateAccessibilityInstance() => new PanelAccessible(this);

    // Build accessible reading order
    private List<AccessibleObject> Readable(AccessibleObject parent)
    {
        List<AccessibleObject> list = new List<AccessibleObject>();
        foreach (Part part in parts.Where(p => p.Speak.Length > 0))
        {
            list.Add(new PartAccessible(this, parent, part.Speak, part.Role, Rectangle.Round(part.Box)));
        }
        if (section?.Chart != null && !chartBox.IsEmpty)
        {
            PartAccessible chart = new PartAccessible(this, parent, section.Chart.Describe(), AccessibleRole.Chart, Rectangle.Round(chartBox));
            float rowHeight = RowHeight;
            float top = chartBox.Y + Face("Segoe UI Semibold", 10.5f).GetHeight() + Px(6);
            for (int i = 0; i < section.Chart.Bars.Count; i++)
            {
                ChartBar bar = section.Chart.Bars[i];
                string name = bar.Label + ": " + bar.Text + (bar.Highlight ? " (marked)" : "");
                chart.Children.Add(new PartAccessible(this, chart, name, AccessibleRole.ListItem,
                    Rectangle.Round(new RectangleF(chartBox.X, top + i * rowHeight, chartBox.Width, rowHeight))));
            }
            list.Add(chart);
        }
        foreach (Card card in cards)
        {
            list.Add(new PartAccessible(this, parent, $"{card.Star.Name}, {card.Line}", AccessibleRole.Graphic, Rectangle.Round(card.Box)));
        }
        if (picks != null && picks.Visible)
        {
            list.Add(picks.AccessibilityObject);
        }
        return list;
    }

    private sealed class PanelAccessible : ControlAccessibleObject
    {
        private readonly StoryPanel panel;

        // Set accessible panel owner
        public PanelAccessible(StoryPanel owner) : base(owner)
        {
            panel = owner;
        }

        // Count readable elements
        public override int GetChildCount() => panel.Readable(this).Count;

        // Read selected child
        public override AccessibleObject? GetChild(int index)
        {
            List<AccessibleObject> list = panel.Readable(this);
            return index >= 0 && index < list.Count ? list[index] : null;
        }
    }

    // One accessible content element
    private sealed class PartAccessible : AccessibleObject
    {
        private readonly Control owner;
        private readonly AccessibleObject parent;
        private readonly string name;
        private readonly AccessibleRole role;
        private readonly Rectangle box;
        public readonly List<AccessibleObject> Children = new List<AccessibleObject>();

        // Set readable element details
        public PartAccessible(Control owner, AccessibleObject parent, string name, AccessibleRole role, Rectangle box)
        {
            this.owner = owner;
            this.parent = parent;
            this.name = name;
            this.role = role;
            this.box = box;
        }

        public override string? Name => name;
        public override AccessibleRole Role => role;
        public override AccessibleStates State => AccessibleStates.ReadOnly;
        public override AccessibleObject? Parent => parent;
        public override Rectangle Bounds => owner.RectangleToScreen(box);
        // Count readable children
        public override int GetChildCount() => Children.Count;
        // Read selected child
        public override AccessibleObject? GetChild(int index) => index >= 0 && index < Children.Count ? Children[index] : null;
    }

    // Parts

    // One animated display element
    private sealed class Part
    {
        // Wrap full text
        public static readonly StringFormat Format = new StringFormat(StringFormat.GenericDefault) { Trimming = StringTrimming.None };
        // Center text vertically
        public static readonly StringFormat Middled = new StringFormat(Format) { LineAlignment = StringAlignment.Center };

        public readonly string Text;
        public readonly Font Font;
        public readonly Color Color;
        public RectangleF Box;
        public readonly int Delay;
        public readonly string Speak;
        public readonly AccessibleRole Role;
        public bool Badge;
        public bool Middle;

        // Set display element values
        public Part(string text, Font font, Color color, RectangleF box, int delay, string speak, AccessibleRole role)
        {
            Text = text;
            Font = font;
            Color = color;
            Box = box;
            Delay = delay;
            Speak = speak;
            Role = role;
        }
    }

    // One player card
    private sealed record Card(PlayerStar Star, string Line, RectangleF Box, float Photo, int Delay, Bitmap? Face);
}
