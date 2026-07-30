using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FinBean.Database;

/// <summary>
/// Enables EF Core CLI tooling (e.g. "dotnet ef migrations add") to create a <see cref="FinBeanDbContext"/>
/// at design time, without booting the FinBean.Api host. Reads the connection string from the
/// ConnectionStrings__FinBeanDb environment variable, falling back to the local docker-compose default.
/// </summary>
public sealed class FinBeanDbContextFactory : IDesignTimeDbContextFactory<FinBeanDbContext>
{
    public FinBeanDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__FinBeanDb")
            ?? "Host=localhost;Port=5432;Database=finbean;Username=finbean;Password=changeme-local-dev";

        var optionsBuilder = new DbContextOptionsBuilder<FinBeanDbContext>()
            .UseNpgsql(connectionString);

        return new FinBeanDbContext(optionsBuilder.Options);
    }
}
