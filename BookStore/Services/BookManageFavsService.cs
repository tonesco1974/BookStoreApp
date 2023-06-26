using BookStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookStore.Services
{
    /// <summary>
    /// Manage the favourite status of a book 
    /// </summary>
    public class BookManageFavsService : IBookManageFavsService
    {
        private readonly ApplicationDBContext db;

        // Constructor injection of the ApplicationDBContext
        public BookManageFavsService(ApplicationDBContext db)
        {
            this.db = db;
        }

        // Change the favorite status of a book with the given Id
        public async Task Change(int Id)
        {
            // Find the book with the specified Id
            var book = await db.Books.FirstOrDefaultAsync(x => x.Id == Id);

            // If the book doesn't exist, return without making any changes
            if (book == null) { return; }

            // Toggle the IsFavorite property of the book
            book.IsFavorite = !book.IsFavorite;

            // Save the changes to the database
            await db.SaveChangesAsync();
        }
    }

}
