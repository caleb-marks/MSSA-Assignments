// Caleb Marks

namespace Caleb_Marks_Assignment_4._2
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Initialize Windows Forms
            ApplicationConfiguration.Initialize();
            // Start teacher login
            Application.Run(new LoginForm());
        }
    }
}
