using Avalonia.ReactiveUI;
using AvaloniaApplication.ViewModels;

namespace AvaloniaApplication.Views;

public partial class LoginView : ReactiveUserControl<LoginViewModel>
{
    public LoginView()
    {
        InitializeComponent();
    }
}