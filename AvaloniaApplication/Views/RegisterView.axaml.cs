using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaApplication.ViewModels;

namespace AvaloniaApplication.Views;

public partial class RegisterView : ReactiveUserControl<RegisterViewModel>
{
    public RegisterView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}