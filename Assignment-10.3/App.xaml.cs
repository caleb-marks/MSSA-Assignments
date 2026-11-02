using System.Windows;
using Caleb_Marks_Assignment_10._3.Data;
using Microsoft.EntityFrameworkCore;

namespace Caleb_Marks_Assignment_10._3
{
    public partial class App : Application
    {
        // Create context, build database if missing, & load cars
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Records.Context = new CarContext();
            try
            {
                Records.Context.Database.EnsureCreated();
                Records.Context.Cars.Load();
            }
            catch (Exception error)
            {
                MessageBox.Show("The car database could not be opened. Check that SQL Server is running.\n\n" + error.Message);
                Shutdown();
            }
        }
    }
}
