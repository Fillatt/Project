using AvaloniaApplication.Services;
using DataBase;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Serilog;
using Splat;
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

    private AuthorizationService _authorizationService;

    private ValidationService _validationService;
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
        _authorizationService = App.Current.Services.GetRequiredService<AuthorizationService>();
        _validationService = App.Current.Services.GetRequiredService<ValidationService>();

        StartRegisterCommand = ReactiveCommand.CreateFromTask(StartRegisterAsync);
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
        bool isValidated = GetValidation();
        if (isValidated) await StartAuthorizationAsync();

        Log.Debug("RegisterViewModel.StartRegisterAsync: Done");
        Log.Information("Registration: End");
    }

    /// <summary>
    /// Начать валидацию
    /// </summary>
    /// <returns></returns>
    private bool GetValidation()
    {
        Log.Debug("RegisterViewModel.GetValidation: Start");

        CleanErrorMessages();
        var validation = _validationService.GetValidation(new Account { Login = Login, Password = Password });
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

        var result = await _authorizationService.RegisterAsync(new Account { Login = Login, Password = Password });
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
    public void NavigateAuthentication() => _navigationService.NavigateAuthentication();
    #endregion
}
