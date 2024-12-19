using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaApplication.ViewModels;
using AvaloniaApplication.Views;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Linq;
using System.Runtime.Intrinsics.Arm;

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
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();

                var mvm = new MainWindowViewModel();
                var lvm = new AuthenticationWindowViewModel();                

                var loginWindow = new AuthenticationWindow() { DataContext = lvm };               

                lvm.AuthenticationSuccess += EndAuthentication(loginWindow, mvm, desktop);                

                desktop.MainWindow = loginWindow;
                loginWindow.Show();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private EventHandler EndAuthentication(Window window, MainWindowViewModel mvm, IClassicDesktopStyleApplicationLifetime desktop)
        {
            return (sender, args) =>
            {
                var mainWindow = new MainWindow()
                {
                    DataContext = mvm
                };
                mainWindow.Show();
                window.Close();
                desktop.MainWindow = mainWindow;
            };
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