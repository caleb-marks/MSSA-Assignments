// Caleb Marks

namespace Caleb_Marks_Assignment_4._1._2
{
    internal class Program
    {
        // Set up the app and open the calculator form
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
