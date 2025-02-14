using assignment1.Models;
using Microsoft.EntityFrameworkCore;

namespace assignment1.Data;

public class ApplicationDBContext : DbContext
{

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
        
    }
    
    public DbSet<Product> Products { get; set; }
    
    
}