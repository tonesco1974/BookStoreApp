﻿using AutoMapper;
using BookStore.Constants;
using BookStore.Data;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services
{
    public class BookSerachService : IBookSerachService
    {
        private readonly ApplicationDBContext db;
        private readonly IMapper map;

        // Constructor injection of the ApplicationDBContext and IMapper
        public BookSerachService(ApplicationDBContext db, IMapper map)
        {
            this.db = db;
            this.map = map;
        }

        // Retrieve all books with their related entities
        public List<BookViewModel> Process()
        {
            var books = db.Books
                        .Include(b => b.Authors)
                        .Include(b => b.Categories)
                        .ToList();
            var booksVM = books.ConvertAll(map.Map<BookViewModel>);

            return booksVM;
        }

        // Retrieve all favorite books with their related entities
        public List<BookViewModel> GetFavouritesBook()
        {
            var books = db.Books
                        .Include(b => b.Authors)
                        .Include(b => b.Categories)
                        .Where(b => b.IsFavorite)
                        .ToList();
            var booksVM = books.ConvertAll(map.Map<BookViewModel>);
            return booksVM;
        }

        // Retrieve books ordered by title
        private List<BookViewModel> GetBooksOrdeByTitle()
        {
            var books = db.Books
                        .Include(b => b.Authors)
                        .Include(b => b.Categories)
                        .OrderBy(b => b.Title)
                        .ToList();
            var booksVM = books.ConvertAll(map.Map<BookViewModel>);
            return booksVM;
        }

        // Retrieve books ordered by published date
        private List<BookViewModel> GetBooksOrderByPublishedDate()
        {
            var books = db.Books
                        .Include(b => b.Authors)
                        .Include(b => b.Categories)
                        .OrderByDescending(b => b.PublishedDate)
                        .ToList();
            var booksVM = books.ConvertAll(map.Map<BookViewModel>);
            return booksVM;
        }

        // Get sorted books based on the specified order
        public List<BookViewModel> GetSorteredBooks(EnumOrderBy order)
        {
            if (order == EnumOrderBy.Title) return GetBooksOrdeByTitle();
            if (order == EnumOrderBy.PublishDate) return GetBooksOrderByPublishedDate();

            return Process();
        }

        // Search for books based on the provided search string
        public List<BookViewModel> GetBooks(string search)
        {
            var books = db.Books
                   .Where(b => b.Title.ToLower().Contains(search)
                                    || b.Isbn.Contains(search)
                                    || b.Authors.Any(b => b.Name.ToLower().Contains(search)))
                   .ToList();
            var booksVM = books.ConvertAll(map.Map<BookViewModel>);
            return booksVM;
        }
    }

}
