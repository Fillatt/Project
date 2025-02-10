using APIClient;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using AvaloniaApplication.Services;
using ConsoleApp;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Serilog;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaApplication.ViewModels;

public class NeuralAPIViewModel : ReactiveObject, IRoutableViewModel
{
    #region Private Fields
    private string _urlPath;

    private NeuralAPIService _apiService;

    private HealthCheckService _healthCheckService;

    private IFilesService _filesService;

    private CancellationTokenSource _checkStopTokenSource;

    private Bitmap? _imageFileBitmap;

    private IStorageFile? _imageFile;

    private NeuralAPIResponse _response;

    private string _connectionIndicatorColor;
    #endregion

    #region Properties
    /// <summary>
    /// Ссылка на IScreen, которому принадлежит данная модель представления
    /// </summary>
    public IScreen HostScreen { get; }

    /// <summary>
    /// Идентификатор для IRotableViewModel
    /// </summary>
    public string UrlPathSegment { get; } = "NeuralViewModel";

    public string UrlPath
    {
        get => _urlPath;
        set { this.RaiseAndSetIfChanged(ref _urlPath, value); }
    }

    public Bitmap? ImageFile
    {
        get => _imageFileBitmap;
        set { this.RaiseAndSetIfChanged(ref _imageFileBitmap, value); }
    }

    public BehaviorSubject<bool> IsFileChosen { get; set; } = new BehaviorSubject<bool>(false);

    public BehaviorSubject<bool> IsFileSended { get; set; } = new BehaviorSubject<bool>(false);

    public NeuralAPIResponse Response
    {
        get => _response;
        set => this.RaiseAndSetIfChanged(ref _response, value);
    }

    public string ConnectionIndicatorColor
    {
        get => _connectionIndicatorColor;
        set => this.RaiseAndSetIfChanged(ref _connectionIndicatorColor, value);
    }
    #endregion

    #region Commands
    public ReactiveCommand<Unit, Unit> GetUrlCommand { get; }

    public ReactiveCommand<Unit, Unit> ConnectionCheckCommand { get; }

    public ReactiveCommand<Unit, Unit> OpenImageFileCommand { get; }

    public ReactiveCommand<Unit, Unit> SendImageFileCommand { get; }
    #endregion

    #region Constructors
    public NeuralAPIViewModel(IScreen screen = null)
    {
        HostScreen = screen ?? Locator.Current.GetService<IScreen>();

        UrlPath = App.Current.Services.GetRequiredService<ConfigurationService>().GetNeuralApiUrl();

        _apiService = App.Current.Services.GetRequiredService<NeuralAPIService>();
        _healthCheckService = App.Current.Services.GetRequiredService<HealthCheckService>();
        _filesService = App.Current.Services.GetRequiredService<IFilesService>();
        _healthCheckService.ApiService = _apiService;
        _healthCheckService.IsConnected.Subscribe(ConnectionLostActions);
        _healthCheckService.IsConnected.Subscribe(SetConnectionIndicator);

        ConnectionCheckCommand = ReactiveCommand.CreateFromTask(ConnectionCheck);
        OpenImageFileCommand = ReactiveCommand.CreateFromTask(OpenImageFile, _healthCheckService.IsConnected);
        GetUrlCommand = ReactiveCommand.Create(GetUrl);
        SendImageFileCommand = ReactiveCommand.CreateFromTask(SendFile, IsFileChosen);
    }
    #endregion

    #region Private Methods
    private async Task OpenImageFile()
    {
        Log.Debug("NeuralAPIViewModel.OpenImageFile: Start");
        var file = await _filesService.OpenImageFileAsync();
        if (file != null)
        {
            _imageFile = file;
            ImageFile = new Bitmap(await _imageFile.OpenReadAsync());
            IsFileChosen.OnNext(true);
            Log.Debug("NeuralAPIViewModel.OpenImageFile: Done; File: Is not null");
            Log.Information("Open image: The image was received");
        }
        else
        {
            Log.Debug("NeuralAPIViewModel.OpenImageFile: Done; File: Is null");
            Log.Information("Open image: The image was not received");
        }
    }

    private async Task ConnectionCheck()
    {
        Log.Debug("NeuralAPIViewModel.ConnectionCheck: Start");
        await _healthCheckService.ExecuteAsync(_checkStopTokenSource.Token);
        Log.Debug("NeuralAPIViewModel.ConnectionCheck: Done");
    }

    private void GetUrl()
    {
        Log.Debug("NeuralAPIViewModel.GetUrl: Start");
        CancelTheToken();
        InitTheToken();
        _apiService.ApiUrl = UrlPath;
        ConnectionCheckCommand.Execute();
        Log.Debug("NeuralAPIViewModel.GetUrl: Done; UrlPath: {UrlPath}", UrlPath);
        Log.Information("Get URL: URL is {Url}", UrlPath);
    }

    private async Task SendFile()
    {
        Log.Debug("NeuralAPIViewModel.SendFile: Start");
        Response = await _apiService.SendAsync(_imageFile.Path);
        IsFileChosen.OnNext(false);
        Log.Debug("NeuralAPIViewModel.SendFile: Done; Response: {Response}", Response);
        Log.Information("Send File: Image sent");
    }


    private void ConnectionLostActions(bool isConnected)
    {
        Log.Debug("NeuralAPIViewModel.ConnectionLostActions: Start");
        if (!isConnected)
        {
            IsFileChosen.OnNext(false);
            IsFileSended.OnNext(false);
        }
        Log.Debug("NeuralAPIViewModel.ConnectionLostActions: Done");
    }

    private void CancelTheToken() => _checkStopTokenSource?.Cancel();

    private void InitTheToken() => _checkStopTokenSource = new CancellationTokenSource();

    private void SetConnectionIndicator(bool isConnection)
    {
        Log.Debug("NeuralAPIViewModel.SetConnectionIndicator: Start");
        if (isConnection) ConnectionIndicatorColor = "Green";
        else ConnectionIndicatorColor = "Red";
        Log.Debug("NeuralAPIViewModel.SetConnectionIndicator: Done; ConnectionIndicatorColor: {ConnectionIndicatorColor}", ConnectionIndicatorColor);
    }
    #endregion
}
