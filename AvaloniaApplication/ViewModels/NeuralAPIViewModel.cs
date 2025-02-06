using APIClient;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using AvaloniaApplication.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;
using System.Reactive;
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

    private CancellationToken _stopToken;

    private Bitmap? _chosenImageFileInBitmap;

    private IStorageFile? _chosenImageFile;

    private Bitmap? _receivedImageFileInBitmap;

    private IStorageFile? _receivedImageFile;
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

    public Bitmap? ChosenImageFile
    {
        get => _chosenImageFileInBitmap;
        set { this.RaiseAndSetIfChanged(ref _chosenImageFileInBitmap, value); }
    }

    public Bitmap? ReceivedImageFile
    {
        get => _receivedImageFileInBitmap;
        set { this.RaiseAndSetIfChanged(ref _receivedImageFileInBitmap, value); }
    }

    public BehaviorSubject<bool> IsFileChosen { get; set; } = new BehaviorSubject<bool>(false);

    public BehaviorSubject<bool> IsFileSended { get; set; } = new BehaviorSubject<bool>(false);
    #endregion

    #region Commands
    public ReactiveCommand<Unit, Unit> GetUrlCommand { get; }

    public ReactiveCommand<Unit, Unit> ConnectionCheckCommand { get; }

    public ReactiveCommand<Unit, Unit> OpenImageFileCommand { get; }

    public ReactiveCommand<Unit, Unit> SendImageFileCommand { get; }

    public ReactiveCommand<Unit, Unit> SaveImageFileCommand { get; }
    #endregion

    #region Constructors
    public NeuralAPIViewModel(IScreen screen = null)
    {
        HostScreen = screen ?? Locator.Current.GetService<IScreen>();

        _stopToken = new CancellationTokenSource().Token;

        _apiService = App.Current.Services.GetRequiredService<NeuralAPIService>();
        _healthCheckService = App.Current.Services.GetRequiredService<HealthCheckService>();
        _filesService = App.Current.Services.GetRequiredService<IFilesService>();
        _healthCheckService.ApiService = _apiService;

        ConnectionCheckCommand = ReactiveCommand.CreateFromTask(ConnectionCheck);
        OpenImageFileCommand = ReactiveCommand.CreateFromTask(OpenImageFile, _healthCheckService.IsConnected);
        GetUrlCommand = ReactiveCommand.Create(GetUrl);
        SendImageFileCommand = ReactiveCommand.CreateFromTask(SendFile, _healthCheckService.IsConnected);
    }
    #endregion

    #region Private Methods
    private async Task OpenImageFile()
    {
        _chosenImageFile = await _filesService.OpenImageFileAsync();
        ChosenImageFile = new Bitmap(await _chosenImageFile.OpenReadAsync());
    }

    private async Task ConnectionCheck() => await _healthCheckService.ExecuteAsync(_stopToken);

    private void GetUrl()
    {
        _apiService.ApiUrl = UrlPath;
        ConnectionCheckCommand.Execute();
    }

    private async Task SendFile()
    {
        var test = await _apiService.SendAsync(_chosenImageFile.Path);
    }
    #endregion
}
