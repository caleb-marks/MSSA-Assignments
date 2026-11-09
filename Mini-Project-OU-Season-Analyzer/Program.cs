// Caleb Marks

using Caleb_Marks_OU_Season_Analyzer.Logic;

namespace Caleb_Marks_OU_Season_Analyzer;

internal static class Program
{
    // Route startup arguments
    [STAThread]
    static int Main(string[] args)
    {
        if (args.Contains("--selftest"))
        {
            SelfTestResult result = SelfTest.Run();
            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "selftest.txt"), result.Report);
            return result.AllPassed ? 0 : 1;
        }

        ApplicationConfiguration.Initialize();
        int shots = Array.IndexOf(args, "--shots");
        if (shots >= 0 && shots + 1 < args.Length)
        {
            // Report capture errors
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
            try
            {
                return StoryForm.Shoot(args[shots + 1]);
            }
            catch (Exception error)
            {
                File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "shots-error.txt"), error.ToString());
                return 2;
            }
        }
        // Enable live video playback
        Controls.ClipPlayer.Allowed = true;
        Application.Run(new StoryForm(args.Contains("--offline")));
        return 0;
    }
}
