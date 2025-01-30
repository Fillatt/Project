using APIClient;
using ConsoleApp;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;

namespace AvaloniaApplication
{
    public static class Services
    {
        #region Properties
        public static IServiceProvider Provider { get; set; }
        #endregion

        #region Constructors
        static Services()
        {
            var loggerConfiguration = new LoggerConfiguration()
               .MinimumLevel.Debug()
                  .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(@"Logs\Info.log"))
                  .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(@"Logs\Debug.log"))
                  .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(@"Logs\Warning.log"))
                  .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(@"Logs\Error.log"))
                  .CreateLogger();

            Configuration configuration = new("appsettings.json");

            Provider = new ServiceCollection()
                .AddSingleton(configuration)
                .AddSingleton<ILogger>(loggerConfiguration)
                .AddSingleton<NavigationService>()
                .AddDbContext<AccountContext>(options => options.UseSqlServer(configuration.GetAccountConnectionString()))   
                .AddDbContext<ApiRequestResultContext>(options => options.UseSqlServer(configuration.GetApiRequestResultConnectionString()))
                .AddTransient<Authorization>()
                .AddTransient<DbService>()
                .AddTransient<APIService>()
                .BuildServiceProvider();
        }
        #endregion
    }
}
