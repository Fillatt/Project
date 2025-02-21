using AvaloniaApplication.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Serilog;
using Splat;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace AvaloniaApplication.ViewModels;

public class RegisterViewModel : ReactiveObject, IRoutableViewModel
{
    #region Private Fields   
    private string _title = "Registration";

    private string _titleColor = "Black";

    private string _loginError;

    private string _passwordError;

    private NavigationService _navigationService;

    private SignalRClientAuthorizationService _signalRClientService;
    #endregion

    #region Public Properties
    /// <summary>
    /// Заголовок
    /// </summary>
    public string Title
    {
        get => _title;
        set { this.RaiseAndSetIfChanged(ref _title, value); }
    }

    /// <summary>
    /// Цвет заголовка
    /// </summary>
    public string TitleColor
    {
        get => _titleColor;
        set { this.RaiseAndSetIfChanged(ref _titleColor, value); }
    }

    /// <summary>
    /// Ссылка на IScreen, которому принадлежит данная модель представления
    /// </summary>
    public IScreen HostScreen { get; }

    /// <summary>
    /// Идентификатор для IRotableViewModel
    /// </summary>
    public string UrlPathSegment { get; } = "RegisterViewModel";

    /// <summary>
    /// Логин
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Сообщение о ошибке логина
    /// </summary>
    public string LoginError
    {
        get => _loginError;
        set { this.RaiseAndSetIfChanged(ref _loginError, value); }
    }

    /// <summary>
    /// Сообщение о ошибке пароля
    /// </summary>
    public string PasswordError
    {
        get => _passwordError;
        set { this.RaiseAndSetIfChanged(ref _passwordError, value); }
    }
    #endregion

    #region Commands   
    public ReactiveCommand<Unit, Unit> StartRegisterCommand { get; }

    public ReactiveCommand<Unit, Unit> AuthenticationCommand { get; }
    #endregion

    #region Constructors
    public RegisterViewModel(IScreen screen = null)
    {
        HostScreen = screen ?? Locator.Current.GetService<IScreen>();

        _navigationService = App.Current.Services.GetRequiredService<NavigationService>();
        _signalRClientService = App.Current.Services.GetRequiredService<SignalRClientAuthorizationService>();
        _signalRClientService.IsConnectedSubject.Subscribe(OnConnectionChange);

        StartRegisterCommand = ReactiveCommand.CreateFromTask(StartRegisterAsync, _signalRClientService.IsConnectedSubject);
        AuthenticationCommand = ReactiveCommand.Create(NavigateAuthentication);
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// Начать регистрацию
    /// </summary>
    /// <returns></returns>
    private async Task StartRegisterAsync()
    {
        Log.Debug("RegisterViewModel.StartRegisterAsync: Start");
        Log.Information("Registration: Start");

        ClearTitle();
        bool isValidated = await GetValidationAsync();
        if (isValidated) await StartAuthorizationAsync();

        Log.Debug("RegisterViewModel.StartRegisterAsync: Done");
        Log.Information("Registration: End");
    }

    /// <summary>
    /// Начать валидацию
    /// </summary>
    /// <returns></returns>
    private async Task<bool> GetValidationAsync()
    {
        Log.Debug("RegisterViewModel.GetValidation: Start");

        CleanErrorMessages();
        await _signalRClientService.StartValidationAsync(Login, Password);
        var validation = _signalRClientService.GetValidationResult();
        LoginError = validation.LoginError;
        PasswordError = validation.PasswordError;

        Log.Debug("RegisterViewModel.GetValidation: Done; Is success: {IsSucces}", validation.IsSuccess);
        if (!validation.IsSuccess) Log.Warning("RegisterViewModel.GetValidation: Validation is fail; LoginErrror: {LoginError}; PasswordError: {PasswordError}", LoginError, PasswordError);

        return validation.IsSuccess;
    }

    /// <summary>
    /// Очистить сообщения о ошибках валидации
    /// </summary>
    private void CleanErrorMessages()
    {
        Log.Debug("RegisterViewModel.CleanErrorMessages: Start");

        LoginError = string.Empty;
        PasswordError = string.Empty;

        Log.Debug("RegisterViewModel.CleanErrorMessages: Done");
    }

    /// <summary>
    /// Очистить заголовок
    /// </summary>
    private void ClearTitle()
    {
        Log.Debug("RegisterViewModel.ClearTitle: Start");

        Title = "Registration";
        TitleColor = "Black";

        Log.Debug("RegisterViewModel.ClearTitle: Done");
    }

    /// <summary>
    /// Начать авторизацию
    /// </summary>
    /// <returns></returns>
    private async Task StartAuthorizationAsync()
    {
        Log.Debug("RegisterViewModel.StartAuthorizationAsync: Start");
        Log.Information("Authorization: Start");

        await _signalRClientService.StartRegisterAsync(Login, Password);
        var result = _signalRClientService.GetAuthorizationResult();

        if (result.IsSuccess) NavigateAuthentication();
        else
        {
            Title = result.Message;
            TitleColor = "Red";
        }

        Log.Debug("RegisterViewModel.StartAuthorizationAsync: Done; Is success: {IsSuccess}", result.IsSuccess);
        if (!result.IsSuccess) Log.Warning("RegisterViewModel.StartAuthorizationAsync: Authorization is fail; Message: {Message}", result.Message);
        Log.Information("Authorization: End");
    }

    /// <summary>
    /// Навигация к окну аутентификации
    /// </summary>
    /// <returns></returns>
    private void NavigateAuthentication() => _navigationService.NavigateAuthentication();

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
