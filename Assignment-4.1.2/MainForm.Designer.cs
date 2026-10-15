namespace Caleb_Marks_Assignment_4._1._2
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblFirst = new Label();
            txtFirst = new TextBox();
            lblSecond = new Label();
            txtSecond = new TextBox();
            btnAdd = new Button();
            btnSubtract = new Button();
            btnMultiply = new Button();
            btnDivide = new Button();
            lblResult = new Label();
            txtResult = new TextBox();
            btnClear = new Button();
            SuspendLayout();

            lblFirst.Name = "lblFirst";
            lblFirst.Location = new Point(24, 24);
            lblFirst.Size = new Size(440, 22);
            lblFirst.Text = "First number";
            lblFirst.TabIndex = 0;

            txtFirst.Name = "txtFirst";
            txtFirst.Location = new Point(24, 50);
            txtFirst.Size = new Size(440, 27);
            txtFirst.TextChanged += Input_TextChanged;
            txtFirst.TabIndex = 0;

            lblSecond.Name = "lblSecond";
            lblSecond.Location = new Point(24, 92);
            lblSecond.Size = new Size(440, 22);
            lblSecond.Text = "Second number";
            lblSecond.TabIndex = 1;

            txtSecond.Name = "txtSecond";
            txtSecond.Location = new Point(24, 118);
            txtSecond.Size = new Size(440, 27);
            txtSecond.TextChanged += Input_TextChanged;
            txtSecond.TabIndex = 1;

            btnAdd.Name = "btnAdd";
            btnAdd.Location = new Point(24, 164);
            btnAdd.Size = new Size(104, 38);
            btnAdd.Text = "Add +";
            btnAdd.Click += btnAdd_Click;
            btnAdd.TabIndex = 2;

            btnSubtract.Name = "btnSubtract";
            btnSubtract.Location = new Point(136, 164);
            btnSubtract.Size = new Size(104, 38);
            btnSubtract.Text = "Subtract -";
            btnSubtract.Click += btnSubtract_Click;
            btnSubtract.TabIndex = 3;

            btnMultiply.Name = "btnMultiply";
            btnMultiply.Location = new Point(248, 164);
            btnMultiply.Size = new Size(104, 38);
            btnMultiply.Text = "Multiply *";
            btnMultiply.Click += btnMultiply_Click;
            btnMultiply.TabIndex = 4;

            btnDivide.Name = "btnDivide";
            btnDivide.Location = new Point(360, 164);
            btnDivide.Size = new Size(104, 38);
            btnDivide.Text = "Divide /";
            btnDivide.Click += btnDivide_Click;
            btnDivide.TabIndex = 5;

            lblResult.Name = "lblResult";
            lblResult.Location = new Point(24, 226);
            lblResult.Size = new Size(440, 22);
            lblResult.Text = "Result";
            lblResult.TabIndex = 6;

            txtResult.Name = "txtResult";
            txtResult.Location = new Point(24, 252);
            txtResult.Size = new Size(440, 27);
            txtResult.ReadOnly = true;
            txtResult.TabIndex = 6;

            btnClear.Name = "btnClear";
            btnClear.Location = new Point(24, 304);
            btnClear.Size = new Size(104, 36);
            btnClear.Text = "Clear";
            btnClear.Click += btnClear_Click;
            btnClear.TabIndex = 7;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(490, 365);
            Text = "Calculator - Assignment 4.1.2";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Controls.Add(lblFirst);
            Controls.Add(txtFirst);
            Controls.Add(lblSecond);
            Controls.Add(txtSecond);
            Controls.Add(btnAdd);
            Controls.Add(btnSubtract);
            Controls.Add(btnMultiply);
            Controls.Add(btnDivide);
            Controls.Add(lblResult);
            Controls.Add(txtResult);
            Controls.Add(btnClear);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblFirst = null!;
        private TextBox txtFirst = null!;
        private Label lblSecond = null!;
        private TextBox txtSecond = null!;
        private Button btnAdd = null!;
        private Button btnSubtract = null!;
        private Button btnMultiply = null!;
        private Button btnDivide = null!;
        private Label lblResult = null!;
        private TextBox txtResult = null!;
        private Button btnClear = null!;
    }
}
