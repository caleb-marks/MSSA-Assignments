using System.Drawing.Drawing2D;
using Caleb_Marks_OU_Season_Analyzer.Models;

namespace Caleb_Marks_OU_Season_Analyzer.Controls;

// Scalable verdict badges
// Check or flame identifies outcome
// Shapes distinguish outcomes
internal static class Badge
{
    public static readonly Color Crimson = Color.FromArgb(0x84, 0x16, 0x17);
    public static readonly Color Cream = Color.FromArgb(0xFD, 0xF9, 0xD8);

    // Select verdict label
    public static string Word(Finish finish) => finish == Finish.LockedIn ? "Locked in" : "Cooked";

    // Describe playoff status
    public static string Subline(Finish finish) => finish == Finish.LockedIn ? "still in the playoff race" : "out of the playoff race";

    // Draw centered badge
    // C8: draw verdict shapes
    public static void Draw(Graphics g, Finish finish, RectangleF box)
    {
        float size = Math.Min(box.Width, box.Height);
        if (size < 4f)
        {
            return;
        }
        RectangleF s = new RectangleF(box.X + (box.Width - size) / 2, box.Y + (box.Height - size) / 2, size, size);
        SmoothingMode old = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        if (finish == Finish.LockedIn)
        {
            LockedIn(g, s);
        }
        else
        {
            Cooked(g, s);
        }
        g.SmoothingMode = old;
    }

    // Convert proportional coordinates
    private static PointF P(RectangleF s, float fx, float fy) => new PointF(s.X + s.Width * fx, s.Y + s.Height * fy);

    // Draw check badge
    private static void LockedIn(Graphics g, RectangleF s)
    {
        using SolidBrush crimson = new SolidBrush(Crimson);
        g.FillEllipse(crimson, s);
        float ring = s.Width * 0.035f;
        if (s.Width >= 48)
        {
            using Pen inner = new Pen(Cream, ring);
            RectangleF r = RectangleF.Inflate(s, -s.Width * 0.08f, -s.Width * 0.08f);
            g.DrawEllipse(inner, r);
        }
        using Pen check = new Pen(Cream, s.Width * 0.13f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        g.DrawLines(check, new PointF[] { P(s, 0.28f, 0.53f), P(s, 0.44f, 0.69f), P(s, 0.74f, 0.35f) });
    }

    // Draw flame badge
    private static void Cooked(Graphics g, RectangleF s)
    {
        using SolidBrush cream = new SolidBrush(Cream);
        using SolidBrush crimson = new SolidBrush(Crimson);
        float ringWidth = s.Width * 0.09f;
        RectangleF inner = RectangleF.Inflate(s, -ringWidth / 2, -ringWidth / 2);
        g.FillEllipse(cream, inner);
        using Pen ring = new Pen(Crimson, ringWidth);
        g.DrawEllipse(ring, inner);
        using GraphicsPath flame = Shape(s, new float[]
        {
            0.50f, 0.86f, 0.31f, 0.86f, 0.22f, 0.69f, 0.28f, 0.54f, 0.33f, 0.42f, 0.43f, 0.36f, 0.43f, 0.15f,
            0.58f, 0.27f, 0.76f, 0.40f, 0.76f, 0.61f, 0.76f, 0.77f, 0.65f, 0.86f, 0.50f, 0.86f,
        });
        g.FillPath(crimson, flame);
        using GraphicsPath heart = Shape(s, new float[]
        {
            0.50f, 0.81f, 0.41f, 0.81f, 0.38f, 0.73f, 0.42f, 0.66f, 0.46f, 0.59f, 0.50f, 0.56f, 0.51f, 0.49f,
            0.57f, 0.58f, 0.63f, 0.64f, 0.61f, 0.73f, 0.60f, 0.79f, 0.55f, 0.81f, 0.50f, 0.81f,
        });
        g.FillPath(cream, heart);
    }

    // Build closed curved shape
    private static GraphicsPath Shape(RectangleF s, float[] xy)
    {
        PointF[] points = new PointF[xy.Length / 2];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = P(s, xy[i * 2], xy[i * 2 + 1]);
        }
        GraphicsPath path = new GraphicsPath();
        path.AddBeziers(points);
        path.CloseFigure();
        return path;
    }
}
