using Avalonia.ReactiveUI;
using AvaloniaApplication.ViewModels;

namespace AvaloniaApplication.Views;

public partial class JokeAPIView : ReactiveUserControl<JokeAPIViewModel>
{
    public JokeAPIView()
    {
        InitializeComponent();
    }
}