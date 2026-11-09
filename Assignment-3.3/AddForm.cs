using System.ComponentModel;
using Caleb_Marks_Assignment_3._3.Models;

namespace Caleb_Marks_Assignment_3._3
{
    public partial class AddForm : Form
    {
        // Keep the student added by this form
        internal Student? SavedStudent { get; private set; }

        public AddForm()
        {
            InitializeComponent();
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
            // Include January through December in the month list
            List<MonthOfAdmission> months = new List<MonthOfAdmission>();
            foreach (MonthOfAdmission month in Enum.GetValues(typeof(MonthOfAdmission)))
            {
                if (month != MonthOfAdmission.None)
                {
                    months.Add(month);
                }
            }
            comboMonth.DataSource = months;

            // Fill the grade list with letter grades
            comboGrade.DataSource = new List<char> { 'A', 'B', 'C', 'D', 'F' };

            // Suggest an unused student ID
            txtStudId.Text = NextStudId().ToString();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int id;
                string firstName;
                string lastName;
                string address;
                if (!TryReadInput(out id, out firstName, out lastName, out address))
                {
                    DialogResult = DialogResult.None; // keep the dialog open
                    return;
                }

                // Read the selected month and grade
                MonthOfAdmission month = (MonthOfAdmission)comboMonth.SelectedItem!;
                char grade = (char)comboGrade.SelectedItem!;

                Student newStudent = new Student
                {
                    StudId = id,
                    FirstName = firstName,
                    LastName = lastName,
                    Address = address,
                    MonthOfAdmission = month,
                    Grade = grade
                };

                Data.Students.Add(newStudent);
                SavedStudent = newStudent;
                MessageBox.Show("Record added!");

                DialogResult = DialogResult.OK;
            }
            catch (Exception error)
            {
                MessageBox.Show("The add operation could not be completed.\n" + error.Message,
                    "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Check the student ID when the user leaves the box
        private void txtStudId_Validating(object sender, CancelEventArgs e)
        {
            // Check for a blank ID when the user submits
            if (txtStudId.TextLength == 0)
            {
                errorProvider.SetError(txtStudId, string.Empty);
                return;
            }

            // Check that the student ID is a whole number
            int val;
            if (!Int32.TryParse(txtStudId.Text.Trim(), out val))
            {
                MessageBox.Show("Please enter numbers only!");
                e.Cancel = true;   // keep focus on the text field
                errorProvider.SetError(txtStudId, "Please enter numbers only!");
                return;
            }

            if (val <= 0)
            {
                MessageBox.Show("Student Id must be greater than zero!");
                e.Cancel = true;
                errorProvider.SetError(txtStudId, "Student Id must be greater than zero.");
                return;
            }

            // Clear the error icon
            errorProvider.SetError(txtStudId, string.Empty);
        }

        // Check the entries and show the first problem
        private bool TryReadInput(out int id, out string firstName, out string lastName, out string address)
        {
            id = 0;
            firstName = txtFirstName.Text.Trim();
            lastName = txtLastName.Text.Trim();
            address = txtAddress.Text.Trim();

            if (!Int32.TryParse(txtStudId.Text.Trim(), out id) || id <= 0)
            {
                ShowProblem("Student Id must be a whole number greater than zero.", txtStudId);
                return false;
            }

            // Check whether the student ID is already used
            foreach (Student student in Data.Students)
            {
                if (student.StudId == id)
                {
                    ShowProblem($"Student Id {id} is already in use.", txtStudId);
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                ShowProblem("First Name is required.", txtFirstName);
                return false;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                ShowProblem("Last Name is required.", txtLastName);
                return false;
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                ShowProblem("Address is required.", txtAddress);
                return false;
            }

            if (comboMonth.SelectedItem == null)
            {
                ShowProblem("Select a month of admission.", comboMonth);
                return false;
            }

            if (comboGrade.SelectedItem == null)
            {
                ShowProblem("Select a grade.", comboGrade);
                return false;
            }

            return true;
        }

        private static void ShowProblem(string message, Control focusTarget)
        {
            MessageBox.Show(message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focusTarget.Focus();
        }

        private static int NextStudId()
        {
            int highestId = 0;
            foreach (Student student in Data.Students)
            {
                if (student.StudId > highestId)
                {
                    highestId = student.StudId;
                }
            }

            if (highestId < Int32.MaxValue)
            {
                return highestId + 1;
            }

            // Find an unused ID if the highest ID cannot increase
            int nextId = 1;
            bool used = true;
            while (used)
            {
                used = false;
                foreach (Student student in Data.Students)
                {
                    if (student.StudId == nextId)
                    {
                        nextId++;
                        used = true;
                        break;
                    }
                }
            }

            return nextId;
        }
    }
}
