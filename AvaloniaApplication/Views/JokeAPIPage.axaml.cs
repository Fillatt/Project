using Avalonia.ReactiveUI;
using AvaloniaApplication.ViewModels;

namespace AvaloniaApplication.Views;

public partial class JokeAPIPage : ReactiveUserControl<JokeAPIViewModel>
{
    public JokeAPIPage()
    {
        InitializeComponent();
    }
}