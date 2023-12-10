using finance_backend.DBModels;
using Microsoft.EntityFrameworkCore;

namespace finance_backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Users> Users { get; set; }
    
}