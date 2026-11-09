namespace Caleb_Marks_Week_2_Challenge_Labs
{
    // Check the temperature
    internal static class TemperatureState
    {
        // Display the message for the assignment's temperature range
        public static void Run()
        {
            int temperature = Program.ReadInt("Enter the temperature in Fahrenheit: ");

            if (temperature <= 10)
            {
                Console.WriteLine("Freezing weather");
            }
            else if (temperature <= 20)
            {
                Console.WriteLine("Very Cold weather");
            }
            else if (temperature <= 35)
            {
                Console.WriteLine("Cold weather");
            }
            else if (temperature <= 50)
            {
                Console.WriteLine("Normal in Weather");
            }
            else if (temperature <= 65)
            {
                Console.WriteLine("Its Hot");
            }
            else
            {
                Console.WriteLine("Its very hot.");
            }
        }
    }
}
