namespace Caleb_Marks_Assignment_3._3
{
    partial class AddForm
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
            components = new System.ComponentModel.Container();
            fieldLayout = new TableLayoutPanel();
            lblStudId = new Label();
            txtStudId = new TextBox();
            lblFirstName = new Label();
            txtFirstName = new TextBox();
            lblLastName = new Label();
            txtLastName = new TextBox();
            lblAddress = new Label();
            txtAddress = new TextBox();
            lblMonth = new Label();
            comboMonth = new ComboBox();
            lblGrade = new Label();
            comboGrade = new ComboBox();
            btnSubmit = new Button();
            btnCancel = new Button();
            errorProvider = new ErrorProvider(components);
            fieldLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // fieldLayout
            // 
            fieldLayout.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            fieldLayout.ColumnCount = 2;
            fieldLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            fieldLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            fieldLayout.Controls.Add(lblStudId, 0, 0);
            fieldLayout.Controls.Add(txtStudId, 1, 0);
            fieldLayout.Controls.Add(lblFirstName, 0, 1);
            fieldLayout.Controls.Add(txtFirstName, 1, 1);
            fieldLayout.Controls.Add(lblLastName, 0, 2);
            fieldLayout.Controls.Add(txtLastName, 1, 2);
            fieldLayout.Controls.Add(lblAddress, 0, 3);
            fieldLayout.Controls.Add(txtAddress, 1, 3);
            fieldLayout.Controls.Add(lblMonth, 0, 4);
            fieldLayout.Controls.Add(comboMonth, 1, 4);
            fieldLayout.Controls.Add(lblGrade, 0, 5);
            fieldLayout.Controls.Add(comboGrade, 1, 5);
            fieldLayout.Location = new Point(24, 24);
            fieldLayout.Name = "fieldLayout";
            fieldLayout.RowCount = 6;
            fieldLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            fieldLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            fieldLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            fieldLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            fieldLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            fieldLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            fieldLayout.Size = new Size(1357, 240);
            fieldLayout.TabIndex = 0;
            // 
            // lblStudId
            // 
            lblStudId.Anchor = AnchorStyles.Left;
            lblStudId.AutoSize = true;
            lblStudId.Location = new Point(3, 0);
            lblStudId.Name = "lblStudId";
            lblStudId.Size = new Size(129, 40);
            lblStudId.TabIndex = 0;
            lblStudId.Text = "Student Id";
            // 
            // txtStudId
            // 
            txtStudId.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtStudId.Location = new Point(153, 3);
            txtStudId.Name = "txtStudId";
            txtStudId.Size = new Size(1201, 47);
            txtStudId.TabIndex = 0;
            txtStudId.Validating += txtStudId_Validating;
            // 
            // lblFirstName
            // 
            lblFirstName.Anchor = AnchorStyles.Left;
            lblFirstName.AutoSize = true;
            lblFirstName.Location = new Point(3, 40);
            lblFirstName.Name = "lblFirstName";
            lblFirstName.Size = new Size(97, 40);
            lblFirstName.TabIndex = 1;
            lblFirstName.Text = "First Name";
            // 
            // txtFirstName
            // 
            txtFirstName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtFirstName.Location = new Point(153, 43);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(1201, 47);
            txtFirstName.TabIndex = 1;
            // 
            // lblLastName
            // 
            lblLastName.Anchor = AnchorStyles.Left;
            lblLastName.AutoSize = true;
            lblLastName.Location = new Point(3, 80);
            lblLastName.Name = "lblLastName";
            lblLastName.Size = new Size(97, 40);
            lblLastName.TabIndex = 2;
            lblLastName.Text = "Last Name";
            // 
            // txtLastName
            // 
            txtLastName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtLastName.Location = new Point(153, 83);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(1201, 47);
            txtLastName.TabIndex = 2;
            // 
            // lblAddress
            // 
            lblAddress.Anchor = AnchorStyles.Left;
            lblAddress.AutoSize = true;
            lblAddress.Location = new Point(3, 120);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(125, 40);
            lblAddress.TabIndex = 3;
            lblAddress.Text = "Address";
            // 
            // txtAddress
            // 
            txtAddress.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtAddress.Location = new Point(153, 123);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(1201, 47);
            txtAddress.TabIndex = 3;
            // 
            // lblMonth
            // 
            lblMonth.Anchor = AnchorStyles.Left;
            lblMonth.AutoSize = true;
            lblMonth.Location = new Point(3, 160);
            lblMonth.Name = "lblMonth";
            lblMonth.Size = new Size(142, 40);
            lblMonth.TabIndex = 4;
            lblMonth.Text = "Month of Admission";
            // 
            // comboMonth
            // 
            comboMonth.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            comboMonth.DropDownStyle = ComboBoxStyle.DropDownList;
            comboMonth.FormattingEnabled = true;
            comboMonth.Location = new Point(153, 163);
            comboMonth.Name = "comboMonth";
            comboMonth.Size = new Size(1201, 49);
            comboMonth.TabIndex = 4;
            // 
            // lblGrade
            // 
            lblGrade.Anchor = AnchorStyles.Left;
            lblGrade.AutoSize = true;
            lblGrade.Location = new Point(3, 200);
            lblGrade.Name = "lblGrade";
            lblGrade.Size = new Size(98, 40);
            lblGrade.TabIndex = 5;
            lblGrade.Text = "Grade";
            // 
            // comboGrade
            // 
            comboGrade.Anchor = AnchorStyles.Left;
            comboGrade.DropDownStyle = ComboBoxStyle.DropDownList;
            comboGrade.FormattingEnabled = true;
            comboGrade.Location = new Point(153, 203);
            comboGrade.Name = "comboGrade";
            comboGrade.Size = new Size(70, 49);
            comboGrade.TabIndex = 5;
            // 
            // btnSubmit
            // 
            btnSubmit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSubmit.Location = new Point(1181, 412);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(93, 30);
            btnSubmit.TabIndex = 1;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.CausesValidation = false;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(1288, 412);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(93, 30);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // AddForm
            // 
            AcceptButton = btnSubmit;
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            CancelButton = btnCancel;
            ClientSize = new Size(1405, 466);
            Controls.Add(fieldLayout);
            Controls.Add(btnSubmit);
            Controls.Add(btnCancel);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add Student";
            Load += AddForm_Load;
            fieldLayout.ResumeLayout(false);
            fieldLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel fieldLayout;
        private Label lblStudId;
        private Label lblFirstName;
        private Label lblLastName;
        private Label lblAddress;
        private Label lblMonth;
        private Label lblGrade;
        private TextBox txtStudId;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtAddress;
        private ComboBox comboMonth;
        private ComboBox comboGrade;
        private Button btnSubmit;
        private Button btnCancel;
        private ErrorProvider errorProvider;
    }
}
