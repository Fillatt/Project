using Avalonia.Collections;
using AvaloniaApplication.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Serilog;
using Splat;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace AvaloniaApplication.ViewModels;
public class MainViewModel : ReactiveObject, IRoutableViewModel
{
    #region Private Fields   
    /// <summary>
    /// Сообщение о ходе выполнения фигурами своих заданий
    /// </summary>
    private string _message;

    private string _titleConnectionError;

    /// <summary>
    /// Список сумм
    /// </summary>
    private AvaloniaList<DoubleValueRecord> _amounts = [];

    /// <summary>
    /// BehaviorSubject-флаг начала выполения программы
    /// </summary>
    private BehaviorSubject<bool> _isStart = new BehaviorSubject<bool>(false);

    SignalRClientModelService _modelService;
    #endregion

    #region Properties
    /// <summary>
    /// Ссылка на IScreen, которому принадлежит IRotableViewModel
    /// </summary>
    public IScreen HostScreen { get; }

    /// <summary>
    /// Идентификатор для IRotableViewModel
    /// </summary>
    public string UrlPathSegment { get; } = "MainViewModel";

    public string TitleConnectionError
    {
        get => _titleConnectionError;
        set { this.RaiseAndSetIfChanged(ref _titleConnectionError, value); }
    }

    /// <summary>
    /// Сообщение о ходе выполнения фигурами своих заданий
    /// </summary>
    public string Message
    {
        get => _message;
        set { this.RaiseAndSetIfChanged(ref _message, value); }
    }

    /// <summary>
    /// Список сумм
    /// </summary>
    public AvaloniaList<DoubleValueRecord> Amounts
    {
        get => _amounts;
        set { this.RaiseAndSetIfChanged(ref _amounts, value); }
    }
    #endregion

    #region Commands

    /// <summary>
    /// Начать выполнение программы
    /// </summary>
    public ReactiveCommand<Unit, Unit> StartCommand { get; }

    /// <summary>
    /// Остановить программу
    /// </summary>
    public ReactiveCommand<Unit, Unit> StopCommand { get; }
    #endregion

    #region Constructors
    public MainViewModel(IScreen screen = null)
    {
        HostScreen = screen ?? Locator.Current.GetService<IScreen>();

        _modelService = App.Current.Services.GetRequiredService<SignalRClientModelService>();
        _modelService.AmountsSubject.Subscribe(ReceiveAmounts);
        _modelService.MessageSubject.Subscribe(ReceiveMessage);
        _modelService.IsConnectedSubject.Subscribe(OnConnectionChange);

        StartCommand = ReactiveCommand.CreateFromTask(StartAsync, _modelService.IsConnectedSubject);
        StopCommand = ReactiveCommand.CreateFromTask(StopAsync, _isStart);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Начать выполнение программы
    /// </summary>
    /// <returns></returns>
    private async Task StartAsync()
    {
        Log.Debug("MainViewModel.StartAsync: Start");
        Log.Information("Main Program: Start");

        ClearTheOutput();
        _isStart.OnNext(true);

        await _modelService.StartModelAsync();

        Log.Debug("MainViewModel.StartAsync: Done");
        Log.Information("Main Program: End");
    }

    /// <summary>
    /// Остановить выполнение программы
    /// </summary>
    private async Task StopAsync()
    {
        Log.Debug("MainViewModel.Stop: Start");

        await _modelService.StopModelAsync();
        _isStart.OnNext(false);

        Log.Debug("MainViewModel.Stop: Done");
        Log.Information("Main Program: Is stopped");
    }

    /// <summary>
    /// Очистить выведенную в UI информацию
    /// </summary>
    private void ClearTheOutput()
    {
        Log.Debug("MainViewModel.ClearTheOutput: Start");

        Message = string.Empty;
        Amounts.Clear();

        Log.Debug("MainViewModel.ClearTheOutput: Done");
    }

    private void ReceiveAmounts(List<double> amounts)
    {
        AvaloniaList<DoubleValueRecord> doubleValues = new();
        foreach (var amount in amounts) doubleValues.Add(new DoubleValueRecord(amount));
        Amounts = doubleValues;
    }

    private void ReceiveMessage(string message)
    {
        Message = message;
    }

    private void OnConnectionChange(bool isConnected)
    {
        if (isConnected) TitleConnectionError = string.Empty;
        else TitleConnectionError = "Connection is lost";
    }
    #endregion
}
