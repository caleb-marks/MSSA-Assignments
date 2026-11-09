// Caleb Marks

namespace Caleb_Marks_Assignment_11._3_Client
{
    internal static class Program
    {
        // Start games window
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
