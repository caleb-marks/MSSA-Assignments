using System.Runtime.InteropServices;

namespace Caleb_Marks_OU_Season_Analyzer.Controls;

// Cached GDI paper bitmap
internal sealed class PaperSheet : IDisposable
{
    private const int SourceCopy = 0x00CC0020;
    private readonly IntPtr bits;
    private readonly IntPtr dc;
    private readonly IntPtr oldBits;

    // Create memory drawing context
    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateCompatibleDC(IntPtr hdc);
    // Select drawing object
    [DllImport("gdi32.dll")]
    private static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);
    // Release drawing context
    [DllImport("gdi32.dll")]
    private static extern bool DeleteDC(IntPtr hdc);
    // Release drawing object
    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr obj);
    // Copy bitmap pixels
    [DllImport("gdi32.dll")]
    private static extern bool BitBlt(IntPtr dest, int x, int y, int width, int height, IntPtr source, int sourceX, int sourceY, int rop);

    // Create bitmap drawing context
    public PaperSheet(string key, Bitmap paper)
    {
        Key = key;
        bits = paper.GetHbitmap();
        dc = CreateCompatibleDC(IntPtr.Zero);
        oldBits = SelectObject(dc, bits);
    }

    // Paper cache key
    public string Key { get; }

    // Copy paper region
    public void Blit(Graphics g, Rectangle dest, Point source)
    {
        using System.Drawing.Drawing2D.Matrix move = g.Transform;
        int dx = (int)move.OffsetX;
        int dy = (int)move.OffsetY;
        IntPtr hdc = g.GetHdc();
        try
        {
            BitBlt(hdc, dest.X + dx, dest.Y + dy, dest.Width, dest.Height, dc, source.X, source.Y, SourceCopy);
        }
        finally
        {
            g.ReleaseHdc(hdc);
        }
    }

    // Release GDI resources
    public void Dispose()
    {
        SelectObject(dc, oldBits);
        DeleteDC(dc);
        DeleteObject(bits);
    }
}
