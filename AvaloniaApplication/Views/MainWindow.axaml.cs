using Avalonia.ReactiveUI;
using AvaloniaApplication.ViewModels;
namespace AvaloniaApplication.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}