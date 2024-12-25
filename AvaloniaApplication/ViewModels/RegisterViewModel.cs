using DataBase;
using ReactiveUI;
using Serilog;
using System.Reactive;
using System.Threading.Tasks;

namespace AvaloniaApplication.ViewModels;

public class RegisterViewModel : ReactiveObject, IRoutableViewModel
{
    #region Private Fields
    /// <summary>
    /// Заголовок
    /// </summary>
    private string _title = "Registration";

    /// <summary>
    /// Цвет заголовка
    /// </summary>
    private string _titleColor = "Black";

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
    /// <summary>
    /// Начать регистрация
    /// </summary>
    public ReactiveCommand<Unit, Unit> StartRegisterCommand { get; }
    #endregion

    #region Constructors
    public RegisterViewModel(IScreen screen)
    {
        StartRegisterCommand = ReactiveCommand.CreateFromTask(StartRegisterAsync);
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
        var validation = Validation.GetValidation(Login, Password);
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

        var result = await Authorization.RegisterAsync(new Account { Login = Login, Password = Password }, _db);
        Title = result.Message;
        TitleColor = result.MessageColor;

        Log.Debug("RegisterViewModel.StartAuthorizationAsync: Done; Is success: {IsSuccess}", result.IsSuccess);
        if (!result.IsSuccess) Log.Warning("RegisterViewModel.StartAuthorizationAsync: Authorization is fail; Message: {Message}", result.Message);
        Log.Information("Authorization: End");
    }
    #endregion
}
