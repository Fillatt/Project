using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaApplication.ViewModels;

namespace AvaloniaApplication.Views;

public partial class MainPage : ReactiveUserControl<MainViewModel>
{
    public MainPage()
    {
        InitializeComponent();
    }
}