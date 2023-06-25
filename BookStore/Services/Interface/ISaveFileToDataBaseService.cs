using BookStore.Models;

namespace BookStore.Services
{
    public interface ISaveFileToDataBaseService
    {
        Task Proccess(IEnumerable<JsonFileViewModel> model);
        string RemoveDollarSignFromJsonString(string jsonString);
    }
}