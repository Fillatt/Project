using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaApplication.ViewModels;
using AvaloniaApplication.Views;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Serilog;
using Splat;
using System.Linq;

namespace AvaloniaApplication
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Регистрация моделей представления
                Locator.CurrentMutable.RegisterConstant<IScreen>(new MainWindowViewModel());
                Locator.CurrentMutable.Register(() => new LoginViewModel(), typeof(IRoutableViewModel), contract: "Login");
                Locator.CurrentMutable.Register(() => new RegisterViewModel(), typeof(IRoutableViewModel), contract: "Register");
                Locator.CurrentMutable.Register(() => new MainViewModel(), typeof(IRoutableViewModel), contract: "Main");
                Locator.CurrentMutable.Register(() => new APIViewModel(), typeof(IRoutableViewModel), contract: "API");

                Log.Logger = Services.Provider.GetRequiredService<Serilog.ILogger>();

                DisableAvaloniaDataAnnotationValidation();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = Locator.Current.GetService<IScreen>()
                };

                Services.Provider.GetRequiredService<NavigationService>().NavigateAuthentication();
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