using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Principal;

namespace BookStore.Models
{
    [Serializable]
    public class JsonFileViewModel
    {
        public object _id { get; set; }
        public string title { get; set; }
        public string isbn  { get; set; }
        public int pageCount { get; set; }
        public dynamic publishedDate { get; set; }
        public string thumbnailUrl { get; set; }
        public string shortDescription { get; set; }
        public string longDescription { get; set; }
        public string status { get; set; }
        public string[] authors { get; set; }
        public string[] categories  { get; set; }
    }
    [Serializable]
    public class IdObject
    {
        [JsonProperty("$oid")]
        public string oid { get; set; } = null!; 
    }
    [Serializable]
    public class DateObject
    {
        [JsonProperty("$date")]
        public DateTime? date { get; set; }
    }
}
