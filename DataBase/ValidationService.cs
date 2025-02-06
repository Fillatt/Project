using Serilog;

namespace DataBase;

public class ValidationService
{
    #region Records
    /// <summary>
    /// Запись для получения результата валидации
    /// </summary>
    public record ValidationResult
    {
        public string LoginError { get; set; }

        public string PasswordError { get; set; }

        public bool IsSuccess { get; set; }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Получение результата валидации
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public ValidationResult GetValidation(Account account)
    {
        Log.Debug("Validation.GetValidation: Start");

        var loginValidation = LoginValidation(account.Login);
        var passwordValidation = PasswordValidation(account.Password);

        var result = new ValidationResult
        {
            PasswordError = passwordValidation.PasswordError,
            LoginError = loginValidation.LoginError,
            IsSuccess = loginValidation.IsSuccess && passwordValidation.IsSuccess
        };

        Log.Debug("Validation.GetValidation: Done; Result: {@Result}", result);

        return result;
    }
    #endregion

    #region Private Methods   

    /// <summary>
    /// Валидация логина
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    private ValidationResult LoginValidation(string login)
    {
        Log.Debug("Validation.LoginValidation: Start");

        ValidationResult result = new();
        if (IsEmpty(login)) result = LoginIsEmpty();
        else result.IsSuccess = true;

        Log.Debug("Validation.LoginValidation: Done; Result: {@Result}", result);

        return result;
    }

    /// <summary>
    /// Валидация пароля
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    private ValidationResult PasswordValidation(string password)
    {
        Log.Debug("Validation.PasswordValidation: Start");

        ValidationResult result = new();
        if (IsEmpty(password)) result = PasswordIsEmpty();
        else result.IsSuccess = true;

        Log.Debug("Validation.PasswordValidation: Done; Result: {@Result}", result);

        return result;
    }

    private ValidationResult PasswordIsEmpty() => GetPasswordError("Password can not be empty");

    private ValidationResult LoginIsEmpty() => GetLoginError("Login can not be empty");

    private ValidationResult GetLoginError(string message)
    {
        return new ValidationResult
        {
            LoginError = message,
            IsSuccess = false
        };
    }

    private ValidationResult GetPasswordError(string message)
    {
        return new ValidationResult
        {
            PasswordError = message,
            IsSuccess = false
        };
    }

    private bool IsEmpty(string element) => string.IsNullOrEmpty(element);
    #endregion
}
