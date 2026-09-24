namespace Caleb_Marks_Assignment_4._1._2
{
    public partial class MainForm : Form
    {
        // Create one Math calculator and store it as an ICalculator
        private readonly ICalculator calculator = new Math();

        public MainForm()
        {
            InitializeComponent();
        }

        private void Calculate(string operation)
        {
            txtResult.Clear();
            decimal first;
            decimal second;

            // Turn both text entries into numbers before doing any math
            if (!Decimal.TryParse(txtFirst.Text, out first))
            {
                ShowProblem("Enter a valid first number.", txtFirst);
                return;
            }
            if (!Decimal.TryParse(txtSecond.Text, out second))
            {
                ShowProblem("Enter a valid second number.", txtSecond);
                return;
            }

            try
            {
                decimal result;

                // Match the button that was clicked to the correct math method
                switch (operation)
                {
                    case "Add":
                        result = calculator.Add(first, second);
                        break;
                    case "Subtract":
                        result = calculator.Subtract(first, second);
                        break;
                    case "Multiply":
                        result = calculator.Multiply(first, second);
                        break;
                    case "Divide":
                        result = calculator.Divide(first, second);
                        break;
                    default:
                        return;
                }
                txtResult.Text = result.ToString();
            }
            catch (DivideByZeroException)
            {
                ShowProblem("The second number cannot be zero when dividing.", txtSecond);
            }
            catch (OverflowException)
            {
                ShowProblem("The result is too large. Enter smaller numbers.", txtFirst);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Calculate("Add");
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            Calculate("Subtract");
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            Calculate("Multiply");
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            Calculate("Divide");
        }

        private void Input_TextChanged(object sender, EventArgs e)
        {
            // Clear the answer because it no longer matches the new numbers
            txtResult.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFirst.Clear();
            txtSecond.Clear();
            txtResult.Clear();
            txtFirst.Focus();
        }

        private static void ShowProblem(string message, Control input)
        {
            // Show the warning, then place the cursor back in that box
            MessageBox.Show(message, "Check Your Numbers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            input.Focus();
        }
    }
}
