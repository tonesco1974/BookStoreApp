using BookStore.Data;
using BookStore.Entities;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Data;

namespace BookStore.Services
{
    public class SaveFileToDataBaseService : ISaveFileToDataBaseService
    {
        private readonly ApplicationDBContext db;

        public SaveFileToDataBaseService(ApplicationDBContext db)
        {
            this.db = db;
        }

        public async Task Proccess(IEnumerable<JsonFileViewModel> model)
        {
            await GenerateCategoriesAsync(model);
            await GenerateAuthorsAsync(model);
            await GenerateBooksAsync(model);
        }

        private async Task GenerateBooksAsync(IEnumerable<JsonFileViewModel> model)
        {
            foreach (var bookJson in model.Where(b => !string.IsNullOrEmpty(b.isbn)))
            {
                try
                {
                    var bookExist = await db.Books.FirstOrDefaultAsync(b => b.Isbn == bookJson.isbn);
                    if (bookExist is null)
                    {
                        var book = new Book
                        {
                            Title = bookJson.title,
                            Isbn = bookJson.isbn,
                            LongDescription = bookJson.longDescription,
                            PageCount = bookJson.pageCount,
                            ShortDescription = bookJson.shortDescription,
                            Status = bookJson.status,
                            ThumbnailUrl = bookJson.thumbnailUrl,
                        };
                        GetExternalId(bookJson, book);
                        GetPublishedDate(bookJson, book);
                        GetCategories(bookJson, book);
                        GetAuthors(bookJson, book);
                        db.Books.Add(book);
                        await db.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private void GetAuthors(JsonFileViewModel? bookJson, Book book)
        {
            foreach (var aut in bookJson.authors)
            {
                if (!string.IsNullOrEmpty(aut))
                {
                    var author = db.Authors.FirstOrDefault(a => a.Name.Trim().ToLower() == aut.Trim().ToLower());
                    if (author != null)
                        book.Authors.Add(author);
                }
            }
        }

        private void GetCategories(JsonFileViewModel? bookJson, Book book)
        {
            if (bookJson == null) return;   

            foreach (var cat in bookJson.categories)
            {
                if (!string.IsNullOrEmpty(cat))
                {
                    var category = db.Categories.FirstOrDefault(c => c.Name.Trim().ToLower() == cat.Trim().ToLower());
                    if (category != null)
                        book.Categories.Add(category);
                }
            }
        }

        private static void GetPublishedDate(JsonFileViewModel? bookJson, Book book)
        {
            if (bookJson == null) return;
            if (bookJson.publishedDate is not null)
            {
                var dateObject = JsonConvert.DeserializeObject(bookJson.publishedDate.ToString());
                if (dateObject is DateTime)
                {
                    DateTime.TryParse(dateObject, out DateTime date);
                    book.PublishedDate = date;
                }
                else
                {
                    var iDateObject = JsonConvert.DeserializeObject<DateObject>(bookJson.publishedDate.ToString());
                    book.PublishedDate = iDateObject?.date;
                }
            }
        }

        private static void GetExternalId(JsonFileViewModel? bookJson, Book book)
        {
            if(bookJson == null) return;
            if (bookJson._id is not null)
            {
                var idObject = JsonConvert.DeserializeObject(bookJson._id.ToString());
                if (idObject is Int64)
                {
                    int.TryParse(idObject.ToString(), out int id);
                    book.ExternalId = id;
                }
                else
                {
                    var iObject = JsonConvert.DeserializeObject<IdObject>(idObject.ToString());
                    book.ExternalOid = iObject?.oid;
                }
            }
        }

        public string RemoveDollarSignFromJsonString(string jsonString)
        {
            JToken token = JToken.Parse(jsonString);

            if (token is JObject)
            {
                JObject jsonObject = (JObject)token;
                RemoveDollarSignFromJObject(jsonObject);
                return jsonObject.ToString();
            }

            return jsonString;
        }

        private void RemoveDollarSignFromJObject(JObject jsonObject)
        {
            foreach (JProperty property in jsonObject.Properties().ToList())
            {
                if (property.Name.StartsWith("$"))
                {
                    property.Remove();
                }

                if (property.Value is JObject nestedObject)
                {
                    RemoveDollarSignFromJObject(nestedObject);
                }
            }
        }

        private async Task GenerateAuthorsAsync(IEnumerable<JsonFileViewModel> model)
        {
            List<string> UniqueAuthors = model
           .SelectMany(book => book.authors.Where(a => !string.IsNullOrWhiteSpace(a)))
           .Distinct()
           .ToList();

            foreach (var author in UniqueAuthors)
            {
                if (!db.Authors.Any(c => c.Name.Trim().ToLower() == author.Trim().ToLower()))
                {
                    await db.Authors.AddAsync(new Author() { Name = author });
                    await db.SaveChangesAsync();
                }
            }
        }

        private async Task GenerateCategoriesAsync(IEnumerable<JsonFileViewModel> model)
        {
            List<string> UniqueCategories = model
                        .SelectMany(book => book.categories.Where(c => !string.IsNullOrWhiteSpace(c)))
                        .Distinct()
                        .ToList();

            foreach (var category in UniqueCategories)
            {
                if (!db.Categories.Any(c => c.Name.Trim().ToLower() == category.Trim().ToLower()))
                {
                    await db.Categories.AddAsync(new Category() { Name = category });
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
