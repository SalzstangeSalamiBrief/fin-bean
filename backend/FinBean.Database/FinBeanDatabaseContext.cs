using FinBean.Database.Models;
using FinBean.Database.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FinBean.Database;

public class FinBeanDatabaseContext(IOptions<DatabaseOptions> databaseOptions): DbContext
{
    public DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(databaseOptions.Value.GetConnectionString());
    }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>()
            .HasKey(up => up.Id);
    }
}