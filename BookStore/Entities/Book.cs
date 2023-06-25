namespace BookStore.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public int ExternalId { get; set; }
        public string Isbn { get; set; } = null!; 
        public string? ExternalOid { get; set; }
        public string Title { get; set; } = null!;
        public int PageCount { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string?  ThumbnailUrl { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription  { get; set; }
        public string? Status { get; set; }
        public HashSet<Author> Authors { get; set; } = new();
        public HashSet<Category> Categories { get; set; } = new();
        public bool IsFavorite { get; set; } = false; 
    }
}
