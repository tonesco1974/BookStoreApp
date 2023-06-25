using BookStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookStore.Services
{
    public class BookManageFavsService : IBookManageFavsService
    {
        private readonly ApplicationDBContext db;

        public BookManageFavsService(ApplicationDBContext db)
        {
            this.db = db;
        }

        public async Task Change(int Id)
        {
            var book = await db.Books.FirstOrDefaultAsync(x => x.Id == Id);
            if (book == null) { return; }
            book.IsFavorite = !book.IsFavorite;
            await db.SaveChangesAsync();
        }
    }
}
