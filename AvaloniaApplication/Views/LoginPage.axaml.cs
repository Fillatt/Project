using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaApplication.ViewModels;

namespace AvaloniaApplication.Views;

public partial class LoginPage : ReactiveUserControl<LoginViewModel>
{
    public LoginPage()
    {
        InitializeComponent();
    }
}