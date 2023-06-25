using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Entities.Configurations
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(p => p.PublishedDate).HasColumnType("date");
            builder.Property(p => p.Isbn).IsRequired(); 
            builder.Property(p => p.Title).IsRequired();
            builder.HasIndex(p => p.Title); 
            builder.HasIndex(p=>p.Isbn).IsUnique();
            builder.Property(p => p.LongDescription).HasMaxLength(5000);
            builder.Property(p=>p.ShortDescription).HasMaxLength(5000);
        }
    }
}
