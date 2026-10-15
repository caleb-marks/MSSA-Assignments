namespace Caleb_Marks_Assignment_3._4
{
    // Display, add, and delete coffees
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        // Fill the roast choices and show the starting list
        private void MainForm_Load(object sender, EventArgs e)
        {
            comboRoast.Items.Add("Light");
            comboRoast.Items.Add("Medium");
            comboRoast.Items.Add("Dark");
            comboRoast.SelectedIndex = 0;
            LoadCoffees();
        }

        // Rebind a plain List after its records change
        private void LoadCoffees()
        {
            try
            {
                coffeeGrid.DataSource = null;
                coffeeGrid.DataSource = Data.Coffees;
                DataGridViewColumn? idColumn = coffeeGrid.Columns["Id"];
                DataGridViewColumn? sizeColumn = coffeeGrid.Columns["SizeMl"];
                DataGridViewColumn? decafColumn = coffeeGrid.Columns["IsDecaf"];
                DataGridViewColumn? priceColumn = coffeeGrid.Columns["Price"];
                if (idColumn != null && sizeColumn != null && decafColumn != null && priceColumn != null)
                {
                    idColumn.HeaderText = "Coffee ID";
                    sizeColumn.HeaderText = "Size (ml)";
                    decafColumn.HeaderText = "Decaf";
                    priceColumn.DefaultCellStyle.Format = "C2";
                }
                coffeeGrid.ClearSelection();
            }
            catch (Exception error)
            {
                MessageBox.Show("The coffee list could not be refreshed.\n" + error.Message,
                    "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Check the form before creating a new coffee
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int id;
                if (!Int32.TryParse(txtId.Text, out id) || id <= 0)
                {
                    ShowProblem("Coffee ID must be a whole number greater than zero.", txtId);
                    return;
                }

                // Each record needs a different id
                foreach (Coffee existingCoffee in Data.Coffees)
                {
                    if (existingCoffee.Id == id)
                    {
                        ShowProblem("That coffee ID is already in use.", txtId);
                        return;
                    }
                }

                string name = txtName.Text.Trim();
                if (name.Length == 0)
                {
                    ShowProblem("Enter a coffee name.", txtName);
                    return;
                }

                decimal price;
                if (!Decimal.TryParse(txtPrice.Text, out price) || price < 0)
                {
                    ShowProblem("Enter a price of zero or more.", txtPrice);
                    return;
                }

                int sizeMl;
                if (!Int32.TryParse(txtSize.Text, out sizeMl) || sizeMl <= 0)
                {
                    ShowProblem("Size must be a whole number greater than zero.", txtSize);
                    return;
                }

                if (comboRoast.SelectedIndex < 0)
                {
                    ShowProblem("Select a roast.", comboRoast);
                    return;
                }

                // Set both inherited properties and properties specific to Coffee
                Coffee coffee = new Coffee();
                coffee.Id = id;
                coffee.Name = name;
                coffee.Price = price;
                coffee.SizeMl = sizeMl;
                coffee.Roast = comboRoast.Text;
                coffee.IsDecaf = checkDecaf.Checked;
                Data.Coffees.Add(coffee);
                LoadCoffees();
                ClearInputs();
            }
            catch (Exception error)
            {
                MessageBox.Show("The add operation could not be completed.\n" + error.Message,
                    "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Require a selected row and confirmation before removing a coffee
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (coffeeGrid.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Select a coffee to delete.", "Nothing Selected");
                    return;
                }

                object? selectedItem = coffeeGrid.SelectedRows[0].DataBoundItem;
                if (selectedItem == null)
                {
                    return;
                }

                Coffee coffee = (Coffee)selectedItem;
                DialogResult result = MessageBox.Show(
                    "Delete " + coffee.Name + "?", "Delete Coffee", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Data.Coffees.Remove(coffee);
                    LoadCoffees();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("The delete operation could not be completed.\n" + error.Message,
                    "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Clear the entry fields without changing any saved records
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        // Prepare the form for another entry
        private void ClearInputs()
        {
            txtId.Clear();
            txtName.Clear();
            txtPrice.Clear();
            txtSize.Clear();
            comboRoast.SelectedIndex = 0;
            checkDecaf.Checked = false;
            txtId.Focus();
        }

        // Explain the problem and return to its input
        private static void ShowProblem(string message, Control input)
        {
            MessageBox.Show(message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            input.Focus();
        }
    }
}
