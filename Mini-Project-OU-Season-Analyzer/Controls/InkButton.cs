using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Caleb_Marks_OU_Season_Analyzer.Controls;

// Watercolor action button
// Primary actions use accent fill
internal class InkButton : Button
{
    // Pointer state
    protected bool hover;
    protected bool pressed;
    private bool primary;

    // Initialize painted button
    public InkButton()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
            | ControlStyles.SupportsTransparentBackColor, true);
        FlatStyle = FlatStyle.Flat;
        // Preserve transparent corners
        BackColor = Color.Transparent;
        Font = Palette.BodyBold;
        Cursor = Cursors.Hand;
        Height = 34;
    }

    // Primary action styling
    [Category("Appearance"), DefaultValue(false)]
    public bool Primary { get { return primary; } set { primary = value; Invalidate(); } }

    // Activate hover state
    protected override void OnMouseEnter(EventArgs e)
    {
        hover = true;
        Invalidate();
        base.OnMouseEnter(e);
    }

    // Clear pointer state
    protected override void OnMouseLeave(EventArgs e)
    {
        hover = false;
        pressed = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    // Activate pressed state
    protected override void OnMouseDown(MouseEventArgs e)
    {
        pressed = true;
        Invalidate();
        base.OnMouseDown(e);
    }

    // Clear pressed state
    protected override void OnMouseUp(MouseEventArgs e)
    {
        pressed = false;
        Invalidate();
        base.OnMouseUp(e);
    }

    // Repaint enabled state
    protected override void OnEnabledChanged(EventArgs e)
    {
        Invalidate();
        base.OnEnabledChanged(e);
    }

    // Select state colors
    private void StateColors(out Color fill, out Color text, out Color border)
    {
        if (Palette.HighContrast)
        {
            fill = SystemColors.ButtonFace;
            text = Enabled ? SystemColors.ControlText : SystemColors.GrayText;
            border = SystemColors.ControlText;
        }
        else if (!Enabled)
        {
            // Use disabled styling
            fill = Palette.CardFill;
            text = Palette.Muted;
            border = Palette.Rule;
        }
        else if (Primary)
        {
            fill = Palette.Darken(Palette.Accent, pressed ? 0.8f : hover ? 0.9f : 1f);
            text = Palette.OnAccent;
            border = Palette.Darken(fill, 0.75f);
        }
        else
        {
            fill = pressed ? Palette.Press : hover ? Palette.Hover : Palette.CardFill;
            text = Palette.Ink;
            border = Palette.Border;
        }
    }

    // Paint button
    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        StateColors(out Color fill, out Color text, out Color border);
        PaintParentUnder(e);
        PaintPatch(g, fill, border);

        // Show keyboard focus
        // Use system focus color when needed
        if (Focused && ShowFocusCues)
        {
            // Scale focus ring
            float px = DeviceDpi / 96f;
            using Pen ring = new(Primary && !Palette.HighContrast ? Palette.OnAccent : Palette.Focus, 2 * px);
            g.DrawRectangle(ring, 3 * px, 3 * px, Width - 7 * px, Height - 7 * px);
        }

        TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine;
        TextRenderer.DrawText(g, Text, Font, ClientRectangle, text, ShowKeyboardCues ? flags : flags | TextFormatFlags.HidePrefix);
    }

    // Paint parent background
    protected void PaintParentUnder(PaintEventArgs e)
    {
        if (Parent == null)
        {
            return;
        }
        GraphicsState state = e.Graphics.Save();
        e.Graphics.TranslateTransform(-Left, -Top);
        using (PaintEventArgs under = new(e.Graphics, new Rectangle(Left, Top, Width, Height)))
        {
            InvokePaintBackground(Parent, under);
            InvokePaint(Parent, under);
        }
        e.Graphics.Restore(state);
    }

    // Paint button shape
    // High contrast uses plain rectangle
    private void PaintPatch(Graphics g, Color fill, Color border)
    {
        using SolidBrush brush = new(fill);
        if (Palette.HighContrast)
        {
            g.FillRectangle(brush, ClientRectangle);
            using Pen pen = new(border);
            g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            return;
        }
        float px = DeviceDpi / 96f;
        RectangleF box = new(1.5f * px, 1.5f * px, Width - 3f * px, Height - 3f * px);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        Watercolor.WashFill(g, box, brush, border, Watercolor.Seed(Name.Length > 0 ? Name : Text), px);
        g.SmoothingMode = SmoothingMode.Default;
    }

    // Refresh keyboard cues
    protected override void OnChangeUICues(UICuesEventArgs e)
    {
        Invalidate();
        base.OnChangeUICues(e);
    }
}
