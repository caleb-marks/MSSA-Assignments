using Caleb_Marks_Assignment_11._1.Data;
using Caleb_Marks_Assignment_11._1.Models;

namespace Caleb_Marks_Assignment_11._1.Services
{
    // Create, read, update, & delete books; form never touches context
    public class CRUD
    {
        // Return every book
        public List<Book> GetBooks()
        {
            using (Ccad23bookInventoryContext context = new Ccad23bookInventoryContext())
            {
                return context.Books.OrderBy(x => x.Name).ToList();
            }
        }

        // Find one book; null when missing
        public Book? GetBook(string isbn)
        {
            using (Ccad23bookInventoryContext context = new Ccad23bookInventoryContext())
            {
                return context.Books.Find(isbn);
            }
        }

        // Add book & save database
        public void AddBook(Book book)
        {
            using (Ccad23bookInventoryContext context = new Ccad23bookInventoryContext())
            {
                context.Books.Add(book);
                context.SaveChanges();
            }
        }

        // Copy changed fields; ISBN stays same
        public void UpdateBook(Book changedBook)
        {
            using (Ccad23bookInventoryContext context = new Ccad23bookInventoryContext())
            {
                Book? existingBook = context.Books.Find(changedBook.Isbn);
                if (existingBook != null)
                {
                    existingBook.Name = changedBook.Name;
                    existingBook.AuthorName = changedBook.AuthorName;
                    existingBook.Description = changedBook.Description;
                    existingBook.Quantity = changedBook.Quantity;
                    context.SaveChanges();
                }
            }
        }

        // Remove matching book when found
        public void DeleteBook(string isbn)
        {
            using (Ccad23bookInventoryContext context = new Ccad23bookInventoryContext())
            {
                Book? book = context.Books.Find(isbn);
                if (book != null)
                {
                    context.Books.Remove(book);
                    context.SaveChanges();
                }
            }
        }
    }
}
