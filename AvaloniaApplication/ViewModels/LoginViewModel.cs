using AvaloniaApplication.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Serilog;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace AvaloniaApplication.ViewModels;

public class LoginViewModel : ReactiveObject, IRoutableViewModel
{
    #region Private Fields  
    private string _title = "Login";

    private string _titleColor = "Black";

    private string _loginError;

    private string _passwordError;

    NavigationService _navigationService;

    SignalRClientAuthorizationService _signalRClientService;
    #endregion

    #region Properties
    /// <summary>
    /// Ссылка на IScreen, которому принадлежит данная модель представления
    /// </summary>
    public IScreen HostScreen { get; }

    /// <summary>
    /// Идентификатор для IRotableViewModel
    /// </summary>
    public string UrlPathSegment { get; } = "LoginViewModel";

    public string Title
    {
        get => _title;
        set { this.RaiseAndSetIfChanged(ref _title, value); }
    }

    public string TitleColor
    {
        get => _titleColor;
        set { this.RaiseAndSetIfChanged(ref _titleColor, value); }
    }

    public string Login { get; set; }

    public string Password { get; set; }

    public string LoginError
    {
        get => _loginError;
        set { this.RaiseAndSetIfChanged(ref _loginError, value); }
    }

    public string PasswordError
    {
        get => _passwordError;
        set { this.RaiseAndSetIfChanged(ref _passwordError, value); }
    }

    /// <summary>
    /// BehaviorSubject-флаг успеха аутентификации
    /// </summary>
    public static BehaviorSubject<bool> IsAuthenticated { get; set; } = new BehaviorSubject<bool>(false);
    #endregion

    #region Commands
    public ReactiveCommand<Unit, Unit> StartLoginCommand { get; }

    public ReactiveCommand<Unit, Unit> RegisterCommand { get; }
    #endregion

    #region Constructors
    public LoginViewModel(IScreen screen = null)
    {
        HostScreen = screen ?? Locator.Current.GetService<IScreen>();

        _navigationService = App.Current.Services.GetRequiredService<NavigationService>();
        _signalRClientService = App.Current.Services.GetRequiredService<SignalRClientAuthorizationService>();
        _signalRClientService.IsConnectedSubject.Subscribe(OnConnectionChange);

        StartLoginCommand = ReactiveCommand.CreateFromTask(StartLoginAsync, _signalRClientService.IsConnectedSubject);
        RegisterCommand = ReactiveCommand.Create(NavigateRegister);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Начать вход в систему
    /// </summary>
    /// <returns></returns>
    private async Task StartLoginAsync()
    {
        Log.Debug("LoginViewModel.StartLoginAsync: Start");
        Log.Information("Logining: Start");

        ClearTitle();
        bool isValidated = await GetValidationAsync();
        if (isValidated) await StartAuthorizationAsync();

        Log.Information("Logining: End");
        Log.Debug("LoginViewModel.StartLoginAsync: Done");
    }

    /// <summary>
    /// Начать валидацию
    /// </summary>
    /// <returns></returns>
    private async Task<bool> GetValidationAsync()
    {
        Log.Debug("LoginViewModel.GetValidation: Start");

        CleanErrorMessages();
        await _signalRClientService.StartValidationAsync(Login, Password);
        var validation = _signalRClientService.GetValidationResult();
        LoginError = validation.LoginError;
        PasswordError = validation.PasswordError;

        Log.Debug("LoginViewModel.GetValidation: Done; Is success: {IsSucces}", validation.IsSuccess);
        if (!validation.IsSuccess) Log.Warning("LoginViewModel.GetValidation: Validation is fail; LoginErrror: {LoginError}; PasswordError: {PasswordError}", LoginError, PasswordError);

        return validation.IsSuccess;
    }

    /// <summary>
    /// Очистить сообщения о ошибках валидации
    /// </summary>
    private void CleanErrorMessages()
    {
        Log.Debug("LoginViewModel.CleanErrorMessages: Start");

        LoginError = string.Empty;
        PasswordError = string.Empty;

        Log.Debug("LoginViewModel.CleanErrorMessages: Done");
    }

    /// <summary>
    /// Очистить заголовок
    /// </summary>
    private void ClearTitle()
    {
        Log.Debug("LoginViewModel.ClearTitle: Start");

        Title = "Login";
        TitleColor = "Black";

        Log.Debug("LoginViewModel.ClearTitle: Done");
    }

    /// <summary>
    /// Начать авторизацию
    /// </summary>
    /// <returns></returns>
    private async Task StartAuthorizationAsync()
    {
        Log.Debug("LoginViewModel.StartAuthorizationAsync: Start");
        Log.Information("Authorization: Start");

        await _signalRClientService.StartLoginAsync(Login, Password);
        var result = _signalRClientService.GetAuthorizationResult();
        if (result.IsSuccess) NavigateMain();
        else
        {
            Title = result.Message;
            TitleColor = "Red";
        }
        if (!IsAuthenticated.Value) IsAuthenticated.OnNext(result.IsSuccess);

        Log.Debug("LoginViewModel.StartAuthorizationAsync: Done; Is success: {IsSuccess}", result.IsSuccess);
        if (!result.IsSuccess) Log.Warning("LoginViewModel.StartAuthorizationAsync: Authorization is fail; Message: {Message}", result.Message);
        Log.Information("Authorization: End");
    }

    /// <summary>
    /// Переход к главному окну
    /// </summary>
    private void NavigateMain() => _navigationService.NavigateMain();


    /// <summary>
    /// Переход к окну регистрации
    /// </summary>
    private void NavigateRegister() => _navigationService.NavigateRegister();

    private void OnConnectionChange(bool isConnected)
    {
        if (isConnected) ClearTitle();
        else SetTitleConnectionError();
    }

    private void SetTitleConnectionError()
    {
        Title = "Connection is lost";
        TitleColor = "Red";
    }
    #endregion
}
