namespace Caleb_Marks_OU_Season_Analyzer.Models;

// Team identity & colors
internal sealed record Team(string Id, string Short, string Location, string FullName, string Color, string Division,
    string AltColor = "")
{
    // Oklahoma team defaults
    public static readonly Team Oklahoma = new Team("201", "OU", "Oklahoma", "Oklahoma Sooners", "990000", "FBS", "ffffff");

    // Parse primary color; invalid means gray
    public Color DrawColor =>
        Color.Length == 6 && int.TryParse(Color, System.Globalization.NumberStyles.HexNumber, null, out int rgb)
            ? System.Drawing.Color.FromArgb(255, System.Drawing.Color.FromArgb(rgb))
            : System.Drawing.Color.FromArgb(0x55, 0x55, 0x55);

    // Parse alternate color; invalid means primary
    public Color DrawAltColor =>
        AltColor.Length == 6 && int.TryParse(AltColor, System.Globalization.NumberStyles.HexNumber, null, out int rgb)
            ? System.Drawing.Color.FromArgb(255, System.Drawing.Color.FromArgb(rgb))
            : DrawColor;

    // Check bowl eligibility rules
    public bool HasBowls => Division != "FCS";

    // Format team label
    public override string ToString() => $"{FullName} ({Short}) · {Division}";
}
