namespace Caleb_Marks_Assignment_4._2
{
    partial class MainForm
    {
        // Track form components
        private System.ComponentModel.IContainer components = null;

        // Release form components when they are no longer needed
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Create & arrange form controls
        private void InitializeComponent()
        {
            // Create input controls, buttons, & student grid
            lblStudentId = new Label();
            txtStudentId = new TextBox();
            lblStudentName = new Label();
            txtStudentName = new TextBox();
            lblGpa = new Label();
            txtGpa = new TextBox();
            btnAdd = new Button();
            btnClear = new Button();
            btnDelete = new Button();
            btnSave = new Button();
            studentGrid = new DataGridView();
            lblStorage = new Label();
            ((System.ComponentModel.ISupportInitialize)studentGrid).BeginInit();
            SuspendLayout();

            // Configure student ID label & input
            lblStudentId.Name = "lblStudentId";
            lblStudentId.Location = new Point(24, 24);
            lblStudentId.Size = new Size(160, 22);
            lblStudentId.Text = "Student ID";
            lblStudentId.TabIndex = 0;

            txtStudentId.Name = "txtStudentId";
            txtStudentId.Location = new Point(24, 50);
            txtStudentId.Size = new Size(160, 27);
            txtStudentId.TabIndex = 0;

            // Configure student name label & input
            lblStudentName.Name = "lblStudentName";
            lblStudentName.Location = new Point(204, 24);
            lblStudentName.Size = new Size(340, 22);
            lblStudentName.Text = "Student name";
            lblStudentName.TabIndex = 1;

            txtStudentName.Name = "txtStudentName";
            txtStudentName.Location = new Point(204, 50);
            txtStudentName.Size = new Size(340, 27);
            txtStudentName.TabIndex = 1;

            // Configure GPA label & input
            lblGpa.Name = "lblGpa";
            lblGpa.Location = new Point(564, 24);
            lblGpa.Size = new Size(160, 22);
            lblGpa.Text = "GPA (0 to 4)";
            lblGpa.TabIndex = 2;

            txtGpa.Name = "txtGpa";
            txtGpa.Location = new Point(564, 50);
            txtGpa.Size = new Size(160, 27);
            txtGpa.TabIndex = 2;

            // Configure Add Student button
            btnAdd.Name = "btnAdd";
            btnAdd.Location = new Point(24, 104);
            btnAdd.Size = new Size(150, 36);
            btnAdd.Text = "Add Student";
            btnAdd.Click += btnAdd_Click;
            btnAdd.TabIndex = 3;

            // Configure Clear Inputs button
            btnClear.Name = "btnClear";
            btnClear.Location = new Point(184, 104);
            btnClear.Size = new Size(150, 36);
            btnClear.Text = "Clear Inputs";
            btnClear.Click += btnClear_Click;
            btnClear.TabIndex = 4;

            // Configure Delete Selected button
            btnDelete.Name = "btnDelete";
            btnDelete.Location = new Point(344, 104);
            btnDelete.Size = new Size(160, 36);
            btnDelete.Text = "Delete Selected";
            btnDelete.Click += btnDelete_Click;
            btnDelete.TabIndex = 5;

            // Configure Save Highest GPA button
            btnSave.Name = "btnSave";
            btnSave.Location = new Point(514, 104);
            btnSave.Size = new Size(210, 36);
            btnSave.Text = "Save Highest GPA";
            btnSave.Click += btnSave_Click;
            btnSave.TabIndex = 6;

            // Show student details & allow single-row selection
            studentGrid.Name = "studentGrid";
            studentGrid.Location = new Point(24, 164);
            studentGrid.Size = new Size(700, 285);
            studentGrid.ReadOnly = true;
            studentGrid.AllowUserToAddRows = false;
            studentGrid.AllowUserToDeleteRows = false;
            studentGrid.AllowUserToResizeRows = false;
            studentGrid.MultiSelect = false;
            studentGrid.RowHeadersVisible = false;
            studentGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            studentGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            studentGrid.AutoGenerateColumns = false;
            studentGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            studentGrid.TabIndex = 7;

            // Explain how long student list is kept
            lblStorage.Name = "lblStorage";
            lblStorage.Location = new Point(24, 465);
            lblStorage.Size = new Size(700, 24);
            lblStorage.Text = "Students are kept while this window is open.";
            lblStorage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblStorage.TabIndex = 8;

            // Set window size, title, & starting position
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(750, 510);
            Text = "Students - Assignment 4.2";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(750, 510);
            // Populate form controls
            Controls.Add(lblStudentId);
            Controls.Add(txtStudentId);
            Controls.Add(lblStudentName);
            Controls.Add(txtStudentName);
            Controls.Add(lblGpa);
            Controls.Add(txtGpa);
            Controls.Add(btnAdd);
            Controls.Add(btnClear);
            Controls.Add(btnDelete);
            Controls.Add(btnSave);
            Controls.Add(studentGrid);
            Controls.Add(lblStorage);
            // Match grid columns & Student properties
            DataGridViewTextBoxColumn colStudentId = new DataGridViewTextBoxColumn();
            colStudentId.DataPropertyName = "StudentId";
            colStudentId.HeaderText = "Student ID";
            colStudentId.SortMode = DataGridViewColumnSortMode.NotSortable;
            studentGrid.Columns.Add(colStudentId);
            DataGridViewTextBoxColumn colStudentName = new DataGridViewTextBoxColumn();
            colStudentName.DataPropertyName = "StudentName";
            colStudentName.HeaderText = "Student Name";
            colStudentName.SortMode = DataGridViewColumnSortMode.NotSortable;
            studentGrid.Columns.Add(colStudentName);
            DataGridViewTextBoxColumn colGPA = new DataGridViewTextBoxColumn();
            colGPA.DataPropertyName = "GPA";
            colGPA.HeaderText = "GPA";
            colGPA.SortMode = DataGridViewColumnSortMode.NotSortable;
            studentGrid.Columns.Add(colGPA);
            // Finish grid & form layout
            ((System.ComponentModel.ISupportInitialize)studentGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        // Store student form controls
        private Label lblStudentId = null!;
        private TextBox txtStudentId = null!;
        private Label lblStudentName = null!;
        private TextBox txtStudentName = null!;
        private Label lblGpa = null!;
        private TextBox txtGpa = null!;
        private Button btnAdd = null!;
        private Button btnClear = null!;
        private Button btnDelete = null!;
        private Button btnSave = null!;
        private DataGridView studentGrid = null!;
        private Label lblStorage = null!;
    }
}
