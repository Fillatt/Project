using APIClient;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaApplication.Services;
using AvaloniaApplication.ViewModels;
using AvaloniaApplication.Views;
using ConsoleApp;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Serilog;
using Serilog.Events;
using Splat;
using System;
using System.Linq;

namespace AvaloniaApplication
{
    public partial class App : Application
    {
        public new static App? Current => Application.Current as App;

        public IServiceProvider? Services { get; private set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Debug()
                  .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(@"Logs\Info.log"))
                  .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(@"Logs\Debug.log"))
                  .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(@"Logs\Warning.log"))
                  .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(@"Logs\Error.log"))
                  .CreateLogger();

                Configuration configuration = new("appsettings.json");

                Services = new ServiceCollection()
                    .AddSingleton(configuration)
                    .AddSingleton<Serilog.ILogger>(loggerConfiguration)
                    .AddSingleton<NavigationService>()
                    .AddDbContext<Context>(options => options.UseSqlServer(configuration.GetConnectionString()))
                    .AddTransient<AuthorizationService>()
                    .AddTransient<ValidationService>()
                    .AddTransient<DbService>()
                    .AddTransient<JokeAPIService>()
                    .AddTransient<NeuralAPIService>()
                    .AddTransient<HealthCheckService>()
                    .AddTransient<IFilesService>(x => new FilesService(desktop.MainWindow))
                    .BuildServiceProvider();

                Locator.CurrentMutable.RegisterConstant<IScreen>(new MainWindowViewModel());
                Locator.CurrentMutable.Register(() => new LoginViewModel(), typeof(IRoutableViewModel), contract: "Login");
                Locator.CurrentMutable.Register(() => new RegisterViewModel(), typeof(IRoutableViewModel), contract: "Register");
                Locator.CurrentMutable.Register(() => new MainViewModel(), typeof(IRoutableViewModel), contract: "Main");
                Locator.CurrentMutable.Register(() => new JokeAPIViewModel(), typeof(IRoutableViewModel), contract: "JokeAPI");
                Locator.CurrentMutable.Register(() => new NeuralAPIViewModel(), typeof(IRoutableViewModel), contract: "NeuralAPI");

                DisableAvaloniaDataAnnotationValidation();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = Locator.Current.GetService<IScreen>()
                };

                Log.Logger = Services.GetRequiredService<Serilog.ILogger>();

                Services.GetRequiredService<NavigationService>().NavigateAuthentication();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}