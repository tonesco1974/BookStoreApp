namespace BookStore.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Book> Books { get; set; } = new();
    }
}
