using Avalonia.Collections;
using AvaloniaApplication.Models;
using Figure;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace AvaloniaApplication.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private Model _model;

    private string _message;

    private AvaloniaList<IFigure> _figures = [];

    private AvaloniaList<DoubleValue> _amounts = [];

    public MainWindowViewModel()
    {
        _model = new();
        GetStartCommand = ReactiveCommand.CreateRunInBackground(StartCommandAsync);
    }

    public ReactiveCommand<Unit, Task> GetStartCommand { get; }

    public string Message
    {
        get => _message;
        set { this.RaiseAndSetIfChanged(ref _message, value); }
    }

    public AvaloniaList<DoubleValue> Amounts
    {
        get => _amounts;
        set { this.RaiseAndSetIfChanged(ref _amounts, value); }
    }

    private async Task StartCommandAsync()
    {
        while (true)
        {
            GetAmounts();
            FiguresInit();
            await DoTasksAsync();
        }
    }

    private void GetAmounts() => Amounts = _model.GetAmounts();

    private void FiguresInit() => _figures = _model.FiguresInit();

    private async Task DoTasksAsync()
    {   
        Message = string.Empty;
        foreach(var figure in _figures)
        {          
            figure.DoTheTask();
            Message += figure.Message;
            await Task.Delay(_model.Sleep);
        }
    }
}
