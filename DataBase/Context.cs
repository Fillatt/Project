using Microsoft.EntityFrameworkCore;
using ConsoleApp;

namespace DataBase;

/// <summary>
/// Контекст базы данных
/// </summary>
public class Context : DbContext
{
    public DbSet<AccountDB> Accounts => Set<AccountDB>();

    public Context() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = ConsoleApp.Configuration.ReadFromConfiguration("ConnectionString");
        optionsBuilder.UseSqlServer(connectionString);
    }
}
