using Avalonia.Collections;
using AvaloniaApplication.Models;
using Figure;
using ReactiveUI;
using Serilog;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaApplication.ViewModels;
public class MainViewModel : ReactiveObject, IRoutableViewModel
{
    #region Private Fields   
    /// <summary>
    /// Объект модели предметной области
    /// </summary>
    private Model _model;

    /// <summary>
    /// Сообщение о ходе выполнения фигурами своих заданий
    /// </summary>
    private string _message;

    /// <summary>
    /// Список фигур
    /// </summary>
    private AvaloniaList<IFigure> _figures = [];

    /// <summary>
    /// Список сумм
    /// </summary>
    private AvaloniaList<DoubleValue> _amounts = [];

    /// <summary>
    /// Токен отмены выполнения программы
    /// </summary>
    private CancellationToken _tokenStart;

    /// <summary>
    /// Source для токена отмены выполнения программы
    /// </summary>
    private CancellationTokenSource _ctsStart;

    /// <summary>
    /// BehaviorSubject-флаг начала выполения программы
    /// </summary>
    private BehaviorSubject<bool> _isStart = new BehaviorSubject<bool>(false);
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
    public AvaloniaList<DoubleValue> Amounts
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
        _model = new();
        StartCommand = ReactiveCommand.CreateFromTask(StartAsync);
        StopCommand = ReactiveCommand.Create(Stop, _isStart);
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
        Log.Information("Program: Start");

        ClearTheOutput();
        _isStart.OnNext(true);
        InitTheToken();
        while (!_tokenStart.IsCancellationRequested)
        {
            GetAmounts();
            FiguresInit();
            await DoTasksAsync();
        }
        _isStart.OnNext(false);

        Log.Debug("MainViewModel.StartAsync: Done");
        Log.Information("Program: End");
    }

    /// <summary>
    /// Остановить выполнение программы
    /// </summary>
    private void Stop()
    {
        Log.Debug("MainViewModel.Stop: Start");

        CancelTheToken();

        Log.Debug("MainViewModel.Stop: Done");
        Log.Information("Program: Is stopped");
    }

    /// <summary>
    /// Вычислить суммы
    /// </summary>
    private void GetAmounts()
    {
        Log.Debug("MainViewModel.GetAmounts: Start");

        Amounts = _model.GetAmounts();

        Log.Debug("MainViewModel.GetAmounts: Done");
    }

    /// <summary>
    /// Инициализация фигур
    /// </summary>
    private void FiguresInit()
    {
        Log.Debug("MainViewModel.GetAmounts: Start");

        _figures = _model.FiguresInit();

        Log.Debug("MainViewModel.GetAmounts: Done");
    }

    /// <summary>
    /// Начать выполнение фигурами своих заданий
    /// </summary>
    /// <returns></returns>
    private async Task DoTasksAsync()
    {
        Log.Debug("MainViewModel.DoTasksAsync: Start");

        Message = string.Empty;
        foreach (var figure in _figures)
        {
            figure.StartTheMission();
            Message += figure.Message + '\n';
            try { await Task.Delay(_model.Sleep, _tokenStart); }
            catch (OperationCanceledException) { return; }
            figure.StopTheMission();
            Message += figure.Message + '\n';
        }

        Log.Debug("MainViewModel.DoTasksAsync: Done");
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

    /// <summary>
    /// Инициализация токена выполнения программы
    /// </summary>
    private void InitTheToken()
    {
        Log.Debug("MainViewModel.InitTheToken: Start");

        _ctsStart = new CancellationTokenSource();
        _tokenStart = _ctsStart.Token;

        Log.Debug("MainViewModel.InitTheToken: Done");
    }

    /// <summary>
    /// Отмена токена выполнения программы
    /// </summary>
    private void CancelTheToken()
    {
        Log.Debug("MainViewModel.CancelTheToken: Start");

        _ctsStart.Cancel();
        _ctsStart.Dispose();

        Log.Debug("MainViewModel.CancelTheToken: Done");
    }
    #endregion
}
