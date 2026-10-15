namespace Caleb_Marks_Assignment_4._1
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // Release the form's components when the window closes
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Arrange the entry fields and phone book grid
        private void InitializeComponent()
        {
            lblFirstName = new Label();
            txtFirstName = new TextBox();
            lblLastName = new Label();
            txtLastName = new TextBox();
            lblMobilePhone = new Label();
            txtMobilePhone = new TextBox();
            lblWorkPhone = new Label();
            txtWorkPhone = new TextBox();
            lblAddress = new Label();
            txtAddress = new TextBox();
            btnAdd = new Button();
            btnClear = new Button();
            btnSearch = new Button();
            btnShowAll = new Button();
            btnDelete = new Button();
            colFirstName = new DataGridViewTextBoxColumn();
            colLastName = new DataGridViewTextBoxColumn();
            colMobilePhone = new DataGridViewTextBoxColumn();
            colWorkPhone = new DataGridViewTextBoxColumn();
            colAddress = new DataGridViewTextBoxColumn();
            lblSearch = new Label();
            txtSearch = new TextBox();
            lblStorage = new Label();
            peopleGrid = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)peopleGrid).BeginInit();
            SuspendLayout();

            lblFirstName.AutoSize = true;
            lblFirstName.Location = new Point(24, 24);
            lblFirstName.Name = "lblFirstName";
            lblFirstName.Text = "First name";
            txtFirstName.Location = new Point(24, 48);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.AccessibleName = "First name";
            txtFirstName.Size = new Size(320, 23);
            txtFirstName.TabIndex = 0;

            lblLastName.AutoSize = true;
            lblLastName.Location = new Point(372, 24);
            lblLastName.Name = "lblLastName";
            lblLastName.Text = "Last name";
            txtLastName.Location = new Point(372, 48);
            txtLastName.Name = "txtLastName";
            txtLastName.AccessibleName = "Last name";
            txtLastName.Size = new Size(320, 23);
            txtLastName.TabIndex = 1;

            lblMobilePhone.AutoSize = true;
            lblMobilePhone.Location = new Point(24, 84);
            lblMobilePhone.Name = "lblMobilePhone";
            lblMobilePhone.Text = "Mobile phone (optional)";
            txtMobilePhone.Location = new Point(24, 108);
            txtMobilePhone.Name = "txtMobilePhone";
            txtMobilePhone.AccessibleName = "Mobile phone (optional)";
            txtMobilePhone.Size = new Size(320, 23);
            txtMobilePhone.TabIndex = 2;

            lblWorkPhone.AutoSize = true;
            lblWorkPhone.Location = new Point(372, 84);
            lblWorkPhone.Name = "lblWorkPhone";
            lblWorkPhone.Text = "Work phone (optional)";
            txtWorkPhone.Location = new Point(372, 108);
            txtWorkPhone.Name = "txtWorkPhone";
            txtWorkPhone.AccessibleName = "Work phone (optional)";
            txtWorkPhone.Size = new Size(320, 23);
            txtWorkPhone.TabIndex = 3;

            lblAddress.AutoSize = true;
            lblAddress.Location = new Point(24, 144);
            lblAddress.Name = "lblAddress";
            lblAddress.Text = "Address (optional)";
            txtAddress.Location = new Point(24, 168);
            txtAddress.Name = "txtAddress";
            txtAddress.AccessibleName = "Address (optional)";
            txtAddress.Size = new Size(668, 23);
            txtAddress.TabIndex = 4;

            btnAdd.Location = new Point(732, 48);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(220, 34);
            btnAdd.Text = "Add Person";
            btnAdd.TabIndex = 5;
            btnAdd.Click += btnAdd_Click;

            btnClear.Location = new Point(732, 108);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(220, 34);
            btnClear.Text = "Clear Inputs";
            btnClear.TabIndex = 6;
            btnClear.Click += btnClear_Click;

            btnSearch.Location = new Point(580, 234);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(112, 34);
            btnSearch.Text = "Search";
            btnSearch.TabIndex = 8;
            btnSearch.Click += btnSearch_Click;

            btnShowAll.Location = new Point(708, 234);
            btnShowAll.Name = "btnShowAll";
            btnShowAll.Size = new Size(112, 34);
            btnShowAll.Text = "Show All";
            btnShowAll.TabIndex = 9;
            btnShowAll.Click += btnShowAll_Click;

            btnDelete.Location = new Point(836, 234);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(140, 34);
            btnDelete.Text = "Delete Selected";
            btnDelete.TabIndex = 10;
            btnDelete.Click += btnDelete_Click;

            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(24, 210);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Search by full name (first and last)";
            txtSearch.Location = new Point(24, 240);
            txtSearch.Name = "txtSearch";
            txtSearch.AccessibleName = "Search by full name";
            txtSearch.Size = new Size(532, 23);
            txtSearch.TabIndex = 7;

            lblStorage.AutoSize = true;
            lblStorage.Location = new Point(24, 570);
            lblStorage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblStorage.Name = "lblStorage";
            lblStorage.Text = "Contacts are kept while this window is open.";

            // Show all five details even when the phone book is empty
            colFirstName.DataPropertyName = "FirstName";
            colFirstName.HeaderText = "First Name";
            colFirstName.Name = "colFirstName";
            colFirstName.FillWeight = 90;

            colLastName.DataPropertyName = "LastName";
            colLastName.HeaderText = "Last Name";
            colLastName.Name = "colLastName";
            colLastName.FillWeight = 90;

            colMobilePhone.DataPropertyName = "MobilePhone";
            colMobilePhone.HeaderText = "Mobile Phone";
            colMobilePhone.Name = "colMobilePhone";
            colMobilePhone.FillWeight = 120;

            colWorkPhone.DataPropertyName = "WorkPhone";
            colWorkPhone.HeaderText = "Work Phone";
            colWorkPhone.Name = "colWorkPhone";
            colWorkPhone.FillWeight = 120;

            colAddress.DataPropertyName = "Address";
            colAddress.HeaderText = "Address";
            colAddress.Name = "colAddress";
            colAddress.FillWeight = 180;

            peopleGrid.AutoGenerateColumns = false;
            peopleGrid.Columns.AddRange(new DataGridViewColumn[] { colFirstName, colLastName, colMobilePhone, colWorkPhone, colAddress });
            peopleGrid.AllowUserToAddRows = false;
            peopleGrid.AllowUserToDeleteRows = false;
            peopleGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            peopleGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            peopleGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            peopleGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            peopleGrid.BackgroundColor = SystemColors.Window;
            peopleGrid.Location = new Point(24, 288);
            peopleGrid.Name = "peopleGrid";
            peopleGrid.AccessibleName = "Phone book";
            peopleGrid.Size = new Size(952, 262);
            peopleGrid.ReadOnly = true;
            peopleGrid.MultiSelect = false;
            peopleGrid.RowHeadersVisible = false;
            peopleGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            peopleGrid.TabIndex = 11;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 600);
            MinimumSize = new Size(1016, 500);
            Name = "MainForm";
            Text = "Phone Book - Assignment 4.1";
            StartPosition = FormStartPosition.CenterScreen;
            Controls.Add(lblFirstName);
            Controls.Add(txtFirstName);
            Controls.Add(lblLastName);
            Controls.Add(txtLastName);
            Controls.Add(lblMobilePhone);
            Controls.Add(txtMobilePhone);
            Controls.Add(lblWorkPhone);
            Controls.Add(txtWorkPhone);
            Controls.Add(lblAddress);
            Controls.Add(txtAddress);
            Controls.Add(btnAdd);
            Controls.Add(btnClear);
            Controls.Add(btnSearch);
            Controls.Add(btnShowAll);
            Controls.Add(btnDelete);
            Controls.Add(lblSearch);
            Controls.Add(txtSearch);
            Controls.Add(lblStorage);
            Controls.Add(peopleGrid);
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)peopleGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblFirstName;
        private TextBox txtFirstName;
        private Label lblLastName;
        private TextBox txtLastName;
        private Label lblMobilePhone;
        private TextBox txtMobilePhone;
        private Label lblWorkPhone;
        private TextBox txtWorkPhone;
        private Label lblAddress;
        private TextBox txtAddress;
        private Button btnAdd;
        private Button btnClear;
        private Button btnSearch;
        private Button btnShowAll;
        private Button btnDelete;
        private DataGridViewTextBoxColumn colFirstName;
        private DataGridViewTextBoxColumn colLastName;
        private DataGridViewTextBoxColumn colMobilePhone;
        private DataGridViewTextBoxColumn colWorkPhone;
        private DataGridViewTextBoxColumn colAddress;
        private Label lblSearch;
        private TextBox txtSearch;
        private Label lblStorage;
        private DataGridView peopleGrid;
    }
}
