// Caleb Marks

namespace Caleb_Marks_Assignment_4._1
{
    // Start the phone book application
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
