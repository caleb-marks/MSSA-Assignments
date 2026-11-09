namespace Caleb_Marks_Assignment_3._3
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            DataGridViewCellStyle alternateStyle = new DataGridViewCellStyle();
            studentGrid = new DataGridView();
            buttonLayout = new TableLayoutPanel();
            btnAdd = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            ((System.ComponentModel.ISupportInitialize)studentGrid).BeginInit();
            buttonLayout.SuspendLayout();
            SuspendLayout();
            //
            // studentGrid
            //
            studentGrid.AllowUserToAddRows = false;
            studentGrid.AllowUserToDeleteRows = false;
            studentGrid.AllowUserToResizeRows = false;
            studentGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            // Adjust row heights to fit the text
            studentGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            studentGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            studentGrid.BackgroundColor = SystemColors.Window;
            studentGrid.BorderStyle = BorderStyle.FixedSingle;
            studentGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            studentGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            headerStyle.BackColor = SystemColors.Control;
            headerStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            headerStyle.ForeColor = SystemColors.WindowText;
            headerStyle.Padding = new Padding(10, 8, 10, 8);
            headerStyle.SelectionBackColor = SystemColors.Control;
            headerStyle.SelectionForeColor = SystemColors.WindowText;
            studentGrid.ColumnHeadersDefaultCellStyle = headerStyle;
            studentGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            cellStyle.BackColor = SystemColors.Window;
            cellStyle.Padding = new Padding(10, 7, 10, 7);
            cellStyle.SelectionBackColor = SystemColors.Highlight;
            cellStyle.SelectionForeColor = SystemColors.HighlightText;
            studentGrid.DefaultCellStyle = cellStyle;
            alternateStyle.BackColor = Color.FromArgb(247, 247, 249);
            alternateStyle.Padding = new Padding(10, 7, 10, 7);
            alternateStyle.SelectionBackColor = SystemColors.Highlight;
            alternateStyle.SelectionForeColor = SystemColors.HighlightText;
            studentGrid.AlternatingRowsDefaultCellStyle = alternateStyle;
            studentGrid.EnableHeadersVisualStyles = false;
            studentGrid.GridColor = Color.FromArgb(225, 225, 228);
            studentGrid.Location = new Point(24, 24);
            studentGrid.MultiSelect = false;
            studentGrid.Name = "studentGrid";
            studentGrid.ReadOnly = true;
            studentGrid.RowHeadersVisible = false;
            studentGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            studentGrid.Size = new Size(852, 376);
            studentGrid.TabIndex = 0;
            //
            // buttonLayout
            //
            // Keep the three buttons evenly spaced
            buttonLayout.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            buttonLayout.ColumnCount = 3;
            buttonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            buttonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            buttonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            buttonLayout.Controls.Add(btnAdd, 0, 0);
            buttonLayout.Controls.Add(btnDelete, 1, 0);
            buttonLayout.Controls.Add(btnRefresh, 2, 0);
            buttonLayout.Location = new Point(24, 420);
            buttonLayout.Name = "buttonLayout";
            buttonLayout.RowCount = 1;
            buttonLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            buttonLayout.Size = new Size(852, 44);
            buttonLayout.TabIndex = 1;
            //
            // btnAdd
            //
            btnAdd.Dock = DockStyle.Fill;
            btnAdd.Margin = new Padding(0, 0, 6, 0);
            btnAdd.Name = "btnAdd";
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Add Student";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            //
            // btnDelete
            //
            btnDelete.Dock = DockStyle.Fill;
            btnDelete.Margin = new Padding(6, 0, 6, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Delete Student";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            //
            // btnRefresh
            //
            btnRefresh.Dock = DockStyle.Fill;
            btnRefresh.Margin = new Padding(6, 0, 0, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            //
            // MainForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(900, 488);
            Controls.Add(studentGrid);
            Controls.Add(buttonLayout);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MinimumSize = new Size(700, 420);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Student Management";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)studentGrid).EndInit();
            buttonLayout.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView studentGrid;
        private TableLayoutPanel buttonLayout;
        private Button btnAdd;
        private Button btnDelete;
        private Button btnRefresh;
    }
}
