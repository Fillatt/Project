using Microsoft.EntityFrameworkCore;

namespace DataBase;

/// <summary>
/// Контекст базы данных
/// </summary>
public class AccountContext : DbContext
{
    public DbSet<AccountDB> Accounts => Set<AccountDB>();

    public AccountContext(DbContextOptions<AccountContext> options) : base(options)
    {
        Database.EnsureCreated();        
    }    
}
