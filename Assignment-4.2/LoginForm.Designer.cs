namespace Caleb_Marks_Assignment_4._2
{
    partial class LoginForm
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
            // Create labels, input boxes, & login button
            lblUserId = new Label();
            txtUserId = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnLogin = new Button();
            SuspendLayout();

            // Configure user ID label & input
            lblUserId.Name = "lblUserId";
            lblUserId.Location = new Point(24, 24);
            lblUserId.Size = new Size(320, 22);
            lblUserId.Text = "User ID";
            lblUserId.TabIndex = 0;

            txtUserId.Name = "txtUserId";
            txtUserId.Location = new Point(24, 50);
            txtUserId.Size = new Size(320, 27);
            txtUserId.TabIndex = 0;

            // Configure password label & hide typed characters
            lblPassword.Name = "lblPassword";
            lblPassword.Location = new Point(24, 92);
            lblPassword.Size = new Size(320, 22);
            lblPassword.Text = "Password";
            lblPassword.TabIndex = 1;

            txtPassword.Name = "txtPassword";
            txtPassword.Location = new Point(24, 118);
            txtPassword.Size = new Size(320, 27);
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TabIndex = 1;

            // Assign login button's click method
            btnLogin.Name = "btnLogin";
            btnLogin.Location = new Point(24, 172);
            btnLogin.Size = new Size(320, 38);
            btnLogin.Text = "Log In";
            btnLogin.Click += btnLogin_Click;
            btnLogin.TabIndex = 2;

            // Set window size, title, & starting position
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(370, 240);
            Text = "Teacher Login - Assignment 4.2";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            // Add controls & let Enter click login button
            Controls.Add(lblUserId);
            Controls.Add(txtUserId);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            AcceptButton = btnLogin;
            ResumeLayout(false);
            PerformLayout();
        }

        // Store login form controls
        private Label lblUserId = null!;
        private TextBox txtUserId = null!;
        private Label lblPassword = null!;
        private TextBox txtPassword = null!;
        private Button btnLogin = null!;
    }
}
