using Avalonia.Collections;
using AvaloniaApplication.Models;
using Figure;
using ReactiveUI;
using System;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaApplication.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    #region Private Fields
    private Model _model;

    private string _message;

    private AvaloniaList<IFigure> _figures = [];

    private AvaloniaList<DoubleValue> _amounts = [];

    private CancellationToken _tokenStart;

    private CancellationTokenSource _ctsStart;
    #endregion

    public MainWindowViewModel()
    {
        _model = new();
        StartCommand = ReactiveCommand.CreateFromTask(StartAsync);
        StopCommand = ReactiveCommand.Create(Stop);
    }

    #region Properties
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
    #endregion

    #region Commands
    public ReactiveCommand<Unit, Unit> StartCommand { get; }

    public ReactiveCommand<Unit, Unit> StopCommand { get; }
    #endregion

    #region Private Methods
    private async Task StartAsync()
    {
        _ctsStart = new CancellationTokenSource();
        _tokenStart = _ctsStart.Token;
        while (!_tokenStart.IsCancellationRequested)
        {             
            GetAmounts();            
            FiguresInit();            
            await DoTasksAsync();           
        }
    }

    private void Stop()
    {
        _ctsStart.Cancel();
        _ctsStart.Dispose();
        Message = string.Empty;
        Amounts.Clear();
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
            try { await Task.Delay(_model.Sleep, _tokenStart); }
            catch (OperationCanceledException) { return; }
        }
    }
    #endregion
}
