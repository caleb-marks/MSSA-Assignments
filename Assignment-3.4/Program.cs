// Caleb Marks

namespace Caleb_Marks_Assignment_3._4
{
    // Start the coffee application
    internal class Program
    {
        // Windows Forms uses one main thread for the controls
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
