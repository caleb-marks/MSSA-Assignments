using Caleb_Marks_OU_Season_Analyzer.Controls;

namespace Caleb_Marks_OU_Season_Analyzer.Logic;

// Highlight library checks
// Use poster-only playback
internal static partial class SelfTest
{
    // Check clip library
    private static void CheckClips()
    {
        List<ClipInfo> clips = ClipLibrary.All;
        Check("K1 every clip in highlights.csv sits beside one screen (1-4)", () =>
        {
            int stepsWithClips = 0;
            for (int step = 1; step <= 4; step++)
            {
                if (ClipLibrary.ForStep(step).Count > 0)
                {
                    stepsWithClips++;
                }
            }
            bool good = clips.Count > 0 && clips.TrueForAll(c => c.Step >= 1 && c.Step <= 4);
            return (good, $"{clips.Count} clips on {stepsWithClips} of 4 screens");
        });
        Check("K2 every clip has a poster built into the exe, a line & a credit listed in CREDITS.md", () =>
        {
            string credits = ClipLibrary.Resource("clips.CREDITS.md");
            List<string> bad = new List<string>();
            foreach (ClipInfo clip in clips)
            {
                using Bitmap? poster = ClipLibrary.Poster(clip);
                if (poster == null || clip.Caption.Length == 0 || clip.Credit.Length <= "Video: ".Length || !credits.Contains("| " + clip.Name + ".mp4"))
                {
                    bad.Add(clip.Name);
                }
            }
            return (bad.Count == 0, bad.Count == 0 ? $"{clips.Count} posters & credits" : "bad: " + string.Join(", ", bad));
        });
        Check("K3 every clip's .mp4 is beside the exe & under 2 MB (or folder missing -> posters only)", () =>
        {
            int found = 0;
            List<string> bad = new List<string>();
            foreach (ClipInfo clip in clips)
            {
                string? path = ClipLibrary.VideoPath(clip);
                if (path != null)
                {
                    found++;
                    if (new FileInfo(path).Length >= 2_000_000)
                    {
                        bad.Add(clip.Name);
                    }
                }
            }
            string detail = found == 0 ? "no clip files: posters only" : $"{found} of {clips.Count} clip files";
            return (bad.Count == 0 && (found == 0 || found == clips.Count), bad.Count == 0 ? detail : "too big: " + string.Join(", ", bad));
        });
        Check("K4 clips folder missing: every screen's box shows its poster, never a video", () =>
        {
            string folder = ClipLibrary.Folder;
            ClipLibrary.Folder = Path.Combine(Path.GetTempPath(), "ou-clips-missing-" + Guid.NewGuid().ToString("N"));
            try
            {
                using ClipPlayer box = new ClipPlayer();
                int shown = 0;
                for (int step = 1; step <= 4; step++)
                {
                    box.ShowClips(ClipLibrary.ForStep(step));
                    if (box.ClipName.Length > 0 && !box.IsPlaying)
                    {
                        shown++;
                    }
                }
                return (shown == 4, $"{shown} of 4 screens showed a poster");
            }
            finally
            {
                ClipLibrary.Folder = folder;
            }
        });
    }
}
