using Microsoft.EntityFrameworkCore;

namespace DataBase;

/// <summary>
/// Контекст базы данных
/// </summary>
public class Context : DbContext
{
    public DbSet<AccountDB> Accounts => Set<AccountDB>();

    public DbSet<ApiRequestResult> ApiRequests => Set<ApiRequestResult>();

    public Context(DbContextOptions<Context> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
