using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase;

public class ApiRequestResultContext : DbContext
{
    public DbSet<ApiRequestResult> ApiRequestsResults => Set<ApiRequestResult>();

    public ApiRequestResultContext(DbContextOptions<ApiRequestResultContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
