using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    
}