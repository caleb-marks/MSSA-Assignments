using System.IO;

namespace Caleb_Marks_Assignment_4._2
{
    public partial class MainForm : Form
    {
        // Keep student records while this form remains open
        private readonly List<Student> students = new List<Student>();

        // Create form & display student list
        public MainForm()
        {
            InitializeComponent();
            LoadStudents();
        }

        // Refresh student grid
        private void LoadStudents()
        {
            // Use fresh list copy so grid displays current students
            studentGrid.DataSource = null;
            studentGrid.DataSource = new List<Student>(students);
            studentGrid.ClearSelection();
        }

        // Validate inputs & add student
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate each student field
            int studentId;
            decimal gpa;
            if (!Int32.TryParse(txtStudentId.Text, out studentId) || studentId <= 0)
            {
                ShowProblem("Enter a student ID greater than zero using whole numbers.", txtStudentId);
                return;
            }
            // Reject duplicate student IDs
            foreach (Student student in students)
            {
                if (student.StudentId == studentId)
                {
                    ShowProblem("That student ID is already in the list.", txtStudentId);
                    return;
                }
            }
            // Remove extra spaces & make sure name was entered
            string name = txtStudentName.Text.Trim();
            if (name.Length == 0)
            {
                ShowProblem("Enter the student's name.", txtStudentName);
                return;
            }
            // Accept numeric GPA values: 0-4
            if (!Decimal.TryParse(txtGpa.Text, out gpa) || gpa < 0 || gpa > 4)
            {
                ShowProblem("Enter a GPA from 0 to 4.", txtGpa);
                return;
            }

            // Add student, refresh list, & clear inputs
            Student newStudent = new Student();
            newStudent.StudentId = studentId;
            newStudent.StudentName = name;
            newStudent.GPA = gpa;
            students.Add(newStudent);
            LoadStudents();
            ClearInputs();
        }

        // Confirm & delete selected student
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Require student selection
            if (studentGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a student to delete.", "Nothing Selected");
                return;
            }
            // Get selected row's student
            object? selectedItem = studentGrid.SelectedRows[0].DataBoundItem;
            if (selectedItem == null)
            {
                return;
            }
            Student student = (Student)selectedItem;
            // Remove student only if teacher confirms
            DialogResult result = MessageBox.Show("Delete " + student.StudentName + "?", "Delete Student",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                students.Remove(student);
                LoadStudents();
            }
        }

        // Export highest-GPA student's details
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Require nonempty student list
            if (students.Count == 0)
            {
                MessageBox.Show("Add a student before saving the highest GPA.", "No Students");
                return;
            }

            // Compare GPAs, starting first
            Student highest = students[0];
            for (int i = 1; i < students.Count; i++)
            {
                // Keep first student if highest GPA is tied
                if (students[i].GPA > highest.GPA)
                {
                    highest = students[i];
                }
            }

            // Let teacher choose text file location
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = "Save Student with Highest GPA";
                dialog.Filter = "Text files (*.txt)|*.txt";
                dialog.FileName = "HighestGPAStudent.txt";
                dialog.DefaultExt = "txt";
                dialog.AddExtension = true;
                // Stop if teacher cancels save
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    // Write highest-GPA student's three details
                    using (StreamWriter writer = new StreamWriter(dialog.FileName))
                    {
                        writer.WriteLine("Student ID: " + highest.StudentId);
                        writer.WriteLine("Student name: " + highest.StudentName);
                        writer.WriteLine("GPA: " + highest.GPA);
                    }
                    MessageBox.Show("Student details saved to:\n" + dialog.FileName, "Saved",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // Show helpful message if file cannot be saved
                catch (Exception error)
                {
                    MessageBox.Show("The file could not be saved. Try another location.\n" + error.Message,
                        "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Clear inputs when button is clicked
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        // Clear boxes; keep student list
        private void ClearInputs()
        {
            txtStudentId.Clear();
            txtStudentName.Clear();
            txtGpa.Clear();
            txtStudentId.Focus();
        }

        // Show problem & focus incorrect field
        private static void ShowProblem(string message, Control input)
        {
            MessageBox.Show(message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            input.Focus();
        }
    }
}
