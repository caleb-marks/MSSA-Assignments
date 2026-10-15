namespace Caleb_Marks_Assignment_1._4
{
    // Define a class for storing student information
    internal class Student
    {
        // Store the student's identification number
        private int studentId;
        // Store the student's first name
        private string studentFname = "";
        // Store the student's last name
        private string studentLname = "";
        // Store the student's grade
        private char studentGrade;

        // Allow access to the student's identification number
        public int StudentId
        {
            // Return the student's identification number
            get
            {
                return studentId;
            }
            // Set the student's identification number
            set
            {
                studentId = value;
            }
        }

        // Allow access to the student's first name
        public string StudentFname
        {
            // Return the student's first name
            get
            {
                return studentFname;
            }
            // Set the student's first name
            set
            {
                studentFname = value;
            }
        }

        // Allow access to the student's last name
        public string StudentLname
        {
            // Return the student's last name
            get
            {
                return studentLname;
            }
            // Set the student's last name
            set
            {
                studentLname = value;
            }
        }

        // Allow access to the student's grade
        public char StudentGrade
        {
            // Return the student's grade
            get
            {
                return studentGrade;
            }
            // Set the student's grade
            set
            {
                studentGrade = value;
            }
        }
    }
}
