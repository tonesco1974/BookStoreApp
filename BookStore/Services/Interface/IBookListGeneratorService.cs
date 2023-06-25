using BookStore.Models;

namespace BookStore.Services
{
    public interface IBookListGeneratorService
    {
        List<JsonFileViewModel>? Process(string fileContent);
    }
}