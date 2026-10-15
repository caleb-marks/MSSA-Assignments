using System.Drawing.Drawing2D;
using Caleb_Marks_OU_Season_Analyzer.Data;

namespace Caleb_Marks_OU_Season_Analyzer.Controls;

// Cached team logos
internal static class TeamLogo
{
    private static readonly Dictionary<string, Image> cache = new();

    // Load logo or draw initials
    public static Image Get(string teamId, string name, Color color)
    {
        if (cache.TryGetValue(teamId, out Image? saved))
        {
            return saved;
        }
        Image logo;
        try
        {
            // Copy pixels; release file
            using Image loaded = Image.FromFile(LogoPath(teamId));
            logo = new Bitmap(loaded);
        }
        catch (Exception error) when (error is FileNotFoundException || error is OutOfMemoryException || error is ArgumentException)
        {
            logo = Initials(name, color);
        }
        cache[teamId] = logo;
        return logo;
    }

    // Build logo file path
    private static string LogoPath(string teamId) => Path.Combine(GameCsv.DataFolder, "logos", teamId + ".png");

    // Release cached logos
    // Caller detaches displayed images first
    public static void Clear()
    {
        foreach (Image logo in cache.Values)
        {
            logo.Dispose();
        }
        cache.Clear();
    }

    // Draw initials fallback
    private static Image Initials(string name, Color color)
    {
        Bitmap bmp = new(160, 160);
        using Graphics g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using SolidBrush fill = new(color);
        g.FillEllipse(fill, 4, 4, 152, 152);
        string letters = new string(name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(word => word[0]).Take(3).ToArray());
        // Use pixel-sized lettering
        float size = 64f;
        Font font = new("Segoe UI Semibold", size, GraphicsUnit.Pixel);
        // Shrink overflowing initials
        while (size > 20f && TextRenderer.MeasureText(g, letters, font, Size.Empty, TextFormatFlags.NoPadding).Width > 112)
        {
            font.Dispose();
            size -= 4f;
            font = new Font("Segoe UI Semibold", size, GraphicsUnit.Pixel);
        }
        // Choose contrasting lettering
        Color text = color.GetBrightness() > 0.6f ? Color.FromArgb(0x2A, 0x28, 0x33) : Color.FromArgb(0xF3, 0xEF, 0xE8);
        TextFormatFlags center = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding;
        TextRenderer.DrawText(g, letters, font, new Rectangle(0, 0, 160, 160), text, center);
        font.Dispose();
        return bmp;
    }
}
