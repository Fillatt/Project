using Avalonia.ReactiveUI;
using AvaloniaApplication.ViewModels;

namespace AvaloniaApplication.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
    }
}