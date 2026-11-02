// Caleb Marks

namespace Caleb_Marks_Assignment_10._2
{
    internal class Program
    {
        // Show assignment menu & run selected problem
        static void Main(string[] args)
        {
            int choice = 0;
            while (choice != 6)
            {
                Console.WriteLine("\nAssignment 10.2");
                Console.WriteLine("1. Find positive numbers");
                Console.WriteLine("2. Find employees by salary and age");
                Console.WriteLine("3. Find cities by starting and ending letters");
                Console.WriteLine("4. Find numbers greater than 80");
                Console.WriteLine("5. Find the next greatest letter");
                Console.WriteLine("6. Exit");
                choice = ReadInt("Select an option (1-6): ");
                Console.WriteLine();

                // Run selected problem or exit
                switch (choice)
                {
                    case 1:
                        ShowPositiveNumbers();
                        break;

                    case 2:
                        ShowEmployees();
                        break;

                    case 3:
                        FindCities();
                        break;

                    case 4:
                        ShowNumbersGreaterThan80();
                        break;

                    case 5:
                        RunNextLetter();
                        break;

                    case 6:
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Enter a number from 1 to 6.");
                        break;
                }
            }
        }

        // Select positive numbers using LINQ
        static void ShowPositiveNumbers()
        {
            List<int> numbers = new List<int> { 2, -1, 3, -3, 10, -200 };
            IEnumerable<int> positiveNumbers = from number in numbers
                                               where number > 0
                                               select number;

            Console.WriteLine("Positive numbers:");
            foreach (int number in positiveNumbers)
            {
                Console.Write(number + " ");
            }
            Console.WriteLine();
        }

        // Select employees meeting both conditions
        static void ShowEmployees()
        {
            List<Employee> employees = new List<Employee>();
            employees.Add(new Employee { Name = "Baker Mayfield", Age = 27, Salary = 6500m });
            employees.Add(new Employee { Name = "Jalen Hurts", Age = 29, Salary = 5000m });
            employees.Add(new Employee { Name = "Kyler Murray", Age = 30, Salary = 7000m });
            employees.Add(new Employee { Name = "CeeDee Lamb", Age = 24, Salary = 5500m });
            employees.Add(new Employee { Name = "Mark Andrews", Age = 26, Salary = 4500m });

            IEnumerable<Employee> matchingEmployees = from employee in employees
                                                       where employee.Salary > 5000 && employee.Age < 30
                                                       select employee;

            Console.WriteLine("Employees earning more than $5000 and younger than 30:");
            foreach (Employee employee in matchingEmployees)
            {
                Console.WriteLine(employee.Name + " | Age: " + employee.Age + " | Salary: $" + employee.Salary);
            }
        }

        // Match city names & both letters
        static void FindCities()
        {
            List<string> cities = new List<string>
            {
                "ROME", "LONDON", "NAIROBI", "CALIFORNIA", "ZURICH",
                "NEW DELHI", "AMSTERDAM", "ABU DHABI", "PARIS"
            };
            char start = Char.ToUpper(ReadCharacter("Enter the starting character: "));
            char end = Char.ToUpper(ReadCharacter("Enter the ending character: "));

            string startText = start.ToString();
            string endText = end.ToString();

            IEnumerable<string> matchingCities = from city in cities
                                                 where city.StartsWith(startText) && city.EndsWith(endText)
                                                 select city;

            bool found = false;
            foreach (string city in matchingCities)
            {
                Console.WriteLine("The city starting with " + start + " and ending with " + end + " is: " + city);
                found = true;
            }
            if (!found)
            {
                Console.WriteLine("No cities match those characters.");
            }
        }

        // Select numbers greater than 80 using LINQ
        static void ShowNumbersGreaterThan80()
        {
            List<int> numbers = new List<int> { 55, 200, 740, 76, 230, 482, 95 };
            IEnumerable<int> matchingNumbers = from number in numbers
                                              where number > 80
                                              select number;

            Console.WriteLine("Numbers greater than 80:");
            foreach (int number in matchingNumbers)
            {
                Console.Write(number + " ");
            }
            Console.WriteLine();
        }

        // Read letters & target, then show next greatest letter
        static void RunNextLetter()
        {
            char[] letters = ReadSortedLetters();
            char target = ReadCharacter("Enter the target character: ");
            char result = NextGreatestLetter(letters, target);
            Console.WriteLine("Next greatest letter: " + result);
        }

        // Read sorted letters & retry invalid input
        static char[] ReadSortedLetters()
        {
            Console.Write("Enter sorted letters with no spaces: ");
            string input = Console.ReadLine() ?? "";
            while (!IsValidLetters(input))
            {
                Console.Write("Invalid input. Enter sorted letters with at least two different letters: ");
                input = Console.ReadLine() ?? "";
            }

            return input.ToCharArray();
        }

        // Check letters are sorted & include two different letters
        static bool IsValidLetters(string input)
        {
            if (input.Length < 2 || input[0] == input[input.Length - 1])
            {
                return false;
            }

            // Reject non-letters & out-of-order letters
            for (int i = 0; i < input.Length; i++)
            {
                if (!Char.IsLetter(input[i]) || (i > 0 && input[i] < input[i - 1]))
                {
                    return false;
                }
            }

            return true;
        }

        // Find first greater letter using binary search
        static char NextGreatestLetter(char[] letters, char target)
        {
            int left = 0;
            int right = letters.Length - 1;

            // Narrow search while candidate letters remain
            while (left <= right)
            {
                int mid = (left + right) / 2;
                if (letters[mid] <= target)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            // Wrap around when no greater letter exists
            if (left == letters.Length)
            {
                return letters[0];
            }

            return letters[left];
        }

        // Read whole numbers & retry invalid input
        static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            int number;
            while (!Int32.TryParse(Console.ReadLine(), out number))
            {
                Console.Write("Invalid input. Enter a whole number: ");
            }

            return number;
        }

        // Read one character & retry invalid input
        static char ReadCharacter(string prompt)
        {
            Console.Write(prompt);
            char character;
            while (!Char.TryParse(Console.ReadLine(), out character))
            {
                Console.Write("Invalid input. Enter one character: ");
            }

            return character;
        }
    }
}
