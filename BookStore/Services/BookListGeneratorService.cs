using BookStore.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BookStore.Services
{
    public class BookListGeneratorService : IBookListGeneratorService
    {
        public List<JsonFileViewModel>? Process(string fileContent)
        {
            if (string.IsNullOrEmpty(fileContent)) return null;
            return BookListGenerator(fileContent);
        }

        private List<JsonFileViewModel> BookListGenerator(string fileContent)
        {
            var output = new List<JsonFileViewModel>();
            var contentClean = ReplacePattern(fileContent);
            var objectsInFile = contentClean.Split("&&&");
            foreach (var obj in objectsInFile)
            {
                var book = JsonSerializer.Deserialize<JsonFileViewModel>(obj);
                if (book is not null)
                {
                    output.Add(book);
                }

            }
            return output;
        }

        static string ReplacePattern(string input)
        {
            string pattern = @"}\s+{";
            string replacement = "}&&&{";
            string result = Regex.Replace(input, pattern, replacement);
            return result;
        }
    }
}
