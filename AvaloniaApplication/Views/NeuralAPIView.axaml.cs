using Avalonia.ReactiveUI;
using AvaloniaApplication.ViewModels;

namespace AvaloniaApplication.Views;

public partial class NeuralAPIView : ReactiveUserControl<NeuralAPIViewModel>
{
    public NeuralAPIView()
    {
        InitializeComponent();
    }
}