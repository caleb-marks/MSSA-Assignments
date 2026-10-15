using Caleb_Marks_Assignment_3._3.Models;

namespace Caleb_Marks_Assignment_3._3
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadStudents();
        }

        // Reconnect the list so the grid shows added or deleted students
        private void LoadStudents()
        {
            try
            {
                studentGrid.DataSource = null;
                studentGrid.DataSource = Data.Students;
                FormatColumns();
            }
            catch (Exception error)
            {
                MessageBox.Show("The student list could not be refreshed.\n" + error.Message,
                    "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Set the column headings and widths after refreshing
        private void FormatColumns()
        {
            DataGridViewColumn? idColumn = studentGrid.Columns[nameof(Student.StudId)];
            DataGridViewColumn? firstColumn = studentGrid.Columns[nameof(Student.FirstName)];
            DataGridViewColumn? lastColumn = studentGrid.Columns[nameof(Student.LastName)];
            DataGridViewColumn? addressColumn = studentGrid.Columns[nameof(Student.Address)];
            DataGridViewColumn? monthColumn = studentGrid.Columns[nameof(Student.MonthOfAdmission)];
            DataGridViewColumn? gradeColumn = studentGrid.Columns[nameof(Student.Grade)];

            if (idColumn == null || firstColumn == null || lastColumn == null
                || addressColumn == null || monthColumn == null || gradeColumn == null)
            {
                return;   // nothing is bound yet
            }

            idColumn.HeaderText = "Student Id";
            firstColumn.HeaderText = "First Name";
            lastColumn.HeaderText = "Last Name";
            addressColumn.HeaderText = "Address";
            monthColumn.HeaderText = "Month of Admission";
            gradeColumn.HeaderText = "Grade";

            // Give the address column more room
            idColumn.FillWeight = 60;
            firstColumn.FillWeight = 90;
            lastColumn.FillWeight = 90;
            addressColumn.FillWeight = 180;
            monthColumn.FillWeight = 110;
            gradeColumn.FillWeight = 50;

            gradeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            studentGrid.ClearSelection();
        }

        // Get the selected student or ask the user to select a row
        private Student? GetSelectedStudent(string action)
        {
            if (studentGrid.SelectedRows.Count > 0)
            {
                object? selectedItem = studentGrid.SelectedRows[0].DataBoundItem;
                if (selectedItem != null)
                {
                    return (Student)selectedItem;
                }
            }

            MessageBox.Show($"Please select a student to {action}.", "Nothing Selected",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (AddForm form = new AddForm())
                {
                    // Refresh the grid after adding a student
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadStudents();
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("The student form could not be opened.\n" + error.Message,
                    "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Student? selected = GetSelectedStudent("delete");
                if (selected == null)
                {
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete {selected.FirstName} {selected.LastName}?", "Warning",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Data.Students.Remove(selected);
                    LoadStudents();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("The delete operation could not be completed.\n" + error.Message,
                    "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
