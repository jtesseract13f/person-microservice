using Microsoft.EntityFrameworkCore;

namespace PersonMinimalApi;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Person> Persons { get; set; }
    
    
}