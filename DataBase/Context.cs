using Microsoft.EntityFrameworkCore;
using ConsoleApp;

namespace DataBase;

/// <summary>
/// Контекст базы данных
/// </summary>
public class Context : DbContext
{
    public DbSet<AccountDB> Accounts => Set<AccountDB>();

    public Context(DbContextOptions<Context> options) : base(options)
    {
        Database.EnsureCreated();        
    }    
}
