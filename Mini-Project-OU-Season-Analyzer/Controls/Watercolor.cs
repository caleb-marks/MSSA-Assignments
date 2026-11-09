using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Caleb_Marks_OU_Season_Analyzer.Data;

namespace Caleb_Marks_OU_Season_Analyzer.Controls;

// Watercolor drawing helpers
// Fixed seeds stabilize texture
internal static class Watercolor
{
    // Reduced bloom resolution
    private const int BloomScale = 4;
    // Text-band fade distance
    private const int Feather = 36;
    // Relative bloom positions
    private static readonly PointF[] BloomSpots =
        new PointF[] { new PointF(0.07f, 0.45f), new PointF(0.93f, 0.32f), new PointF(0.52f, 0.66f), new PointF(0.22f, 0.90f),
            new PointF(0.84f, 0.88f), new PointF(0.38f, 0.30f) };
    private static readonly Dictionary<Form, PaperSheet> papers = new Dictionary<Form, PaperSheet>();
    private static readonly Dictionary<string, GraphicsPath> paths = new Dictionary<string, GraphicsPath>();
    private static TextureBrush? grain;

    // Calculate repeatable text seed
    public static int Seed(string text)
    {
        int seed = 17;
        foreach (char c in text)
        {
            seed = unchecked(seed * 31 + c);
        }
        return seed;
    }

    // Repeatable team texture seed
    private static int TeamSeed => Seed(TeamStore.Current.Id);

    // Clear drawing caches
    public static void Clear()
    {
        foreach (PaperSheet sheet in papers.Values)
        {
            sheet.Dispose();
        }
        papers.Clear();
        paths.Clear();
        if (grain != null)
        {
            grain.Dispose();
            grain = null;
        }
    }

    // Paint shared paper region
    public static void PaintPage(Graphics g, Control control, Rectangle area)
    {
        Form? form = control.FindForm();
        if (Palette.HighContrast || form == null || !control.IsHandleCreated || form.ClientSize.Width < 1 || form.ClientSize.Height < 1)
        {
            g.Clear(Palette.Page);
            return;
        }
        PaperSheet sheet = SheetFor(form);
        Point at = form.PointToClient(control.PointToScreen(Point.Empty));
        Rectangle clip = Rectangle.Intersect(Rectangle.Intersect(area, Rectangle.Ceiling(g.VisibleClipBounds)), control.ClientRectangle);
        if (clip.Width > 0 && clip.Height > 0)
        {
            sheet.Blit(g, clip, new Point(clip.X + at.X, clip.Y + at.Y));
        }
    }

    // Reuse matching paper sheet
    private static PaperSheet SheetFor(Form form)
    {
        IPaperForm? host = form as IPaperForm;
        Rectangle[] quiet = host != null ? host.QuietBands() : new Rectangle[0];
        string key = $"{form.ClientSize.Width}x{form.ClientSize.Height}@{form.DeviceDpi}|{string.Join(";", quiet)}";
        if (papers.TryGetValue(form, out PaperSheet? cached) && cached.Key == key)
        {
            return cached;
        }
        if (cached == null)
        {
            form.Disposed += (sender, e) => Forget(form);
        }
        else
        {
            cached.Dispose();
            form.BeginInvoke(() => form.Invalidate(true));
        }
        using Bitmap paper = Paper(form.ClientSize, quiet, form.DeviceDpi / 96f);
        PaperSheet sheet = new PaperSheet(key, paper);
        papers[form] = sheet;
        return sheet;
    }

    // Calculate form-relative bounds
    public static Rectangle BoundsIn(Form form, Control control)
    {
        Point at = control.Location;
        for (Control? parent = control.Parent; parent != null && parent != form; parent = parent.Parent)
        {
            at.Offset(parent.Location);
        }
        return new Rectangle(at, control.Size);
    }

    // Release form's paper sheet
    private static void Forget(Form form)
    {
        if (papers.Remove(form, out PaperSheet? cached))
        {
            cached.Dispose();
        }
    }

    // Create paper bitmap; caller disposes
    public static Bitmap Paper(Size size, Rectangle[] quiet, float scale)
    {
        Bitmap paper = new Bitmap(Math.Max(1, size.Width), Math.Max(1, size.Height), PixelFormat.Format32bppPArgb);
        using Graphics g = Graphics.FromImage(paper);
        g.Clear(Palette.Page);
        using (Bitmap blooms = BloomLayer(paper.Size, quiet, scale))
        using (ImageAttributes edges = new ImageAttributes())
        {
            // Mirror stretched edges
            edges.SetWrapMode(WrapMode.TileFlipXY);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            Rectangle whole = new Rectangle(0, 0, (blooms.Width - 1) * BloomScale, (blooms.Height - 1) * BloomScale);
            g.DrawImage(blooms, whole, 0, 0, blooms.Width - 1, blooms.Height - 1, GraphicsUnit.Pixel, edges);
        }
        g.PixelOffsetMode = PixelOffsetMode.Default;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        Fibers(g, paper.Size, scale);
        g.FillRectangle(Grain(), 0, 0, paper.Width, paper.Height);
        return paper;
    }

    // Build reduced bloom layer
    private static Bitmap BloomLayer(Size size, Rectangle[] quiet, float scale)
    {
        Bitmap layer = new Bitmap(size.Width / BloomScale + 2, size.Height / BloomScale + 2, PixelFormat.Format32bppPArgb);
        using (Graphics g = Graphics.FromImage(layer))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Random random = new Random(TeamSeed);
            Color[] colors = Palette.BloomColors;
            int count = 4 + random.Next(3);
            // Alternate bloom colors
            for (int i = 0; i < count; i++)
            {
                Bloom(g, random, BloomSpots[i], layer.Size, colors[i % 2]);
            }
        }
        Quiet(layer, quiet, scale);
        return layer;
    }

    // Draw irregular color bloom
    private static void Bloom(Graphics g, Random random, PointF spot, Size size, Color color)
    {
        float radius = Math.Min(size.Width, size.Height) * (0.26f + 0.14f * (float)random.NextDouble());
        PointF center = new PointF((spot.X + Jitter(random, 0.05f)) * size.Width, (spot.Y + Jitter(random, 0.05f)) * size.Height);
        float alpha = 0.16f + 0.06f * (float)random.NextDouble();
        using GraphicsPath shape = new GraphicsPath();
        shape.AddClosedCurve(Blob(random, center, radius, 0.22f, 11), 0.5f);
        using (PathGradientBrush fill = new PathGradientBrush(shape))
        {
            fill.CenterPoint = center;
            fill.CenterColor = Color.FromArgb((int)(255 * alpha), color);
            fill.SurroundColors = new Color[] { Color.FromArgb(0, color) };
            fill.FocusScales = new PointF(0.45f, 0.4f);
            g.FillPath(fill, shape);
        }
        using Pen ring = new Pen(Color.FromArgb((int)(255 * alpha * 0.3f), color), Math.Max(1f, radius * 0.03f));
        g.DrawPath(ring, shape);
    }

    // Generate symmetric random offset
    private static float Jitter(Random random, float spread) => (float)(random.NextDouble() * 2 - 1) * spread;

    // Build irregular circular outline
    private static PointF[] Blob(Random random, PointF center, float radius, float wobble, int count)
    {
        PointF[] points = new PointF[count];
        double turn = random.NextDouble() * Math.Tau;
        // Generate angular points
        for (int k = 0; k < count; k++)
        {
            double angle = turn + Math.Tau * k / count + Jitter(random, 0.25f) / count;
            float r = radius * (1 + Jitter(random, wobble));
            points[k] = new PointF(center.X + r * (float)Math.Cos(angle), center.Y + r * (float)Math.Sin(angle));
        }
        return points;
    }

    // Reduce bloom opacity near text
    private static void Quiet(Bitmap layer, Rectangle[] quiet, float scale)
    {
        if (quiet.Length == 0)
        {
            return;
        }
        Rectangle[] bands = quiet.Select(b => Rectangle.FromLTRB(b.Left / BloomScale - 1, b.Top / BloomScale - 1,
            b.Right / BloomScale + 2, b.Bottom / BloomScale + 2)).ToArray();
        int cap = SafeLayerAlpha();
        float feather = Math.Max(1f, Feather * scale / BloomScale);
        BitmapData data = layer.LockBits(new Rectangle(Point.Empty, layer.Size), ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
        byte[] pixels = new byte[data.Stride * layer.Height];
        Marshal.Copy(data.Scan0, pixels, 0, pixels.Length);
        for (int y = 0; y < layer.Height; y++)
        {
            for (int x = 0; x < layer.Width; x++)
            {
                Limit(pixels, y * data.Stride + x * 4, Allowed(bands, x, y, cap, feather));
            }
        }
        Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);
        layer.UnlockBits(data);
    }

    // Calculate text-safe layer opacity
    private static int SafeLayerAlpha()
    {
        float a = Palette.BloomAlpha(0);
        float b = Palette.BloomAlpha(1);
        return (int)(255 * Math.Min(1 - (1 - a) * (1 - 0.3f * a), 1 - (1 - b) * (1 - 0.3f * b)));
    }

    // Calculate permitted pixel opacity
    private static int Allowed(Rectangle[] bands, int x, int y, int cap, float feather)
    {
        float allowed = 255f;
        foreach (Rectangle b in bands)
        {
            int dx = Math.Max(Math.Max(b.Left - x, x - (b.Right - 1)), 0);
            int dy = Math.Max(Math.Max(b.Top - y, y - (b.Bottom - 1)), 0);
            allowed = Math.Min(allowed, cap + (255 - cap) * Math.Min(1f, Math.Max(dx, dy) / feather));
        }
        return (int)allowed;
    }

    // Scale premultiplied pixel channels
    private static void Limit(byte[] pixels, int i, int allowed)
    {
        int alpha = pixels[i + 3];
        if (alpha <= allowed)
        {
            return;
        }
        float factor = allowed / (float)alpha;
        for (int k = 0; k < 4; k++)
        {
            pixels[i + k] = (byte)(pixels[i + k] * factor);
        }
    }

    // Draw light paper fibers
    private static void Fibers(Graphics g, Size size, float scale)
    {
        Random random = new Random(TeamSeed * 17 + 3);
        int count = (int)(size.Width * size.Height / (2600f * scale * scale));
        using Pen light = new Pen(Color.FromArgb(34, Color.White), Math.Max(1f, scale));
        // Draw short fiber curves
        for (int i = 0; i < count; i++)
        {
            PointF start = new PointF((float)random.NextDouble() * size.Width, (float)random.NextDouble() * size.Height);
            double angle = random.NextDouble() * Math.Tau;
            float length = (5 + 18 * (float)random.NextDouble()) * scale;
            PointF bend = new PointF(start.X + length * 0.5f * (float)Math.Cos(angle + 0.4),
                start.Y + length * 0.5f * (float)Math.Sin(angle + 0.4));
            PointF end = new PointF(start.X + length * (float)Math.Cos(angle), start.Y + length * (float)Math.Sin(angle));
            g.DrawCurve(light, new PointF[] { start, bend, end });
        }
    }

    // Build cached grain texture
    private static TextureBrush Grain()
    {
        if (grain != null)
        {
            return grain;
        }
        using Bitmap tile = new Bitmap(256, 256, PixelFormat.Format32bppPArgb);
        Random random = new Random(TeamSeed * 31 + 7);
        Color dark = Color.FromArgb(7, Palette.Ink);
        Color light = Color.FromArgb(20, Color.White);
        // Scatter texture pixels
        for (int i = 0; i < 256 * 256 / 12; i++)
        {
            tile.SetPixel(random.Next(256), random.Next(256), i % 3 == 0 ? light : dark);
        }
        grain = new TextureBrush(tile);
        return grain;
    }

    // Build cached card outline
    public static GraphicsPath CardPath(RectangleF box, float radius, int seed, float px)
    {
        string key = FormattableString.Invariant($"{box.X:0.#},{box.Y:0.#},{box.Width:0.#},{box.Height:0.#},{radius:0.#},{seed},{px:0.##}");
        if (paths.TryGetValue(key, out GraphicsPath? cached))
        {
            return cached;
        }
        // Limit cached path count
        if (paths.Count > 400)
        {
            paths.Clear();
        }
        using GraphicsPath plain = Rounded(box, radius);
        plain.Flatten(null, 0.2f);
        PointF[] outline = Resample(plain.PathPoints, 7f * px);
        Wobble(outline, new Random(seed), 1.5f * px);
        GraphicsPath path = new GraphicsPath();
        path.AddClosedCurve(outline, 0.4f);
        paths[key] = path;
        return path;
    }

    // Create tile outline; caller disposes
    public static GraphicsPath TileShape(RectangleF box, float radius, int seed, float px)
    {
        if (Palette.HighContrast)
        {
            return Rounded(box, radius);
        }
        RectangleF inset = RectangleF.Inflate(box, -1.5f * px, -1.5f * px);
        return (GraphicsPath)CardPath(inset, radius, seed, px).Clone();
    }

    // Resample closed outline
    private static PointF[] Resample(PointF[] points, float step)
    {
        List<PointF> result = new List<PointF>();
        result.Add(points[0]);
        float next = step;
        float walked = 0f;
        for (int i = 0; i < points.Length; i++)
        {
            PointF a = points[i];
            PointF b = points[(i + 1) % points.Length];
            float length = MathF.Sqrt((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y));
            while (length > 0f && next <= walked + length)
            {
                float t = (next - walked) / length;
                result.Add(new PointF(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t));
                next += step;
            }
            walked += length;
        }
        // Remove crowded closing point
        if (result.Count > 3 && walked - (next - step) < step * 0.5f)
        {
            result.RemoveAt(result.Count - 1);
        }
        return result.ToArray();
    }

    // Perturb outline using smooth waves
    private static void Wobble(PointF[] points, Random random, float amplitude)
    {
        int n = points.Length;
        double slow = Math.Tau * (3 + random.Next(4)) / n;
        double quick = Math.Tau * (11 + random.Next(7)) / n;
        double phase1 = random.NextDouble() * Math.Tau;
        double phase2 = random.NextDouble() * Math.Tau;
        PointF[] source = points.ToArray();
        // Offset each outline point
        for (int k = 0; k < n; k++)
        {
            PointF before = source[(k - 1 + n) % n];
            PointF after = source[(k + 1) % n];
            float dx = after.X - before.X;
            float dy = after.Y - before.Y;
            float length = MathF.Max(0.001f, MathF.Sqrt(dx * dx + dy * dy));
            float push = amplitude * (float)(0.65 * Math.Sin(k * slow + phase1) + 0.35 * Math.Sin(k * quick + phase2));
            points[k] = new PointF(source[k].X + dy / length * push, source[k].Y - dx / length * push);
        }
    }

    // Paint fading card wash
    public static void CardWash(Graphics g, GraphicsPath card, Color color, float px)
    {
        RectangleF box = card.GetBounds();
        RectangleF top = new RectangleF(box.X, box.Y - 1, box.Width, box.Height * 0.4f + 1);
        using (LinearGradientBrush wash = new LinearGradientBrush(top, Color.FromArgb(36, color), Color.FromArgb(0, color), LinearGradientMode.Vertical))
        {
            // Mirror gradient edges
            wash.WrapMode = WrapMode.TileFlipXY;
            GraphicsState state = g.Save();
            g.SetClip(card, CombineMode.Intersect);
            g.FillRectangle(wash, top.X, top.Y + 1, top.Width, top.Height - 1);
            g.Restore(state);
        }
        PigmentEdge(g, card, color, 64, px);
    }

    // Draw pigment outline
    public static void PigmentEdge(Graphics g, GraphicsPath shape, Color color, int alpha, float px)
    {
        using Pen pigment = new Pen(Color.FromArgb(alpha, Palette.Darken(color, 0.75f)), Math.Max(1f, 1.2f * px));
        g.DrawPath(pigment, shape);
    }

    // Paint irregular patch
    public static void WashFill(Graphics g, RectangleF box, Brush fill, Color edge, int seed, float px)
    {
        if (box.Width < 1f || box.Height < 1f)
        {
            return;
        }
        using GraphicsPath shape = PatchPath(box, seed, px);
        g.FillPath(fill, shape);
        using Pen pen = new Pen(edge, Math.Max(1f, px));
        g.DrawPath(pen, shape);
    }

    // Build patch outline
    public static GraphicsPath PatchPath(RectangleF box, int seed, float px)
    {
        // Use plain high-contrast rectangle
        if (Palette.HighContrast)
        {
            GraphicsPath plain = new GraphicsPath();
            plain.AddRectangle(box);
            return plain;
        }
        bool wide = box.Width >= box.Height;
        float jx = Math.Min((wide ? 1.5f : 0.6f) * px, box.Width / 6);
        float jy = Math.Min((wide ? 0.6f : 1.5f) * px, box.Height / 6);
        GraphicsPath path = new GraphicsPath();
        path.AddClosedCurve(PatchPoints(new Random(seed), box, jx, jy), 0.2f);
        return path;
    }

    // Generate eight patch points
    private static PointF[] PatchPoints(Random random, RectangleF box, float jx, float jy)
    {
        float midX = box.X + box.Width / 2;
        float midY = box.Y + box.Height / 2;
        float[] xs = { box.Left, midX, box.Right, box.Right, box.Right, midX, box.Left, box.Left };
        float[] ys = { box.Top, box.Top, box.Top, midY, box.Bottom, box.Bottom, box.Bottom, midY };
        PointF[] points = new PointF[8];
        for (int i = 0; i < 8; i++)
        {
            points[i] = new PointF(xs[i] + Jitter(random, jx), ys[i] + Jitter(random, jy));
        }
        return points;
    }

    // Draw irregular dot
    public static void PaintedDot(Graphics g, RectangleF circle, Color color, int seed, float px)
    {
        PointF center = new PointF(circle.X + circle.Width / 2, circle.Y + circle.Height / 2);
        using GraphicsPath shape = new GraphicsPath();
        shape.AddClosedCurve(Blob(new Random(seed), center, circle.Width / 2, 0.05f, 10), 0.5f);
        using (SolidBrush fill = new SolidBrush(color))
        {
            g.FillPath(fill, shape);
        }
        PigmentEdge(g, shape, color, 150, px);
    }

    // Paint tapered brush stroke
    public static void BrushStroke(Graphics g, PointF from, PointF to, float width, Color color, int seed)
    {
        using SolidBrush paint = new SolidBrush(color);
        g.FillPath(paint, StrokePath(from, to, width, seed));
    }

    // Build cached stroke outline
    public static GraphicsPath StrokePath(PointF from, PointF to, float width, int seed)
    {
        string key = FormattableString.Invariant($"stroke {from.X:0.#},{from.Y:0.#},{to.X:0.#},{to.Y:0.#},{width:0.#},{seed}");
        if (paths.TryGetValue(key, out GraphicsPath? cached))
        {
            return cached;
        }
        float dx = to.X - from.X;
        float dy = to.Y - from.Y;
        float length = MathF.Max(0.001f, MathF.Sqrt(dx * dx + dy * dy));
        int steps = Math.Clamp((int)(length / Math.Max(1f, width * 1.5f)), 12, 160);
        GraphicsPath shape = new GraphicsPath();
        shape.AddClosedCurve(StrokeEdge(new Random(seed), from, dx, dy, length, width, steps), 0.3f);
        paths[key] = shape;
        return shape;
    }

    // Build tapered stroke edges
    private static PointF[] StrokeEdge(Random random, PointF from, float dx, float dy, float length, float width, int steps)
    {
        PointF normal = new PointF(-dy / length, dx / length);
        double phase = random.NextDouble() * Math.Tau;
        double cycles = Math.Max(1.3, length / (width * 30));
        PointF[] edge = new PointF[steps * 2 + 2];
        // Generate paired edge points
        for (int i = 0; i <= steps; i++)
        {
            float t = i / (float)steps;
            float taper = MathF.Pow(MathF.Sin(MathF.PI * (0.04f + 0.92f * t)), 0.5f);
            float half = width / 2 * taper * (0.85f + 0.3f * (float)random.NextDouble());
            float wave = width * 0.22f * (float)Math.Sin(t * Math.Tau * cycles + phase);
            PointF mid = new PointF(from.X + dx * t + normal.X * wave, from.Y + dy * t + normal.Y * wave);
            edge[i] = new PointF(mid.X + normal.X * half, mid.Y + normal.Y * half);
            edge[steps * 2 + 1 - i] = new PointF(mid.X - normal.X * half, mid.Y - normal.Y * half);
        }
        return edge;
    }

    // Draw pencil-style line
    public static void PencilLine(Graphics g, PointF from, PointF to, Color color, float px)
    {
        if (Palette.HighContrast)
        {
            using Pen solid = new Pen(color, Math.Max(1f, px));
            g.DrawLine(solid, from, to);
            return;
        }
        using Pen main = new Pen(Color.FromArgb(150, color), Math.Max(1f, 1.1f * px)) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        using Pen twin = new Pen(Color.FromArgb(60, color), Math.Max(1f, 0.8f * px));
        g.DrawLine(main, from, to);
        g.DrawLine(twin, from.X + px, from.Y + 0.6f * px, to.X - px, to.Y + 0.4f * px);
    }

    // Create rounded path; caller disposes
    private static GraphicsPath Rounded(RectangleF box, float radius)
    {
        GraphicsPath path = new GraphicsPath();
        float d = Math.Min(radius * 2, Math.Min(box.Width, box.Height));
        if (d <= 0)
        {
            path.AddRectangle(box);
            return path;
        }
        path.AddArc(box.X, box.Y, d, d, 180, 90);
        path.AddArc(box.Right - d, box.Y, d, d, 270, 90);
        path.AddArc(box.Right - d, box.Bottom - d, d, d, 0, 90);
        path.AddArc(box.X, box.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
