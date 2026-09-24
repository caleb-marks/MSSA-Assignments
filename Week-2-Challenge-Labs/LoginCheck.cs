namespace Caleb_Marks_Week_2_Challenge_Labs
{
    // Check the user ID and password
    internal static class LoginCheck
    {
        // Keep the login locked when returning to the menu
        private static bool lockedOut = false;

        // Allow up to three login attempts
        public static void Run()
        {
            if (lockedOut)
            {
                Console.WriteLine("Too many failed attempts. Access denied.");
                return;
            }

            string correctId = "admin";
            string correctPassword = "password123";
            bool loggedIn = false;

            for (int attempt = 1; attempt <= 3; attempt++)
            {
                Console.Write("Enter user ID: ");
                string userId = Console.ReadLine() ?? "";
                Console.Write("Enter password: ");
                string password = Console.ReadLine() ?? "";

                if (userId == correctId && password == correctPassword)
                {
                    Console.WriteLine("Login successful.");
                    loggedIn = true;
                    break;
                }

                Console.WriteLine("Incorrect user ID or password.");
            }

            // Lock the login after three wrong attempts
            if (!loggedIn)
            {
                lockedOut = true;
                Console.WriteLine("Too many failed attempts. Access denied.");
            }
        }
    }
}
