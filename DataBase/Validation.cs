using Serilog;

namespace DataBase;

/// <summary>
/// Статический класс, осуществялющий валидацию
/// </summary>
public static class Validation
{
    #region Structs
    /// <summary>
    /// Структура для получения результата валидации
    /// </summary>
    public struct ValidationResult
    {
        /// <summary>
        /// Сообщение о ошибке логина
        /// </summary>
        public string LoginError { get; set; }

        /// <summary>
        /// Сообщение о ошибке пароля
        /// </summary>
        public string PasswordError { get; set; }

        /// <summary>
        /// Флаг успеха валидации
        /// </summary>
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
    public static ValidationResult GetValidation(string login, string password)
    {
        Log.Debug("Validation.GetValidation: Start");

        var loginValidation = LoginValidation(login);
        var passwordValidation = PasswordValidation(password);

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
    private static ValidationResult LoginValidation(string login)
    {
        Log.Debug("Validation.LoginValidation: Start");

        ValidationResult result = new();
        if (IsEmpty(login)) result = LoginIsEmpty();
        else if (IsDigit(login)) result = LoginStartWithNumber();        
        else result.IsSuccess = true;

        Log.Debug("Validation.LoginValidation: Done; Result: {@Result}", result);

        return result;
    }

    /// <summary>
    /// Валидация пароля
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    private static ValidationResult PasswordValidation(string password)
    {
        Log.Debug("Validation.PasswordValidation: Start");

        ValidationResult result = new();
        if (IsEmpty(password)) result = PasswordIsEmpty();
        else if (IsEnoughLength(password)) result = PasswordIsNotEnoughLength();
        else result.IsSuccess = true;

        Log.Debug("Validation.PasswordValidation: Done; Result: {@Result}", result);

        return result;
    }

    /// <summary>
    /// Получение ошибки о недостаточной длине пароля
    /// </summary>
    /// <returns></returns>
    private static ValidationResult PasswordIsNotEnoughLength() => GetPasswordError("The password must be longer than 8 characters");

    /// <summary>
    /// Получение ошибки о пустоте пароля
    /// </summary>
    /// <returns></returns>
    private static ValidationResult PasswordIsEmpty() => GetPasswordError("Password can not be empty");

    /// <summary>
    /// Получение ошибки о пустоте логина
    /// </summary>
    /// <returns></returns>
    private static ValidationResult LoginIsEmpty() => GetLoginError("Login can not be empty");

    /// <summary>
    /// Получение ошибки о начале логина с цифры
    /// </summary>
    /// <returns></returns>
    private static ValidationResult LoginStartWithNumber() => GetLoginError("Login can not start with a number");

    /// <summary>
    /// Получение ошибки логина с указанным сообщением
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private static ValidationResult GetLoginError(string message)
    {
        return new ValidationResult
        {
            LoginError = message,
            IsSuccess = false
        };
    }

    /// <summary>
    /// Получение ошибки в пароле с указанным сообщением
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private static ValidationResult GetPasswordError(string message)
    {
        return new ValidationResult
        {
            PasswordError = message,
            IsSuccess = false
        };
    }
    
    private static bool IsEnoughLength(string password) => password.Length < 8;
   
    private static bool IsEmpty(string element) => string.IsNullOrEmpty(element);

    private static bool IsDigit(string element) => char.IsDigit(element[0]);
    #endregion
}
