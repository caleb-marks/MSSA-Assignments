// Caleb Marks

using System.IO;

namespace Caleb_Marks_Assignment_2._3_Part1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string fileName = "PersonalDetails.txt";

            try
            {
                // Create the text file and write dummy personal details to it.
                using (StreamWriter writer = File.CreateText(fileName))
                {
                    writer.WriteLine("Name: Baker Mayfield");
                    writer.WriteLine("Age: 30");
                    writer.WriteLine("Address: 24 Kobe Street");
                }

                // Read the same text file and display its contents.
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string personalDetails = reader.ReadToEnd();

                    Console.WriteLine("Personal Details:");
                    Console.WriteLine(personalDetails);
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("The file could not be created or read.");
                Console.WriteLine(error.Message);
            }
        }
    }
}
