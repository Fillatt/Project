using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaApplication.ViewModels;

namespace AvaloniaApplication.Views;

public partial class NeuralAPIPage : ReactiveUserControl<NeuralAPIViewModel>
{
    public NeuralAPIPage()
    {
        InitializeComponent();
    }
}