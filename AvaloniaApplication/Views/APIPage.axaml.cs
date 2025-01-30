using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaApplication.ViewModels;

namespace AvaloniaApplication.Views;

public partial class APIPage : ReactiveUserControl<APIViewModel>
{
    public APIPage()
    {
        AvaloniaXamlLoader.Load(this);
    }
}