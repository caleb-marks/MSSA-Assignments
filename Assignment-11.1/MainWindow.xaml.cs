using System.Windows;
using Caleb_Marks_Assignment_11._1.Models;
using Caleb_Marks_Assignment_11._1.Services;

namespace Caleb_Marks_Assignment_11._1
{
    public partial class MainWindow : Window
    {
        CRUD crud = new CRUD();

        // Build window & attach Loaded event
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        // Show books; keep Update off
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshGrid();
            btnUpdate.IsEnabled = false;
        }

        // Reload grid rows; show database problems
        private void RefreshGrid()
        {
            try
            {
                BookGrid.ItemsSource = crud.GetBooks();
            }
            catch (Exception error)
            {
                MessageBox.Show("The book database could not be opened. Run Database\\CreateBookInventory.sql first.\n\n" + error.Message);
            }
        }

        // Empty form & return Add mode
        private void ResetForm()
        {
            txtIsbn.Clear();
            txtName.Clear();
            txtAuthor.Clear();
            txtDescription.Clear();
            txtQuantity.Clear();
            txtIsbn.IsReadOnly = false;
            btnAdd.IsEnabled = true;
            btnUpdate.IsEnabled = false;
            BookGrid.SelectedIndex = -1;
        }

        // Validate input, reject duplicate ISBN, & add book
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Book? book = ReadBook();
            if (book == null)
            {
                return;
            }

            try
            {
                if (crud.GetBook(book.Isbn) != null)
                {
                    MessageBox.Show("A book with ISBN " + book.Isbn + " already exists.");
                    return;
                }

                crud.AddBook(book);
                RefreshGrid();
                ResetForm();
            }
            catch (Exception error)
            {
                MessageBox.Show("The book could not be added.\n\n" + error.Message);
            }
        }

        // Fill form using selected row; lock primary key
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            Book? book = BookGrid.SelectedItem as Book;
            if (book == null)
            {
                MessageBox.Show("Select a book in the grid first.");
                return;
            }

            txtIsbn.Text = book.Isbn;
            txtName.Text = book.Name;
            txtAuthor.Text = book.AuthorName;
            txtDescription.Text = book.Description;
            txtQuantity.Text = book.Quantity.ToString();
            txtIsbn.IsReadOnly = true;
            btnAdd.IsEnabled = false;
            btnUpdate.IsEnabled = true;
        }

        // Validate & save selected book changes
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Book? book = ReadBook();
            if (book == null)
            {
                return;
            }

            try
            {
                crud.UpdateBook(book);
                RefreshGrid();
                ResetForm();
            }
            catch (Exception error)
            {
                MessageBox.Show("The book could not be updated.\n\n" + error.Message);
            }
        }

        // Confirm & delete selected book
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Book? book = BookGrid.SelectedItem as Book;
            if (book == null)
            {
                MessageBox.Show("Select a book in the grid first.");
                return;
            }

            MessageBoxResult answer = MessageBox.Show("Delete " + book.Name + "?", "Confirm Delete", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.Yes)
            {
                try
                {
                    crud.DeleteBook(book.Isbn);
                    RefreshGrid();
                    ResetForm();
                }
                catch (Exception error)
                {
                    MessageBox.Show("The book could not be deleted.\n\n" + error.Message);
                }
            }
        }

        // Clear form; nothing saved
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        // Build book using form values; invalid input shows message & returns null
        private Book? ReadBook()
        {
            string isbn = txtIsbn.Text.Replace("-", "").Trim().ToUpper();
            string name = txtName.Text.Trim();
            string author = txtAuthor.Text.Trim();
            int quantity;

            if (!IsValidIsbn(isbn))
            {
                MessageBox.Show("Enter an ISBN with 10 or 13 digits. An ISBN-10 can end in X.");
                return null;
            }

            // Require book name & author; lengths match table columns
            if (name.Length == 0 || name.Length > 200 || author.Length == 0 || author.Length > 200)
            {
                MessageBox.Show("Enter a name and author name, each 200 characters or fewer.");
                return null;
            }

            if (!Int32.TryParse(txtQuantity.Text, out quantity) || quantity < 0)
            {
                MessageBox.Show("Enter a quantity of 0 or more.");
                return null;
            }

            Book book = new Book();
            book.Isbn = isbn;
            book.Name = name;
            book.AuthorName = author;
            book.Description = txtDescription.Text.Trim();
            book.Quantity = quantity;

            return book;
        }

        // Check ISBN length & digits; ISBN-10 allows ending X
        private bool IsValidIsbn(string isbn)
        {
            if (isbn.Length != 10 && isbn.Length != 13)
            {
                return false;
            }

            for (int i = 0; i < isbn.Length; i++)
            {
                bool isDigit = isbn[i] >= '0' && isbn[i] <= '9';
                bool allowedX = isbn.Length == 10 && i == 9 && isbn[i] == 'X';
                if (!isDigit && !allowedX)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
