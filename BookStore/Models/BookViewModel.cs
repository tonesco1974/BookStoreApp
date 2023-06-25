using BookStore.Entities;

namespace BookStore.Models
{
    [Serializable]
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Isbn { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? ThumbnailUrl { get; set; }
        public string? ShortDescription { get; set; }
        public string? Status { get; set; }
        public HashSet<Author> Authors { get; set; } = new();
        public HashSet<Category> Categories { get; set; } = new();
        public bool IsFavorite { get; set; } = false;
        public int PageCount { get; set; }
        public DateTime? PublishedDate { get; set; }
    }
}
