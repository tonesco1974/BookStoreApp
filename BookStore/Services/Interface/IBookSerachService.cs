using BookStore.Constants;
using BookStore.Models;

namespace BookStore.Services
{
    public interface IBookSerachService
    {
        List<BookViewModel> GetFavouritesBook();
        List<BookViewModel> GetSorteredBooks(EnumOrderBy order);
        List<BookViewModel> Process();
        List<BookViewModel> GetBooks(string search );
    }
}