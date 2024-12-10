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

    private ReactiveCommand<Unit, Unit> _getStartCommand;

    private AvaloniaList<IFigure> _figures = [];

    private AvaloniaList<DoubleValue> _amounts = [];

    public ReactiveCommand<Unit, Unit> GetStartCommand
    {
        get => _getStartCommand;
        set { this.RaiseAndSetIfChanged(ref _getStartCommand, value); }
    }

    public ReactiveCommand<Unit, Unit> GetRegisterCommand { get; }

    public ReactiveCommand<Unit, Unit> GetLoginCommand { get; }

    public string Login { get; set; }

    public string Password { get; set; }

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

    public MainWindowViewModel()
    {
        _model = new();
        GetLoginCommand = ReactiveCommand.CreateFromTask(LoginCommandAsync);
        GetRegisterCommand = ReactiveCommand.CreateFromTask(RegisterCommandAsync);
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
        foreach (var figure in _figures)
        {
            figure.DoTheTask();
            Message += figure.Message;
            await Task.Delay(_model.Sleep);
        }
    }

    private void SetStartCommand() => GetStartCommand = ReactiveCommand.CreateFromTask(StartCommandAsync);

    private async Task RegisterCommandAsync()
    {
        SetStartCommand();
    }

    private async Task LoginCommandAsync()
    {
        SetStartCommand();
    }
}
