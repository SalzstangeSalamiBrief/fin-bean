using Microsoft.EntityFrameworkCore;

namespace FinBean.Database;

/// <summary>
/// The EF Core database context for fin-bean, backed by PostgreSQL.
/// Domain entity <see cref="DbSet{TEntity}"/> properties will be added here as later features introduce data.
/// </summary>
public class FinBeanDbContext : DbContext
{
    public FinBeanDbContext(DbContextOptions<FinBeanDbContext> options)
        : base(options)
    {
    }
}
