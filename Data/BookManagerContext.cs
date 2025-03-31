using Microsoft.EntityFrameworkCore;

using BookManager.Entities;

namespace BookManager.Data;

public class BookManagerContext(DbContextOptions<BookManagerContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasGeneratedTsVectorColumn(
                p => p.SearchVector!,
                "english",  // Text search config
                p => new { p.Title, p.Author })  // Included properties
            .HasIndex(p => p.SearchVector)
            .HasMethod("GIN"); // Index method on the search vector (GIN or GIST)
    }
}