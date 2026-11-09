using System.Globalization;
using Microsoft.Win32;

namespace Caleb_Marks_OU_Season_Analyzer.Controls;

// Shared team palette
// Losses use distinct patterns
internal static class Palette
{
    // Contrast thresholds
    private const double MinContrast = 4.5;
    private const double MarkContrast = 3.0;

    // Base paper & ink colors
    private static readonly Color PaperBase = Color.FromArgb(0xFB, 0xF7, 0xEE);
    private static readonly Color CardBase = Color.FromArgb(0xFF, 0xFD, 0xF8);
    private static readonly Color InkBase = Color.FromArgb(0x2B, 0x24, 0x20);
    private static readonly Color InkSoftBase = Color.FromArgb(0x5E, 0x53, 0x4A);

    private static bool? highContrast;
    // Team color sources
    private static Color teamColor;
    private static Color altColor;
    private static Color tint;
    // Tinted surfaces
    private static Color page;
    private static Color card;
    private static Color back;
    private static Color badge;
    private static Color hover;
    private static Color press;
    private static Color rule;
    // Readable drawing colors
    private static Color accent;
    private static Color border;
    private static Color chartEdge;
    private static Color muted;
    private static Color slate;
    private static Color neutral;
    private static Color loss;
    private static int stripeAlpha = 115;
    private static float[] bloomAlphas = new float[] { 0f, 0f };

    // Initialize palette settings
    static Palette()
    {
        SystemEvents.UserPreferenceChanged += (sender, e) =>
        {
            highContrast = null;
            Watercolor.Clear();
        };
        UseTeam(Color.FromArgb(0x99, 0x00, 0x00), Color.White);
    }

    // Cache Windows contrast setting
    public static bool HighContrast
    {
        get
        {
            if (highContrast == null)
            {
                highContrast = SystemInformation.HighContrast;
            }
            return highContrast.Value;
        }
    }

    public static Color Page => HighContrast ? SystemColors.Window : page;
    public static Color CardFill => HighContrast ? SystemColors.Window : card;
    // Reverse-side surface
    public static Color BackFill => HighContrast ? SystemColors.Window : back;
    // Decorative surfaces
    public static Color Badge => HighContrast ? SystemColors.Window : badge;
    public static Color Track => HighContrast ? SystemColors.Window : badge;
    public static Color Backdrop => HighContrast ? SystemColors.Window : badge;
    // Interaction surfaces
    public static Color Hover => HighContrast ? SystemColors.ButtonFace : hover;
    public static Color Press => HighContrast ? SystemColors.ButtonFace : press;
    public static Color Ink => HighContrast ? SystemColors.WindowText : InkBase;
    public static Color InkSoft => HighContrast ? SystemColors.WindowText : InkSoftBase;
    // Decorative borders
    public static Color Rule => HighContrast ? SystemColors.WindowText : rule;
    // Control borders
    public static Color Border => HighContrast ? SystemColors.ControlText : border;
    // Chart outlines
    public static Color ChartEdge => HighContrast ? SystemColors.WindowText : chartEdge;
    // Action accent
    public static Color Accent => HighContrast ? SystemColors.Highlight : accent;
    public static Color OnAccent => HighContrast ? SystemColors.HighlightText : Color.White;
    // Decorative team colors
    public static Color TeamColor => teamColor;
    public static Color AltColor => altColor;
    // Surface tint source
    public static Color WashColor => tint;
    // Result colors
    public static Color Win => HighContrast ? SystemColors.Highlight : accent;
    public static Color Loss => HighContrast ? SystemColors.Window : loss;
    public static Color LossInk => HighContrast ? SystemColors.WindowText : InkBase;
    // Loss stripe color
    public static Color LossStripe => HighContrast ? SystemColors.WindowText : Color.FromArgb(stripeAlpha, InkBase);
    // Muted drawing color
    public static Color Slate => HighContrast ? SystemColors.WindowText : slate;
    // Desaturated text color
    public static Color Neutral => HighContrast ? SystemColors.WindowText : neutral;
    public static Color Muted => HighContrast ? SystemColors.GrayText : muted;
    public static Color Focus => HighContrast ? SystemColors.Highlight : accent;
    public static Color Link => HighContrast ? SystemColors.HotTrack : accent;
    // Select bloom colors
    public static Color[] BloomColors =>
        new Color[] { teamColor, Contrast(altColor, PaperBase) >= 1.25 || altColor.GetSaturation() >= 0.5f ? altColor : teamColor };

    // Maximum text-safe bloom strength
    public static float BloomAlpha(int index) => bloomAlphas[index];

    // Apply team colors
    public static void UseTeam(Color primary, Color alt)
    {
        teamColor = primary;
        altColor = alt;
        tint = Contrast(primary, PaperBase) >= 1.4 || Contrast(alt, PaperBase) < 1.4 ? primary : alt;
        DeriveSurfaces();
        DeriveMarks();
        DeriveResults();
        bloomAlphas = BloomColors.Select(c => SafeBloomAlpha(c)).ToArray();
        Color[] pages = BloomColors.Select((c, i) => DarkestPage(c, bloomAlphas[i])).Append(page).ToArray();
        chartEdge = DarkenUntil(border, MarkContrast, pages);
        Watercolor.Clear();
    }

    // Mix surface colors
    private static void DeriveSurfaces()
    {
        page = Motion.Mix(PaperBase, tint, 0.07f);
        card = Motion.Mix(CardBase, tint, 0.03f);
        back = Motion.Mix(CardBase, tint, 0.08f);
        badge = Motion.Mix(CardBase, tint, 0.14f);
        hover = Motion.Mix(CardBase, tint, 0.10f);
        press = Motion.Mix(CardBase, tint, 0.20f);
        rule = Motion.Mix(CardBase, tint, 0.30f);
    }

    // Derive readable colors
    private static void DeriveMarks()
    {
        // Collect text backgrounds
        Color[] textBacks = new Color[] { Color.White, card, back, badge, Motion.Mix(card, tint, 0.14f), page,
            Over(page, InkBase, 0.03f), press };
        accent = ReadableAccent(teamColor, altColor, textBacks);
        border = DarkenUntil(Motion.Mix(CardBase, tint, 0.45f), MarkContrast, card, back, hover, press);
        muted = DarkenUntil(Motion.Mix(Color.FromArgb(0x9A, 0x92, 0x88), tint, 0.25f), MarkContrast, card, back, badge, page);
        slate = DarkenUntil(Desaturate(Motion.Mix(tint, CardBase, 0.2f), 0.6f), 5.0, card, back, page);
        neutral = DarkenUntil(Desaturate(tint, 0.25f), MinContrast, textBacks);
    }

    // Separate result styles
    private static void DeriveResults()
    {
        float share = 0.22f;
        loss = Motion.Mix(card, tint, share);
        while (Contrast(accent, loss) < MarkContrast && share > 0f)
        {
            share = Math.Max(0f, share - 0.02f);
            loss = Motion.Mix(card, tint, share);
        }
        stripeAlpha = 115;
        while (Contrast(Over(loss, InkBase, stripeAlpha / 255f), card) < MarkContrast && stripeAlpha < 255)
        {
            stripeAlpha = Math.Min(255, stripeAlpha + 10);
        }
    }

    // Find text-safe bloom strength
    private static float SafeBloomAlpha(Color bloom)
    {
        float alpha = 0.22f;
        while (alpha > 0f && !TextReads(DarkestPage(bloom, alpha)))
        {
            alpha = Math.Max(0f, alpha - 0.01f);
        }
        return alpha;
    }

    // Estimate darkest paper pixel
    public static Color DarkestPage(Color bloom, float alpha)
    {
        Color center = Over(Over(page, bloom, alpha), bloom, alpha * 0.3f);
        return Over(center, InkBase, 0.03f);
    }

    // Check text contrast
    public static bool TextReads(Color background)
    {
        return new Color[] { InkBase, InkSoftBase, accent, neutral }.All(c => Contrast(c, background) >= MinContrast);
    }

    // Blend translucent color
    public static Color Over(Color background, Color color, float share) => Motion.Mix(background, color, share);

    // Reduce saturation
    private static Color Desaturate(Color color, float keep)
    {
        int gray = (int)(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
        return Motion.Mix(Color.FromArgb(gray, gray, gray), color, keep);
    }

    // Darken toward target contrast
    private static Color DarkenUntil(Color color, double target, params Color[] backs)
    {
        while (backs.Any(b => Contrast(color, b) < target) && color.R + color.G + color.B > 0)
        {
            color = Darken(color, 0.9f);
        }
        return color;
    }

    // Calculate contrast ratio
    public static double Contrast(Color a, Color b)
    {
        double lighter = Math.Max(Luminance(a), Luminance(b));
        double darker = Math.Min(Luminance(a), Luminance(b));
        return (lighter + 0.05) / (darker + 0.05);
    }

    // Calculate relative luminance
    private static double Luminance(Color color)
    {
        return 0.2126 * Linear(color.R) + 0.7152 * Linear(color.G) + 0.0722 * Linear(color.B);
    }

    // Convert sRGB channel
    private static double Linear(int channel)
    {
        double c = channel / 255.0;
        return c <= 0.04045 ? c / 12.92 : Math.Pow((c + 0.055) / 1.055, 2.4);
    }

    // Select white-compatible accent
    public static Color ReadableAccent(Color primary, Color alt) => ReadableAccent(primary, alt, new Color[] { Color.White });

    // Select background-compatible accent
    private static Color ReadableAccent(Color primary, Color alt, Color[] backs)
    {
        if (backs.All(b => Contrast(primary, b) >= MinContrast))
        {
            return primary;
        }
        if (backs.All(b => Contrast(alt, b) >= MinContrast))
        {
            return alt;
        }
        return DarkenUntil(primary, MinContrast, backs);
    }

    public static readonly Font Body = new Font("Segoe UI", 10f);
    public static readonly Font BodyBold = new Font("Segoe UI Semibold", 10f);
    public static readonly Font Small = new Font("Segoe UI", 9f);
    public static readonly Font SmallBold = new Font("Segoe UI Semibold", 9f);
    public static readonly Font Tiny = new Font("Segoe UI", 8f);
    public static readonly Font Headline = new Font("Segoe UI Semibold", 13f);

    // Parse hexadecimal color
    public static Color FromHex(string hex, Color fallback)
    {
        if (hex.Length == 6 && int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int rgb))
        {
            return Color.FromArgb((rgb >> 16) & 0xFF, (rgb >> 8) & 0xFF, rgb & 0xFF);
        }
        return fallback;
    }

    // Scale RGB brightness
    public static Color Darken(Color color, float factor)
    {
        return Color.FromArgb((int)(color.R * factor), (int)(color.G * factor), (int)(color.B * factor));
    }
}
