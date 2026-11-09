namespace Caleb_Marks_Assignment_3._4
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

        // Arrange the entry controls above the coffee grid
        private void InitializeComponent()
        {
            lblId = new Label();
            lblName = new Label();
            lblPrice = new Label();
            lblSize = new Label();
            lblRoast = new Label();
            txtId = new TextBox();
            txtName = new TextBox();
            txtPrice = new TextBox();
            txtSize = new TextBox();
            comboRoast = new ComboBox();
            checkDecaf = new CheckBox();
            btnAdd = new Button();
            btnDelete = new Button();
            btnClear = new Button();
            coffeeGrid = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)coffeeGrid).BeginInit();
            SuspendLayout();

            // Label each input clearly
            lblId.AutoSize = true;
            lblId.Location = new Point(24, 28);
            lblId.Name = "lblId";
            lblId.Text = "Coffee ID";
            lblName.AutoSize = true;
            lblName.Location = new Point(232, 28);
            lblName.Name = "lblName";
            lblName.Text = "Name";
            lblPrice.AutoSize = true;
            lblPrice.Location = new Point(556, 28);
            lblPrice.Name = "lblPrice";
            lblPrice.Text = "Price ($)";
            lblSize.AutoSize = true;
            lblSize.Location = new Point(24, 78);
            lblSize.Name = "lblSize";
            lblSize.Text = "Size (ml)";
            lblRoast.AutoSize = true;
            lblRoast.Location = new Point(232, 78);
            lblRoast.Name = "lblRoast";
            lblRoast.Text = "Roast";

            // Read the common beverage properties
            txtId.Location = new Point(120, 24);
            txtId.Name = "txtId";
            txtId.Size = new Size(80, 23);
            txtId.TabIndex = 0;
            txtName.Location = new Point(292, 24);
            txtName.Name = "txtName";
            txtName.Size = new Size(240, 23);
            txtName.TabIndex = 1;
            txtPrice.Location = new Point(624, 24);
            txtPrice.Name = "txtPrice";
            txtPrice.Size = new Size(120, 23);
            txtPrice.TabIndex = 2;
            txtSize.Location = new Point(120, 74);
            txtSize.Name = "txtSize";
            txtSize.Size = new Size(80, 23);
            txtSize.TabIndex = 3;

            // Read the properties specific to coffee
            comboRoast.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRoast.Location = new Point(292, 74);
            comboRoast.Name = "comboRoast";
            comboRoast.Size = new Size(160, 23);
            comboRoast.TabIndex = 4;
            checkDecaf.AutoSize = true;
            checkDecaf.Location = new Point(480, 76);
            checkDecaf.Name = "checkDecaf";
            checkDecaf.Text = "Decaffeinated";
            checkDecaf.TabIndex = 5;

            // Connect each action to its event handler
            btnAdd.Location = new Point(24, 124);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(140, 36);
            btnAdd.Text = "Add Coffee";
            btnAdd.TabIndex = 6;
            btnAdd.Click += btnAdd_Click;
            btnDelete.Location = new Point(180, 124);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(140, 36);
            btnDelete.Text = "Delete Selected";
            btnDelete.TabIndex = 7;
            btnDelete.Click += btnDelete_Click;
            btnClear.Location = new Point(336, 124);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(140, 36);
            btnClear.Text = "Clear Inputs";
            btnClear.TabIndex = 8;
            btnClear.Click += btnClear_Click;

            // Display complete records and select one whole row at a time
            coffeeGrid.AllowUserToAddRows = false;
            coffeeGrid.AllowUserToDeleteRows = false;
            coffeeGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            coffeeGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            coffeeGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            coffeeGrid.BackgroundColor = SystemColors.Window;
            coffeeGrid.Location = new Point(24, 184);
            coffeeGrid.Name = "coffeeGrid";
            coffeeGrid.Size = new Size(912, 352);
            coffeeGrid.ReadOnly = true;
            coffeeGrid.MultiSelect = false;
            coffeeGrid.RowHeadersVisible = false;
            coffeeGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            coffeeGrid.TabIndex = 9;

            // Keep the layout usable when the window is resized
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(960, 560);
            MinimumSize = new Size(810, 470);
            Name = "MainForm";
            Text = "Coffee List - Assignment 3.4";
            StartPosition = FormStartPosition.CenterScreen;
            Controls.Add(lblId);
            Controls.Add(lblName);
            Controls.Add(lblPrice);
            Controls.Add(lblSize);
            Controls.Add(lblRoast);
            Controls.Add(txtId);
            Controls.Add(txtName);
            Controls.Add(txtPrice);
            Controls.Add(txtSize);
            Controls.Add(comboRoast);
            Controls.Add(checkDecaf);
            Controls.Add(btnAdd);
            Controls.Add(btnDelete);
            Controls.Add(btnClear);
            Controls.Add(coffeeGrid);
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)coffeeGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblId;
        private Label lblName;
        private Label lblPrice;
        private Label lblSize;
        private Label lblRoast;
        private TextBox txtId;
        private TextBox txtName;
        private TextBox txtPrice;
        private TextBox txtSize;
        private ComboBox comboRoast;
        private CheckBox checkDecaf;
        private Button btnAdd;
        private Button btnDelete;
        private Button btnClear;
        private DataGridView coffeeGrid;
    }
}
