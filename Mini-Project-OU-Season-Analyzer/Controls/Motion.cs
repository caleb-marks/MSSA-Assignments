using System.Runtime.InteropServices;

namespace Caleb_Marks_OU_Season_Analyzer.Controls;

// Shared animation settings
internal static class Motion
{
    // Entrance duration, milliseconds
    public const int Length = 320;
    // Build duration, milliseconds
    public const int Build = 900;
    // Frame interval, milliseconds
    public const int Tick = 16;
    private const uint ClientAreaAnimation = 0x1042;

    // Read Windows animation setting
    [DllImport("user32.dll")]
    private static extern bool SystemParametersInfo(uint action, uint param, ref bool value, uint flags);

    // Disable application animations
    public static bool UserOff { get; set; }

    // Check effective animation preference
    public static bool Enabled
    {
        get
        {
            if (UserOff)
            {
                return false;
            }
            bool clientArea = true;
            if (!SystemParametersInfo(ClientAreaAnimation, 0, ref clientArea, 0))
            {
                clientArea = true;
            }
            return clientArea && SystemInformation.UIEffectsEnabled && !SystemInformation.HighContrast;
        }
    }

    // Calculate cubic easing
    public static float EaseOut(float t)
    {
        float left = 1 - Math.Clamp(t, 0f, 1f);
        return 1 - left * left * left;
    }

    // Blend RGB colors
    public static Color Mix(Color from, Color to, float t)
    {
        t = Math.Clamp(t, 0f, 1f);
        return Color.FromArgb(
            (int)(from.R + (to.R - from.R) * t),
            (int)(from.G + (to.G - from.G) * t),
            (int)(from.B + (to.B - from.B) * t));
    }
}
