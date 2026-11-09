namespace Caleb_Marks_Assignment_4._2
{
    public partial class LoginForm : Form
    {
        // Create login form
        public LoginForm()
        {
            InitializeComponent();
        }

        // Check login when button is clicked
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Validate teacher login
            if (txtUserId.Text != "Teacher" || txtPassword.Text != "Admin")
            {
                MessageBox.Show("The user ID or password is incorrect. Try again.", "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Clear();
                txtUserId.Focus();
                return;
            }

            txtPassword.Clear();
            // Hide login while teacher manages students
            Hide();
            using (MainForm mainForm = new MainForm())
            {
                mainForm.ShowDialog();
            }
            // Close startup form too, so no hidden application stays running
            Close();
        }
    }
}
