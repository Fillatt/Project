using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaApplication.ViewModels;
namespace AvaloniaApplication.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}