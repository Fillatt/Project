using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaApplication.ViewModels;

namespace AvaloniaApplication.Views;

public partial class RegisterPage : ReactiveUserControl<RegisterViewModel>
{
    public RegisterPage()
    {
        AvaloniaXamlLoader.Load(this);
    }
}