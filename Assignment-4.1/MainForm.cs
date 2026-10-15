namespace Caleb_Marks_Assignment_4._1
{
    // Display, add, search for, and delete people
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        // Show the phone book when the form opens
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadPeople();
        }

        // Bind the dictionary values to the grid as person records
        private void LoadPeople()
        {
            peopleGrid.DataSource = null;
            peopleGrid.DataSource = Data.People.Values.ToList();
            peopleGrid.ClearSelection();
            txtSearch.Clear();
        }

        // Check the name before adding a new person
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string firstName = txtFirstName.Text.Trim();
                string lastName = txtLastName.Text.Trim();
                if (firstName.Length == 0)
                {
                    ShowProblem("Enter a first name.", txtFirstName);
                    return;
                }
                if (lastName.Length == 0)
                {
                    ShowProblem("Enter a last name.", txtLastName);
                    return;
                }

                // Each full name must have a different dictionary key
                string key = firstName + " " + lastName;
                if (Data.People.ContainsKey(key))
                {
                    ShowProblem("That full name is already in the phone book.", txtFirstName);
                    return;
                }

                Person person = new Person();
                person.FirstName = firstName;
                person.LastName = lastName;
                person.MobilePhone = txtMobilePhone.Text.Trim();
                person.WorkPhone = txtWorkPhone.Text.Trim();
                person.Address = txtAddress.Text.Trim();
                Data.People.Add(key, person);
                LoadPeople();
                ClearInputs();
            }
            catch (Exception error)
            {
                MessageBox.Show("The person could not be added.\n" + error.Message,
                    "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Find an exact full name and display all of that person's details
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string key = txtSearch.Text.Trim();
            if (key.Length == 0)
            {
                ShowProblem("Enter the first and last name to search for.", txtSearch);
                return;
            }

            List<Person> matches = new List<Person>();
            if (Data.People.ContainsKey(key))
            {
                Person person = Data.People[key];
                matches.Add(person);
            }

            peopleGrid.DataSource = null;
            peopleGrid.DataSource = matches;
            peopleGrid.ClearSelection();
            if (matches.Count == 0)
            {
                MessageBox.Show("No person was found with that full name.", "Search Results",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSearch.Focus();
            }
        }

        // Require a selected person and confirmation before deleting
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (peopleGrid.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Select a person to delete.", "Nothing Selected");
                    return;
                }

                object? selectedItem = peopleGrid.SelectedRows[0].DataBoundItem;
                if (selectedItem == null)
                {
                    return;
                }

                Person person = (Person)selectedItem;
                string key = person.FirstName + " " + person.LastName;
                DialogResult result = MessageBox.Show("Delete " + key + "?", "Delete Person",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    Data.People.Remove(key);
                    LoadPeople();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("The person could not be deleted.\n" + error.Message,
                    "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Return to the complete phone book after a search
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            LoadPeople();
        }

        // Clear entry fields without changing the phone book
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        // Prepare the fields for another person
        private void ClearInputs()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtMobilePhone.Clear();
            txtWorkPhone.Clear();
            txtAddress.Clear();
            txtFirstName.Focus();
        }

        // Explain the problem and return to its input
        private static void ShowProblem(string message, Control input)
        {
            MessageBox.Show(message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            input.Focus();
        }
    }
}
