using DataBase;
using ReactiveUI;
using Serilog;
using System;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace AvaloniaApplication.ViewModels;

public class LoginViewModel : ReactiveObject, IRoutableViewModel
{
    #region Private Fields
    /// <summary>
    /// Заголовок
    /// </summary>
    private static string _title = "Login";

    /// <summary>
    /// Цвет заголовка
    /// </summary>
    private static string _titleColor = "Black";

    /// <summary>
    /// Контекст базы данных
    /// </summary>
    private Context _db = new();

    /// <summary>
    /// Сообщение о ошибке логина
    /// </summary>
    private string _loginError;

    /// <summary>
    /// Сообщение о ошибке пароля
    /// </summary>
    private string _passwordError;
    #endregion

    #region Properties
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
    public string UrlPathSegment { get; } = "LoginViewModel";

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

    /// <summary>
    /// BehaviorSubject-флаг успеха аутентификации
    /// </summary>
    public static BehaviorSubject<bool> IsAuthenticated { get; set; } = new BehaviorSubject<bool>(false);
    #endregion

    #region Events
    /// <summary>
    /// Событие выбора регистрации
    /// </summary>
    public static event EventHandler RegisterSelected;
    #endregion

    #region Commands
    /// <summary>
    /// Начать вход в систему
    /// </summary>
    public ReactiveCommand<Unit, Unit> StartLoginCommand { get; }

    /// <summary>
    /// Выбор регистрации
    /// </summary>
    public ReactiveCommand<Unit, Unit> RegisterCommand { get; }
    #endregion

    #region Constructors
    public LoginViewModel(IScreen screen)
    {
        HostScreen = screen;
        StartLoginCommand = ReactiveCommand.CreateFromTask(StartLoginAsync);
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
        bool isValidated = GetValidation();
        if (isValidated) await StartAuthorizationAsync();

        Log.Information("Logining: End");
        Log.Debug("LoginViewModel.StartLoginAsync: Done");
    }

    /// <summary>
    /// Вызов события выбора регистрации
    /// </summary>
    private void NavigateRegister() => RegisterSelected(this, new EventArgs());

    /// <summary>
    /// Начать валидацию
    /// </summary>
    /// <returns></returns>
    private bool GetValidation()
    {
        Log.Debug("LoginViewModel.GetValidation: Start");

        CleanErrorMessages();
        var validation = Validation.GetValidation(Login, Password);
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
    /// Очистить заголово
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

        var result = await Authorization.LoginAsync(new Account { Login = Login, Password = Password }, _db);
        Title = result.Message;
        TitleColor = result.MessageColor;
        IsAuthenticated.OnNext(result.IsSuccess);

        Log.Debug("LoginViewModel.StartAuthorizationAsync: Done; Is success: {IsSuccess}", result.IsSuccess);
        if (!result.IsSuccess) Log.Warning("LoginViewModel.StartAuthorizationAsync: Authorization is fail; Message: {Message}", result.Message);
        Log.Information("Authorization: End");
    }

    #endregion
}
