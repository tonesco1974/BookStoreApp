using BookStore.Models;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BookStore.Services
{
    /// <summary>
    /// The Process method takes the file content as input and returns a list of JsonFileViewModel objects.
    ///The BookListGenerator method is a private helper method that performs the actual generation of the book list.
    ///It cleans the file content by replacing a specific pattern, splits the content into separate objects based on a delimiter(&&&), 
    ///deserializes each object into a JsonFileViewModel instance, and adds it to the output list.
    ///The ReplacePattern method is a static helper method that replaces a specific pattern(}\s +{) in the input string 
    ///with a replacement string(}&& &{). It uses regular expressions(Regex.Replace) for pattern matching and replacement.
    ///Overall, this class provides a service for processing file content and generating a list of book objects from the content.
    /// </summary>
    public class BookListGeneratorService : IBookListGeneratorService
{
    // Process the file content and generate a list of JsonFileViewModel objects
        public List<JsonFileViewModel>? Process(string fileContent)
        {
            if (string.IsNullOrEmpty(fileContent)) return null;
            return BookListGenerator(fileContent);
        }

        // Generate the book list from the file content
        private List<JsonFileViewModel> BookListGenerator(string fileContent)
        {
            var output = new List<JsonFileViewModel>();

            // Clean the file content by replacing a specific pattern
            var contentClean = ReplacePattern(fileContent);

            // Split the content into separate objects based on a delimiter
            var objectsInFile = contentClean.Split("&&&");

            // Deserialize each object and add it to the output list
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

        // Replace a specific pattern in the input string
        static string ReplacePattern(string input)
        {
            string pattern = @"}\s+{";
            string replacement = "}&&&{";
            string result = Regex.Replace(input, pattern, replacement);
            return result;
        }
    }

}
